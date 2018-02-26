var EventController = require('../controllers/eventController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/')
    .post(function (req, res, next) {
      new EventController().getEventLog(req.body, baseResponse(res, next));
    });
};