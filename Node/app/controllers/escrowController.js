var Escrow = require('../models/escrow.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class EscrowController {
    constructor() {}

    valid(json){
        if (!json) throw new Error(400, 'no body in request');
        if (!json.from) throw new Error(400, 'from address is mandatory');
        if (!json.to) throw new Error(400, 'to address is mandatory');
        if (!json.value) throw new Error(400, 'value is mandatory');
        if (!web3Helper.isAddress(json.from)) throw new Error(400, 'from address is not a valid ethereum address');
        if (!web3Helper.isAddress(json.to)) throw new Error(400, 'to address is not a valid ethereum address');
    }

    escrowResult(json, cb) {
        this.valid(json);
        new Escrow(json.from, json.to, json.value).getResult(cb);
    }
}

module.exports = EscrowController;