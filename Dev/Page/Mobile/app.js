/**
 * Created by Craig.Y.Duan on 2015/5/21.
 */
var app = angular.module('zergApp', [ 'ui.router','oc.lazyLoad','ngStorage']);

var SETTING = {
    BaseUrl:'http://localhost:50597/',
    ApiUrl:'http://localhost:50597/api',
    ImgUrl:'http://img.yoopoon.com/'
};
app.directive('draggable', function($document) {
    var startX=0, startY=0, x = 0, y = 0;
    return function(scope, element, attr) {
        element.css({
            position: 'relative',
            border: '1px solid red',
            backgroundColor: 'white',
            cursor: 'pointer'
        });
        element.bind('mousedown', function(event) {
            startX = event.screenX - x;
            startY = event.screenY - y;
            $document.bind('mousemove', mousemove);
            $document.bind('mouseup', mouseup);
        });

        function mousemove(event) {
            y = event.screenY - startY;
            x = event.screenX - startX;
            element.css({
                top: y + 'px',
                left:  x + 'px'
            });
        }

        function mouseup() {
            $document.unbind('mousemove', mousemove);
            $document.unbind('mouseup', mouseup);
        }
    }
});

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