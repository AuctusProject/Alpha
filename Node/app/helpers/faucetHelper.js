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
            // if (!this.validETHBalance(new Web3Helper().getETHBalance(address))) {
            //     this.sendETH(address, cb);
            // }
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
        new Web3Helper().sendETH(address, config.get('ETH_SEND_FAUCET'), cb);
    }

    sendAUC(address, cb) {
        var web3Helper = new Web3Helper();
        var aucToSendHex = web3Helper.toHex(web3Helper.toWei(config.get('AUC_SEND_FAUCET')));
        var data = web3Helper.getContractMethodData(config.get('AUC_TOKEN_ABI'), config.get('AUC_TOKEN_ADDRESS'), 'mint', [address, aucToSendHex]);
        web3Helper.sendTransaction(1, 50000, config.get('AUC_TOKEN_OWNER'), config.get('AUC_TOKEN_ADDRESS'), 0, data, config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }

    sendETH(to, value, cb) {
        this.sendTransaction(1, 21000, to, value, '', cb);
    }
}

module.exports = FaucetHelper;