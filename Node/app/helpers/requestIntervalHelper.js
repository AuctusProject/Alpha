var express = require('express');
var config = require('nconf');
var transactionRequestCache = require('../util/transactionRequestCache.js');

module.exports = function (app) {
  'use strict';

  var requestInterval = function (req, res, next) {
    console.log("Implement request interval !");
    // if(!transactionRequestCache.valid(req.body.toString(), 'getEscrowResult')){

    // }
    next(); // Passing the request to the next handler in the stack.
  }

  app.use(requestInterval);
};

