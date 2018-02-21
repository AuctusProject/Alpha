var Wallet = require('../models/wallet.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class FaucetController {
    constructor() {}

    valid(json){
        if (!json) throw new Error(400, 'no body in request');
        if (!json.address) throw new Error(400, 'address is mandatory');
        if (!web3Helper.isAddress(json.address)) throw new Error(400, 'not a valid ethereum address');
    }

    request(json, cb) {
        this.valid(json);
        new Wallet(json.address).request(cb);
    }
}

module.exports = FaucetController;