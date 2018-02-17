// config/initializers/server.js

var Web3 = require('web3');
var Web3Utils = require('web3-utils');
var config = require('nconf');

class Web3Helper {
  constructor() {
    console.log(config.get("INFURA_URL"));
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber() {
    return this._web3.eth.blockNumber;
  }

  getTokenBalance(tknContractAddress, accountAddrs, cb) {
    this._web3.eth.call({
      to: tknContractAddress, // Contract address, used call the token balance of the address in question
      data: '0x70a08231000000000000000000000000' + (accountAddrs).substring(2) // Combination of contractData and tknAddress, required to call the balance of an address 
    }, function (err, result) {
      if (result) {
        var tokens = Web3Utils.toBN(result).toString(); // Convert the result to a usable number string
        var tokensEther = Web3Utils.fromWei(tokens, 'ether');
        cb(parseFloat(tokensEther));
      }
      else {
        cb(0);
      }
    });
  }

  static IsAddress(address) {
    return Web3Utils.isAddress(address);
  }
}

module.exports = Web3Helper;

