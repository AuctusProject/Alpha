var TransactionObject = require('../models/transactionObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var transactionRequestCache = require('../util/transactionRequestCache.js');
var config = require('nconf');
var Error = require('../util/error.js');

class Wallet {
    constructor(address) {
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
        web3Helper.getTokenBalance(config.get('AUC_CONTRACT_ADDRESS'), this.address,
            function (err, result) {
                if (err) cb(err);
                else self.validateAndSendTokensIfApplicable(result, cb);
            });
    }

    validateAndSendTokensIfApplicable(aucBalance, cb) {
        var self = this;
        try {
            this.validRequestTimeInterval('faucet');
            if (!this.validETHBalance(web3Helper.getETHBalance(this.address))) {
                this.sendETH(function (err, result) {
                    if (err) cb(err);
                    else self.validateAndSendAUC(aucBalance, cb);
                });
            }
            else {
                self.validateAndSendAUC(aucBalance, cb);
            }
        }
        catch (err) {
            cb(err);
        }
    }

    validateAndSendAUC(aucBalance, cb) {
        var self = this;
        if (!this.validAUCBalance(aucBalance)) {
            this.sendAUC(function (err, result) {
                if (err) cb(err);
                else {
                    cb(null, new TransactionObject(result));
                }
            });
        }
    }

    validRequestTimeInterval(method) {
        if (!transactionRequestCache.valid(this.address, method)) {
            throw new Error(429, 'Too many requests.');
        }
    }

    sendAUC(cb) {
        var aucToSendHex = web3Helper.toHex(web3Helper.toWei(config.get('AUC_SEND_FAUCET')));
        var transactionCount = web3Helper.getTransactionCount(config.get('OWNER_ADDRESS'));
        var nonce = web3Helper.toHex(transactionCount + 1);
        var data = web3Helper.getContractMethodData(config.get('AUC_TOKEN_ABI'), config.get('AUC_CONTRACT_ADDRESS'), 'mint', [this.address, aucToSendHex]);
        web3Helper.sendTransaction(config.get('GAS_PRICE') + 1, 100000, config.get('OWNER_ADDRESS'), config.get('AUC_CONTRACT_ADDRESS'), 0, data, config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb, nonce);
    }

    sendETH(cb) {
        web3Helper.sendTransaction(config.get('GAS_PRICE'), 21000, config.get('OWNER_ADDRESS'), this.address, config.get('ETH_SEND_FAUCET'), '', config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }
}

module.exports = Wallet;
