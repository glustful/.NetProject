'use strict';

/**
 * @ngdoc function
 * @name app.config:uiRouter
 * @description
 * # Config
 * Config for the router
 */

/**
 * 由于不适用项目老方法是用的路由策略被抛弃
 */
//app.config(
//    ['$routeProvider', function ($routeProvider) {
//        $routeProvider
//          .when('/Index', {
//              templateUrl: 'modules/Index/view/index.html',
//              data: { title: '首页' },
//              controller: 'modules/Index/render/index.js'
//              //resolve: load(['modules/Index/render/index.js'])
//              //              access:["admin"]
//          })
//
//          .when('/Activity', {
//              templateUrl: 'modules/activity/view/activity.html',
//              data: { title: 'activity' }
//              //              resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js']),
//              //              access:["admin"]
//          })
//
//          .otherwise({
//              redirectTo: 'app/Index'
//          })
//
//        //-----------------------end-------------------
//
//    }
//    ]
//  );


app.config(['$stateProvider', '$urlRouterProvider',function($stateProvider, $urlRouterProvider){
    $urlRouterProvider
        .otherwise('/app/home');
    $stateProvider
        .state('app',{
            url:'/app',
            templateUrl:'Common/widget/nav/nav.html'
        })
        .state('app.home',{
            url:'/home',
            templateUrl:'modules/Index/view/Index.html'
        })
        .state('app.activity',{
            url:'/activity',
            templateUrl:'modules/activity/view/activity.html'
        })
        .state('app.broker',{
            url:'/broker',
            templateUrl:'modules/broker/view/broker.html'
        })
        .state('app.customerList',{
            url:'/customerList',
            templateUrl:'modules/customerList/view/customerList.html'
        })
        .state('app.detail',{
            url:'/detail',
            templateUrl:'modules/detail/view/detail.html'
        })
        .state('app.hero',{
            url:'/hero',
            templateUrl:'modules/hero/view/hero.html'
        })
}]);