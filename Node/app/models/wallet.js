var FaucetHelper = require('../helpers/faucetHelper.js');
var config = require('nconf');

class Wallet {
    constructor(address) {
        this.address = address;
    }

    request(cb){
        new FaucetHelper().requestForAddress(this.address, cb);
    }
}

module.exports = Wallet;
