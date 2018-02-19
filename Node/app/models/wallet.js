var Web3Helper = require('../helpers/web3Helper.js');
var FaucetCache = require('../util/faucetCache.js');
var config = require('nconf');

class Wallet {
    constructor(address) {
        this.faucetCache = new FaucetCache();
        this.address = address;
    }

    validAUCBalance(balance) {
        return balance > config.get('MIN_AUC_FAUCET');
    }

    validETHBalance(balance) {
        return balance > config.get('MIN_ETH_FAUCET');
    }

    request(cb) {
        var self = this;
        new Web3Helper().getTokenBalance(config.get('AUC_TOKEN_ADDRESS'), this.address,
            function (err, result) {
                if (err) cb(err);
                else self.validateAndSendTokensIfApplicable(result, cb);
            });
    }

    validateAndSendTokensIfApplicable(aucBalance, cb) {
        try {
            this.validRequestTimeInterval();
            if (!this.validAUCBalance(aucBalance)) {
                this.sendAUC(cb);
            }
            if (!this.validETHBalance(new Web3Helper().getETHBalance(this.address))) {
                this.sendETH(cb);
            }
        }
        catch (err) {
            cb(err);
        }
    }

    validRequestTimeInterval() {
        if (!this.faucetCache.valid(this.address)) {
            throw new Error('Cant request tokens. Minimum request interval failed.');
        }
    }

    sendETH(cb) {
        new Web3Helper().sendETH(this.address, 0.01,
            function (err) {
                if (err) cb(err);
                else {
                    cb();
                }
            })
    }

    sendAUC(cb) {
        // new Web3Helper().sendETH(this.address, 0.01,
        //     function (err) {

        //     })
    }
}

module.exports = Wallet;
