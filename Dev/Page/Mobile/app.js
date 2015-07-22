/**
 * Created by Craig.Y.Duan on 2015/5/21.
 */
'use strict';
var app = angular.module('zergApp', [ 'ui.router','ngCookies','oc.lazyLoad','ngStorage','ui.bootstrap']);

var SETTING = {
    BaseUrl:'http://192.168.1.199/',
    ApiUrl:'http://localhost:50766/api',
    ImgUrl:'http://img.yoopoon.com/',
    eventApiUrl:'http://localhost:16857/API'
};
//var SETTING = {
//    BaseUrl:'http://api.iyookee.cn/',
//    ApiUrl:'http://api.iyookee.cn/api',
//    ImgUrl:'http://img.yoopoon.com/'
//};

app.config(
    [        '$controllerProvider', '$compileProvider', '$filterProvider', '$provide',
        function ($controllerProvider,   $compileProvider,   $filterProvider,   $provide) {

            // lazy controller, directive and service
            app.controller = $controllerProvider.register;
            app.directive  = $compileProvider.directive;
            app.filter     = $filterProvider.register;
            app.factory    = $provide.factory;
            app.service    = $provide.service;
            app.constant   = $provide.constant;
            app.value      = $provide.value;
        }
    ])
    .constant('MAIN_CONFIG',[
    {
        name:'zergApp',
        module:false,
        files:['Common/scripts/appCtrl.js']
    }])
    .config(['$ocLazyLoadProvider', 'MAIN_CONFIG', function($ocLazyLoadProvider, MAIN_CONFIG) {
        $ocLazyLoadProvider.config({
            debug: false,
            events: false,
            modules: MAIN_CONFIG
        });
    }]);


//
//
//app.controller("appController",['$state',function($state){
//
//}]);