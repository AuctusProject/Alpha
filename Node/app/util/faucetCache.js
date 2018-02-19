var config = require('nconf');

class FaucetCache {
    constructor() {
        this.cache = {};
    }

    dateDiffMinutes(date1, date2){
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffMinutes = Math.ceil(timeDiff / (1000 * 3600)); 
        return diffMinutes;
    }

    valid(address){
        if (!this.cache[address]) {
            this.cache[address] = new Date();
            return true;
        }
        if (this.dateDiffMinutes(this.cache[address], new Date()) > config.get('MIN_TIME_BETWEEN_FAUCET_REQUESTS')){
            this.cache[address] = new Date();
            return true;
        }
        return false;
    }
}

module.exports = FaucetCache;
