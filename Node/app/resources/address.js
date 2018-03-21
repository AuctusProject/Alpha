var AddressController = require('../controllers/addressController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/:addressHash')
    .get(function (req, res, next) {
      new AddressController().getETHBalance(req.params.addressHash, baseResponse(res, next));
    });
};
