var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/Error.js');

class TransactionObject {
    constructor(transactionHash, blockNumber, blockHash, contractAddress, status) {
        this.transactionHash = transactionHash;
        this.blockNumber = blockNumber;
        this.blockHash = blockHash;
        this.contractAddress = contractAddress;
        this.status = status;
    }

    static GetByHash(hash, cb) {
        web3Helper.getTransactionByHash(hash,
            function (err, result) {
                if (err) cb(err);
                else {
                    cb(null, new TransactionObject(result.transactionHash || result.hash,
                        result.blockNumber,
                        result.blockHash,
                        result.contractAddress,
                        result.status));
                }
            }
        )
    }
}

module.exports = TransactionObject;
