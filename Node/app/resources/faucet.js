// app/routes/transactions.js
var FaucetController = require('../controllers/faucetController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/')
    .post(function (req, res, next) {
      new FaucetController().request(req.body, baseResponse(res, next));
    });
};
