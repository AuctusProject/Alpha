var Wallet = require('../models/wallet.js');
var Web3Helper = require('../helpers/web3Helper.js');
var FaucetCache = require('../util/faucetCache.js');
var config = require('nconf');

class FaucetHelper {
    constructor() { 
        this.faucetCache = new FaucetCache();
    }

    validAUCBalance(balance) {
        return balance > config.get('MIN_AUC_FAUCET');
    }

    validETHBalance(balance) {
        return balance > config.get('MIN_ETH_FAUCET');
    }

    requestForAddress(address, cb) {
        var self = this;
        new Web3Helper().getTokenBalance(config.get('AUC_TOKEN_ADDRESS'), address,
            function (err, result) {
                if (err) cb(err);
                else self.validateAndSendTokensIfApplicable(address, result, cb);
            });
    }

    validateAndSendTokensIfApplicable(address, aucBalance, cb) {
        try {
            this.validRequestTimeInterval(address);
            if (!this.validAUCBalance(aucBalance)) {
                this.sendAUC(address, cb);
            }
            if (!this.validETHBalance(new Web3Helper().getETHBalance(address))) {
                this.sendETH(address, cb);
            }
        }
        catch (err) {
            cb(err);
        }
    }

    validRequestTimeInterval(address) {
        if (!this.faucetCache.valid(address)) {
            throw new Error('Cant request tokens. Minimum request interval failed.');
        }
    }

    sendETH(address, cb) {
        new Web3Helper().sendETH(address, 0.05, cb);
    }

    sendAUC(cb) {
        // new Web3Helper().sendETH(this.address, 0.01,
        //     function (err) {

        //     })
    }
}

module.exports = FaucetHelper;