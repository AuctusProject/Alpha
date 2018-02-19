// app/routes/transactions.js
var Web3Helper = require('../helpers/web3Helper.js');
var web3Helper = new Web3Helper();

module.exports = function(router) {
  'use strict';
  // This will handle the url calls for /transactions/:transaction_id
  router.route('/:transactionId')
  .get(function(req, res, next) {
    // Return transaction
  }) 
  .put(function(req, res, next) {
    // Update transaction
  })
  .patch(function(req, res,next) {
    // Patch
  })
  .delete(function(req, res, next) {
    // Delete record
  });

  router.route('/')
  .get(function(req, res, next) {
    // Logic for GET /transactions routes
  }).post(function(req, res, next) {
    // Create new transaction

  });
};
