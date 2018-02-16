// config/initializers/server.js

var Web3 = require('web3');
var config = require('nconf');

class Web3Helper {
  constructor(){
    console.log(config.get("INFURA_URL"));
    this._web3 = new Web3(new Web3.providers.HttpProvider(config.get("INFURA_URL")));
  }

  getBlockNumber(){
    return this._web3.eth.blockNumber;
  }
}

// var init =  function() {
//   'use strict'; 

  

//   var web3Helper = {};
  
//   web3Helper.getBlockNumber = function(){
//     return _web3.eth.blockNumber;
//   }

//   return web3Helper;
// };

module.exports = Web3Helper;

