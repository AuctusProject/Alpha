// config/initializers/server.js

var Web3 = require('web3');
var Web3Utils = require('web3-utils');
var config = require('nconf');

class Web3Helper {
  constructor(){
    console.log(config.get("INFURA_URL"));
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber(){
    return this._web3.eth.blockNumber;
  }

  static IsAddress(address){
    return Web3Utils.isAddress(address);
  }
}

module.exports = Web3Helper;

