// config/initializers/server.js

var Web3 = require('web3');
var Web3Utils = require('web3-utils');
var config = require('nconf');
const Tx = require('ethereumjs-tx');
var Error = require('../util/error.js');

class Web3Helper {
  constructor() {
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber() {
    return this._web3.eth.blockNumber;
  }

  getTransactionCount(address) {
    return this._web3.eth.getTransactionCount(address);
  }

  getTransaction(hash, cb) {
    this._web3.eth.getTransaction(hash, cb);
  }

  getTransactionReceipt(hash, cb) {
    this._web3.eth.getTransactionReceipt(hash, cb);
  }

  getTransactionByHash(hash, eventCompleteName, cb) {
    var self = this;
    this.getTransactionReceipt(hash,
      function (err, result) {
        if (err) cb(err);
        else if (!result) {
          self.getTransaction(hash,
            function (err, result) {
              if (err) cb(err);
              else if (!result) {
                cb(new Error(404, 'transaction not found'));
              }
              else {
                cb(null, result);
              }
            })
        }
        else {
          result["eventData"] = self.parseTransactionLog(eventCompleteName, result);
          cb(null, result);
        }
      })
  }
  
  parseTransactionLog(eventCompleteName, result) {
    if (eventCompleteName && result.logs && result.logs.length > 0) {
      var topic = this._web3.sha3(eventCompleteName);
      
      var parameters = eventCompleteName.split("(")[1];
      if (parameters)
        parameters = parameters.split(")")[0];
      if (parameters)
        parameters = parameters.split(",");
        
      if (parameters && parameters.length > 0 && parameters[0]) {
        for(var i = 0; i < result.logs.length; ++i) {
          if (result.logs[i].topics[0] == topic) {
            var remainingTopicsArguments = result.logs[i].topics.length - 1;
            var remainingDataArguments = parameters.length - remainingTopicsArguments;
            var dataArgumentsCount = remainingDataArguments;
            var formattedData = "";
            if (remainingDataArguments > 0) {
              formattedData = result.logs[i].data.substr(2);
            }
            var events = [];
            for(var j = 0; j < parameters.length; ++j) {
              var argument = null;
              if (remainingTopicsArguments > 0) {
                argument = result.logs[i].topics[result.logs[i].topics.length - remainingTopicsArguments].substr(2);
                remainingTopicsArguments = remainingTopicsArguments - 1;
              } else if (remainingDataArguments > 0) {
                argument = formattedData.substr((dataArgumentsCount - remainingDataArguments) * 64, 64);
                remainingDataArguments = remainingDataArguments - 1;
              }
              if (argument) {
                if (parameters[j] == "address") {
                  events.push('0x' + argument.substr(24));
                } else {
                  events.push(this._web3.toBigNumber('0x' + argument).toString(10));
                }
              }
            }
            return events;
          }
        }
      }
    }
    return null;
  }

  parseEventParameter(parameter) {
    return "," + (this.isAddress(parameter) ? parameter : this._web3.toHex(parameter));
  }

  getETHBalance(address) {
    var result = this._web3.eth.getBalance(address);
    var balanceBN = Web3Utils.toBN(result).toString(); // Convert the result to a usable number string
    var ether = Web3Utils.fromWei(balanceBN, 'ether');
    return parseFloat(ether);
  }

  getTokenBalance(tknContractAddress, accountAddrs, cb) {
    this._web3.eth.call({
      to: tknContractAddress, // Contract address, used call the token balance of the address in question
      data: '0x70a08231000000000000000000000000' + (accountAddrs).substring(2) // Combination of contractData and tknAddress, required to call the balance of an address 
    }, function (err, result) {
      if (result) {
        var tokens = Web3Utils.toBN(result).toString(); // Convert the result to a usable number string
        var tokensEther = Web3Utils.fromWei(tokens, 'ether');
        cb(err, parseFloat(tokensEther));
      }
      else {
        cb(err);
      }
    });
  }

  setWhitelistChain() {
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("WHITELIST_INFURA")));
  }

  getContractMethodData(abi, contractAddress, method, params) {
    var contractInstance = this._web3.eth.contract(JSON.parse(abi)).at(contractAddress);
    var data = contractInstance[method].getData.apply(null, params);
    return data;
  }

  toWei(value, unit) {
    return this._web3.toWei(value, unit);
  }

  toHex(value) {
    return this._web3.toHex(value);
  }

  toBigNumber(hexValue) {
    return hexValue ? this._web3.toBigNumber(hexValue) : null;
  }

  sendTransaction(gasPrice, gasLimit, from, to, value, data, pk, chainId, cb) {
    const gasPriceWei = this._web3.toWei(gasPrice, 'gwei');
    const valueWei = this._web3.toWei(value, 'ether');
    const rawTx = {
      nonce: this._web3.toHex(this._web3.eth.getTransactionCount(from)),
      gasPrice: this._web3.toHex(gasPriceWei),
      gasLimit: this._web3.toHex(gasLimit),
      to: to,
      value: this._web3.toHex(valueWei),
      data: data,
      chainId: this._web3.toHex(chainId)
    };
    const tx = new Tx(rawTx);
    const privateKey = Buffer.from(pk, 'hex');
    tx.sign(privateKey);
    const serializedTx = tx.serialize();
    this._web3.eth.sendRawTransaction('0x' + serializedTx.toString('hex'), cb);
  }

  isAddress(address) {
    return Web3Utils.isAddress(address);
  }
}

module.exports = new Web3Helper();

