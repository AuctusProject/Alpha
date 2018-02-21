class TransactionObject {
    constructor(transactionHash, blockNumber, blockHash, contractAddress, status) {
        this.transactionHash = transactionHash;
        this.blockNumber = blockNumber;
        this.blockHash = blockHash;
        this.contractAddress = contractAddress;
        this.status = status;
    }
}

module.exports = TransactionObject;
