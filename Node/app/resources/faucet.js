// app/routes/transactions.js
var Web3Helper = require('../helpers/web3Helper.js');
var FaucetController = require('../controllers/faucetController.js');
var web3Helper = new Web3Helper();

module.exports = function(router) {
  'use strict';

  router.route('/')
  .post(function(req, res, next) {
    new FaucetController().request(req.body, function(err){
      if (err) next(err);
      else res.end("OK");
    });
  });
};
