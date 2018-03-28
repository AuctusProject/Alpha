var Whitelist = require('../models/whitelist.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class WhitelistController {
    constructor() {}

    valid(json){
        if (!json) throw new Error(400, 'no body in request');
        if (!json.address) throw new Error(400, 'address is mandatory');
        if (!web3Helper.isAddress(json.address)) throw new Error(400, 'not a valid ethereum address');
    }

    request(json, cb) {
        this.valid(json);
        new Whitelist(json.address).request(cb);
    }
}

module.exports = WhitelistController;