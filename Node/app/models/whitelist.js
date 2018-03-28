var web3Helper = require('../helpers/web3Helper.js');
var config = require('nconf');
var Error = require('../util/error.js');

class Whitelist {
    constructor(address) {
        this.address = address;
    }

    request(cb) {
        var data = "0x32fac3a3000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000a00000000000000000000000000000000000000000000000000000000000000001000000000000000000000000";
        data = data + this.address.substr(2);
        web3Helper.setWhitelistChain();
        web3Helper.sendTransaction(3, 50000, config.get('WHITELIST_OWNER'), config.get('WHITELIST_CONTRACT'), 0, data, config.get('WHITELIST_OWNER_KEY'), config.get('WHITELIST_CHAIN_ID'), cb);
    
        cb(null, { "ok": true });
    }
}

module.exports = Whitelist;
