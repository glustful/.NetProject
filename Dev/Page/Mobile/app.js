/**
 * Created by Craig.Y.Duan on 2015/5/21.
 */
var app = angular.module('zergApp', [ 'ui.router','oc.lazyLoad']);

var SETTING = {
    BaseUrl:'http://localhost:50597/',
    ApiUrl:'http://localhost:50597/api',
    ImgUrl:'http://img.yoopoon.com/'
};

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
        name:'app',
        module:false,
        files:['Common/scripts/appCtrl.js']
    }])
    .config(['$ocLazyLoadProvider', 'MAIN_CONFIG', function($ocLazyLoadProvider, MAIN_CONFIG) {
        $ocLazyLoadProvider.config({
            debug: false,
            events: false,
            modules: MAIN_CONFIG
        });
    }])


//
//
//app.controller("appController",['$state',function($state){
//
//}]);