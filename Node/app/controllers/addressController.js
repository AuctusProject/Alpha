var Address = require('../models/address.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class AddressController {
    constructor() {}

    getETHBalance(addressHash, cb) {
        if (!addressHash) throw new Error(400, 'address is mandatory');
        
        new Address(addressHash).getETHBalance(cb);
    }
}

module.exports = AddressController;