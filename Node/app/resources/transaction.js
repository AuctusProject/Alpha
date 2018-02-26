var TransactionController = require('../controllers/transactionController.js');
var baseResponse = require('../util/baseResponse.js');

module.exports = function (router) {
  'use strict';

  router.route('/:transactionHash')
    .get(function (req, res, next) {
      new TransactionController().getTransactionByHash(req.params.transactionHash, req.query["eventCompleteName"], baseResponse(res, next));
    });
};


// app/routes/transactions.js

// module.exports = function(router) {
//   'use strict';
//   // This will handle the url calls for /transactions/:transaction_id
//   router.route('/:transactionId')
//   .get(function(req, res, next) {
//     // Return transaction
//   }) 
//   .put(function(req, res, next) {
//     // Update transaction
//   })
//   .patch(function(req, res,next) {
//     // Patch
//   })
//   .delete(function(req, res, next) {
//     // Delete record
//   });

//   router.route('/')
//   .get(function(req, res, next) {
//     // Logic for GET /transactions routes
//   }).post(function(req, res, next) {
//     // Create new transaction

//   });
// };
