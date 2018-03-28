var WhitelistController = require('../controllers/whitelistController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/')
    .post(function (req, res, next) {
      new WhitelistController().request(req.body, baseResponse(res, next));
    });
};
