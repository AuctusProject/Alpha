var express = require('express');
var config = require('nconf');
var Error = require('../util/error.js');

module.exports = function (app) {
  'use strict';

  var authentication = function (req, res, next) {
    if (!req.headers["x-api-key"] || req.headers["x-api-key"] != config.get('API_KEY')) {
      throw new Error(401, 'request unauthorized');
    }
    next(); 
  }

  app.use(authentication);
};

