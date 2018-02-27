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
                    var st = web3Helper.toBigNumber(result.status);
                    cb(null, new TransactionObject(result.transactionHash || result.hash,
                        result.blockNumber,
                        result.blockHash,
                        result.contractAddress,
                        (st ? parseInt(st) : null),
                        result.eventData));
                }
            }
        )
    }
}

module.exports = TransactionObject;
