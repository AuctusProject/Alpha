// app/routes/transactions.js
var FaucetController = require('../controllers/faucetController.js');

module.exports = function (router) {
  'use strict';

  router.route('/')
    .post(function (req, res, next) {
      new FaucetController().request(req.body,
        function (err, result) {
          if (err) next(err);
          else {
            res.status(200).json(result).end();
          };
        });
    });
};
