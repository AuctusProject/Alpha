var TransactionObject = require('../models/transactionObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var transactionRequestCache = require('../util/transactionRequestCache.js');
var config = require('nconf');
var Error = require('../util/error.js');

class EscrowHelper {
    constructor() { }

    validRequestTimeInterval(address, method) {
        if (!transactionRequestCache.valid(address, method)) {
            throw new Error(429, 'Too many requests.');
        }
    }

    getEscrowResult(from, to, value, cb) {
        if (!this.validRequestTimeInterval(from, 'purchaseEscrow')){
            throw new Error(429, 'Too many requests.');
        }
        
        var valueHex = web3Helper.toHex(web3Helper.toWei(value));
        var data = web3Helper.getContractMethodData(config.get('PURCHASE_ESCROW_ABI'), config.get('PURCHASE_ESCROW_CONTRACT_ADDRESS'), 'escrowResult', [from, to, valueHex]);
        web3Helper.sendTransaction(config.get('GAS_PRICE'), 200000, config.get('OWNER_ADDRESS'), config.get('PURCHASE_ESCROW_CONTRACT_ADDRESS'), 0, data, config.get('PRIVATE_KEY'), config.get('CHAIN_ID'), cb);
    }
}

module.exports = EscrowHelper;