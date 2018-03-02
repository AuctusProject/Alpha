var TransactionObject = require('../models/transactionObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var transactionRequestCache = require('../util/transactionRequestCache.js');
var config = require('nconf');
var Error = require('../util/error.js');

class Escrow {
    constructor(from, to, value) {
        this.from = from;
        this.to = to;
        this.value = value;
    }

    getEscrowResult(cb) {
        //if (!transactionRequestCache.valid(this.from + this.to, 'getEscrowResult')) {
        //    throw new Error(429, 'Too many requests.');
        //}

        var self = this;
        this.sendEscrowResultTransaction(function (err, result) {
            if (err) cb(err);
            else {
                cb(null, new TransactionObject(result));
            }
        })
    }

    sendEscrowResultTransaction(cb) {
        var valueHex = web3Helper.toHex(web3Helper.toWei(this.value));
        var data = web3Helper.getContractMethodData(config.get('PURCHASE_ESCROW_ABI'), config.get('PURCHASE_ESCROW_CONTRACT_ADDRESS'), 'escrowResult', [this.from, this.to, valueHex]);
        web3Helper.sendTransaction(config.get('GAS_PRICE'), 200000, config.get('ESCROW_OWNER_ADDRESS'), config.get('PURCHASE_ESCROW_CONTRACT_ADDRESS'), 0, data, config.get('ESCROW_OWNER_PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }
}

module.exports = Escrow;
