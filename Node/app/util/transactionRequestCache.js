var config = require('nconf');

class TransactionRequestCache {
    constructor() {
        this.cache = {};
    }

    dateDiffMinutes(date1, date2){
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffMinutes = Math.floor((timeDiff/1000)/60);
        return diffMinutes;
    }

    valid(address, method){
        if (!this.cache[address] || !this.cache[address][method]) {
            this.update(address, method);
            return true;
        }
        if (this.dateDiffMinutes(this.cache[address][method], new Date()) >= config.get('MIN_TIME_BETWEEN_REQUESTS')){
            this.update(address, method);
            return true;
        }
        return false;
    }

    update(address, method){
        if (!this.cache[address]){
            this.cache[address] = {};
        }
        this.cache[address][method] = new Date();
    }
}

module.exports = new TransactionRequestCache();
