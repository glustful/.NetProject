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
            name:'intMallApp',
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
app.config(['$stateProvider','$urlRouterProvider','MAIN_CONFIG',function($stateProvider,$urlRouterProvider,MAIN_CONFIG){
    $urlRouterProvider.otherwise('/index');
    $stateProvider
        .state('index',{
            url:'/index',
            templateUrl:'Modules/index/index.html'
        })
        .state('intDetail',{
            url:'/intDetail',
            templateUrl:'Modules/intDetail/intDetail.html'
        })
        .state('Settlement',{
            url:'/Settlement',
            templateUrl:'Modules/Settlement/Settlement.html'
        })
        .state('purchaseSuccess',{
            url:'/purchaseSuccess',
            templateUrl:'Modules/purchaseSuccess/purchaseSuccess.html'
        })
        .state('newAddress',{
            url:'/newAddress',
            templateUrl:'Modules/newAddress/newAddress.html',
            resolve:load('Modules/newAddress/controller/newAddress.js')

        })


    function load(srcs, callback) {
        return {
            deps: ['$ocLazyLoad', '$q',
                function ($ocLazyLoad, $q) {
                    var deferred = $q.defer();
                    var promise = false;
                    srcs = angular.isArray(srcs) ? srcs : srcs.split(/\s+/);
                    if (!promise) {
                        promise = deferred.promise;
                    }
                    angular.forEach(srcs, function (src) {
                        promise = promise.then(function () {
                            angular.forEach(MAIN_CONFIG, function (module) {
                                if (module.name == src) {
                                    if (!module.module) {
                                        name = module.files;
                                    } else {
                                        name = module.name;
                                    }
                                } else {
                                    name = src;
                                }
                            });
                            return $ocLazyLoad.load(name);
                        });
                    });
                    deferred.resolve();
                    return callback ? promise.then(function () { return callback(); }) : promise;
                }]
        }
    }
}])