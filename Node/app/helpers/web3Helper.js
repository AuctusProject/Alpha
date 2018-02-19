// config/initializers/server.js

var Web3 = require('web3');
var Web3Utils = require('web3-utils');
var config = require('nconf');
const Tx = require('ethereumjs-tx');

class Web3Helper {
  constructor() {
    console.log(config.get("INFURA_URL"));
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber() {
    return this._web3.eth.blockNumber;
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

  sendAUC() {

  }

  sendETH(to, value, cb) {
    this.sendTransaction(6, 21000, to, value, '', cb);
  }

  sendTransaction(gasPrice, gasLimit, to, value, data, cb) {
    var gasPriceGwei = this._web3.toWei(gasPrice, 'gwei');
    const rawTx = {
      nonce: this._web3.toHex(this._web3.eth.getTransactionCount(config.get('AUC_TOKEN_OWNER'))),
      gasPrice: this._web3.toHex(gasPriceGwei),
      gasLimit: this._web3.toHex(gasLimit),
      to: to,
      value: this._web3.toHex(this._web3.toWei(value, 'ether')),
      data: data,
      chainId: config.get('CHAIN_ID')
    };
    const tx = new Tx(rawTx);
    const privateKey = Buffer.from(config.get('PRIVATE_KEY'), 'hex');
    tx.sign(privateKey);
    const serializedTx = tx.serialize();
    this._web3.eth.sendRawTransaction('0x' + serializedTx.toString('hex'),
      function (err, result) {
        if (err) cb(err);
        else {

        }
      });
  }

  static IsAddress(address) {
    return Web3Utils.isAddress(address);
  }
}

module.exports = Web3Helper;

