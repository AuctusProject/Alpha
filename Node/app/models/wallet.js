var Web3Helper = require('../helpers/web3Helper.js');
var config = require('nconf');

class Wallet {
    constructor(address) {
        this.address = address;
    }

    checkBalance(balance) {
        if (balance <= config.get('MIN_AUC_FAUCET')) {
            throw new Error('AUC balance sufficient. Not sending more AUC');
        }
    }

    request(cb) {
        var self = this;
        new Web3Helper().getTokenBalance(config.get('AUC_TOKEN_ADDRESS'), this.address,
            function (balance) {
                try {
                    self.checkBalance(balance);
                    cb();
                }
                catch (err) {
                    cb(err);
                }
            });
    }
}

module.exports = Wallet;
