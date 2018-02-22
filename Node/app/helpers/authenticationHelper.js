var express = require('express');
var config = require('nconf');

module.exports = function (app) {
  'use strict';

  var authentication = function (req, res, next) {
    console.log("Implement authentication !");
    next(); // Passing the request to the next handler in the stack.
  }

  app.use(authentication);
};

