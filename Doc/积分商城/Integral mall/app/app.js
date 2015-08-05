/**
 * Created by YUNJOY on 2015/7/27.
 */
var app=angular.module('intMallApp',[ 'ui.router','ngCookies','oc.lazyLoad','ngStorage','ui.bootstrap']);
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
