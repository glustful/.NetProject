'use strict';

/**
 * @ngdoc overview
 * @name app
 * @description
 * # app
 *
 * Main module of the application.
 */
angular
  .module('app', [
    'ngAnimate',
    'ngAria',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngSanitize',
    'ngTouch',
    'ngMaterial',
    'ngStorage',
    'ngStore',
    'ui.router',
    'ui.utils',
    'ui.bootstrap',
    'ui.load',
    'ui.jp',
    'pascalprecht.translate',
    'oc.lazyLoad',
    'angular-loading-bar',
    'ng.ueditor'
  ]);

var SETTING = {
  ApiUrl:'http://localhost:50597/api',
  TradingApiUrl:'http://localhost:50597/api'
};