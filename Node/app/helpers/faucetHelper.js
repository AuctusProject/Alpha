var Wallet = require('../models/wallet.js');
var TransactionObject = require('../models/transactionObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var faucetCache = require('../util/faucetCache.js');
var config = require('nconf');
var Error = require('../util/error.js');

class FaucetHelper {
    constructor() { }

    validAUCBalance(balance) {
        return balance > config.get('MIN_AUC_FAUCET');
    }

    validETHBalance(balance) {
        return balance > config.get('MIN_ETH_FAUCET');
    }

    requestForAddress(address, cb) {
        var self = this;
        web3Helper.getTokenBalance(config.get('AUC_TOKEN_ADDRESS'), address,
            function (err, result) {
                if (err) cb(err);
                else self.validateAndSendTokensIfApplicable(address, result, cb);
            });
    }

    validateAndSendTokensIfApplicable(address, aucBalance, cb) {
        var self = this;
        try {
            this.validRequestTimeInterval(address);
            if (!this.validETHBalance(web3Helper.getETHBalance(address))) {
                this.sendETH(address,
                    function (err, result) {
                        if (err) cb(err);
                        else self.validateAndSendAUC(address, aucBalance, cb);
                    });
            }
            else {
                self.validateAndSendAUC(address, aucBalance, cb);
            }
        }
        catch (err) {
            cb(err);
        }
    }

    validateAndSendAUC(address, aucBalance, cb) {
        if (!this.validAUCBalance(aucBalance)) {
            this.sendAUC(address,
                function (err, result) {
                    if (err) cb(err);
                    else {
                        cb(null, new TransactionObject(result));
                    }
                });
        }
    }

    validRequestTimeInterval(address) {
        if (!faucetCache.valid(address)) {
            throw new Error(429, 'Too many requests.');
        }
    }

    sendETH(address, cb) {
        web3Helper.sendETH(address, config.get('ETH_SEND_FAUCET'), cb);
    }

    sendAUC(address, cb) {
        var aucToSendHex = web3Helper.toHex(web3Helper.toWei(config.get('AUC_SEND_FAUCET')));
        var data = web3Helper.getContractMethodData(config.get('AUC_TOKEN_ABI'), config.get('AUC_TOKEN_ADDRESS'), 'mint', [address, aucToSendHex]);
        web3Helper.sendTransaction(config.get('GAS_PRICE')+1, 50000, config.get('AUC_TOKEN_OWNER'), config.get('AUC_TOKEN_ADDRESS'), 0, data, config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }

    sendETH(address, cb) {
        web3Helper.sendTransaction(config.get('GAS_PRICE'), 21000, config.get('AUC_TOKEN_OWNER'), address, config.get('ETH_SEND_FAUCET'), '', config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }
}

module.exports = FaucetHelper;