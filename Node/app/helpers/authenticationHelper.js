var express = require('express');
var config = require('nconf');
var Error = require('../util/error.js');

module.exports = function (app) {
  'use strict';

  var authentication = function (req, res, next) {
    if (req.url.indexOf("api/v1/address") >= 0){
      next();
    }
    else if (!req.headers["x-api-key"] || req.headers["x-api-key"] != config.get('API_KEY')) {
      throw new Error(401, 'request unauthorized');
    }
    else {
      next();
    } 
  }

  app.use(authentication);
};

