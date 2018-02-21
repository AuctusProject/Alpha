var EscrowHelper = require('../helpers/escrowHelper.js');
var config = require('nconf');

class Escrow {
    constructor(from, to, value) {
        this.from = from;
        this.to = to;
        this.value = value;
    }

    getResult(cb){
        new EscrowHelper().getEscrowResult(this.from, this.to, this.value, cb);
    }
}

module.exports = Escrow;
