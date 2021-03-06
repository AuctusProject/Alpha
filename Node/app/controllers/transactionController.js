var TransactionObject = require('../models/transactionObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class TransactionController {
    constructor() {}

    getTransactionByHash(hash, eventCompleteName, cb) {
        if (!hash) throw new Error(400, 'transaction hash is mandatory');
        
        TransactionObject.GetByHash(hash, eventCompleteName, cb);
    }
}

module.exports = TransactionController;