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
}]);'use strict';

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
        .state('app.setting',{
            url:'/setting',
            templateUrl:'modules/setting/view/setting.html'
        })
        .state('app.person_setting',{
            url:'/person_setting',
            templateUrl:'modules/person_setting/view/person_setting.html'
        })
        .state('app.security_setting',{
            url:'/security_setting',
            templateUrl:'modules/security_setting/view/security_setting.html'
        })
        .state('app.zhongtian_HouseDetail',{
            url:'/zhongtian_HouseDetail',
            templateUrl:'modules/zhongtian_HouseDetail/view/zhongtian_HouseDetail.html'
        })
        .state('app.zhongtian_seller',{
            url:'/zhongtian_seller',
            templateUrl:'modules/zhongtian_seller/view/zhongtian_seller.html'
        })
        .state('app.activity',{
            url:'/activity',
            templateUrl:'modules/activity/view/activity.html'
        })
        .state('app.partner_list',{
            url:'/partner_list',
            templateUrl:'modules/partner_list/view/partner_list.html'
        })
        .state('app.partner_details',{
            url:'/partner_details',
            templateUrl:'modules/partner_details/view/partner_details.html'
        })
        .state('app.partner_insert',{
            url:'/partner_insert',
            templateUrl:'modules/partner_insert/view/partner_insert.html'
        })
        .state('app.partner_insert1',{
            url:'/partner_insert1',
            templateUrl:'modules/partner_insert1/view/partner_insert1.html'
        })
}]);