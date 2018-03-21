var web3Helper = require('../helpers/web3Helper.js');
var config = require('nconf');
var Error = require('../util/error.js');

class Address {
    constructor(address) {
        this.address = address;
    }

    getETHBalance(cb) {
        var balance = web3Helper.getETHBalance(this.address);
        cb(null, {
            "ethBalance": balance
        });
    }
}

module.exports = Address;
