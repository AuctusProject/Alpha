// config/initializers/server.js

var Web3 = require('web3');
var Web3Utils = require('web3-utils');
var config = require('nconf');
const Tx = require('ethereumjs-tx');

class Web3Helper {
  constructor() {
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber() {
    return this._web3.eth.blockNumber;
  }

  getTransactionCount(address){
    return this._web3.eth.getTransactionCount(address);
  }

  getTransaction(hash, cb) {
    this._web3.eth.getTransaction(hash, cb);
  }

  getTransactionReceipt(hash, cb) {
    this._web3.eth.getTransactionReceipt(hash, cb);
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

  sendTransaction(gasPrice, gasLimit, from, to, value, data, pk, chainId, cb, nonce) {
    const gasPriceWei = this._web3.toWei(gasPrice, 'gwei');
    const valueWei = this._web3.toWei(value, 'ether');
    const rawTx = {
      nonce: nonce ? nonce : this._web3.toHex(this._web3.eth.getTransactionCount(from)),
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

