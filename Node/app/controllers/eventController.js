var EventObject = require('../models/eventObject.js');
var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/error.js');

class EventController {
    constructor() {}

    getEventLog(json, cb) {
        if (!json) throw new Error(400, 'no body in request');
        if (!json.contractAddress) throw new Error(400, 'contract address is mandatory');
        if (!json.eventCompleteName) throw new Error(400, 'event complete name is mandatory');
        if (!web3Helper.isAddress(json.contractAddress)) throw new Error(400, 'contract address is not a valid ethereum address');
        
        EventObject.GetEventLog(json.contractAddress, json.eventCompleteName, json.filterParameters, cb);
    }
}

module.exports = EventController;