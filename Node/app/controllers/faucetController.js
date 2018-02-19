var Wallet = require('../models/wallet.js');
var Web3Helper = require('../helpers/web3Helper.js');

class faucetController {
    constructor() {}

    valid(json){
        if (!json) throw new Error('no body in request');
        if (!json.address) throw new Error('address is mandatory');
        if (!Web3Helper.IsAddress(json.address)) throw new Error('not a valid ethereum address');
    }

    request(json, cb) {
        this.valid(json);
        new Wallet(json.address).request(cb);
    }
}

module.exports = faucetController;