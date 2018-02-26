var web3Helper = require('../helpers/web3Helper.js');
var Error = require('../util/Error.js');

class EventObject {
    constructor() {
        
    }

    static GetEventLog(contractAddress, eventCompleteName, filterParameters, cb) {
        web3Helper.getEventLog(contractAddress, eventCompleteName, filterParameters,
            function (err, result) {
                if (err) cb(err);
                else {
                    console.log(result);
                    console.log(JSON.stringify(result));
                    cb(null, new EventObject());
                }
            }
        )
    }
}

module.exports = EventObject;
