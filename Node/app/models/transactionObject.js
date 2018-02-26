var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/Error.js');

class TransactionObject {
    constructor(transactionHash, blockNumber, blockHash, contractAddress, status, eventData) {
        this.transactionHash = transactionHash;
        this.blockNumber = blockNumber;
        this.blockHash = blockHash;
        this.contractAddress = contractAddress;
        this.status = status;
        this.eventData = eventData;
    }

    static GetByHash(hash, eventCompleteName, cb) {
        web3Helper.getTransactionByHash(hash, eventCompleteName,
            function (err, result) {
                if (err) cb(err);
                else {
                    
                    cb(null, new TransactionObject(result.transactionHash || result.hash,
                        result.blockNumber,
                        result.blockHash,
                        result.contractAddress,
                        web3Helper.toBigNumber(result.status),
                        result.eventData));
                }
            }
        )
    }
}

module.exports = TransactionObject;
