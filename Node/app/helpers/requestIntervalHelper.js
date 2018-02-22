var express = require('express');
var config = require('nconf');

module.exports = function (app) {
  'use strict';

  var requestInterval = function (req, res, next) {
    console.log("Implement request interval !");
    next(); // Passing the request to the next handler in the stack.
  }

  app.use(requestInterval);
};

