var EscrowController = require('../controllers/escrowController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/')
    .post(function (req, res, next) {
      new EscrowController().escrowResult(req.body, baseResponse(res, next));
    });
};
