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
app  .run(
    ['$rootScope', '$state', '$stateParams',
        function ($rootScope, $state, $stateParams) {
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
            //$rootScope.$on('$stateChangeStart', function (event,next) {
            //    if(next.name==='access.signin' || next.name==='access.signup' || next.name==='access.forgot-password'){
            //        return;
            //    }
            //    if(!AuthService.IsAuthenticated()){
            //        event.preventDefault();
            //        $state.go('access.signin');
            //    }
            //    if(next.access !== undefined){
            //        if(!AuthService.IsAuthorized(next.access)){
            //            event.preventDefault();
            //            //TODO:跳转到权限提示页
            //        }
            //
            //    }
            //});
        }
    ]
)
    .config(['$stateProvider', '$urlRouterProvider','MAIN_CONFIG',function($stateProvider, $urlRouterProvider,MAIN_CONFIG){
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
        .state('app.partner_list',{
            url:'/partner_list',
            templateUrl:'modules/partner_list/view/partner_list.html'
        })
        .state('app.partner_details',{
            url:'/partner_details',
            templateUrl:'modules/partner_details/view/partner_details.html',
            resolve:load('modules/partner_details/controller/partner_details.js')
        })
        .state('app.partner_insert',{
            url:'/partner_insert',
            templateUrl:'modules/partner_insert/view/partner_insert.html'
        })
        .state('app.partner_insert1',{
            url:'/partner_insert1',
            templateUrl:'modules/partner_insert1/view/partner_insert1.html'
        })
        .state('app.groom',{
            url:'/groom',
            templateUrl:'modules/groom/view/groom.html',
            resolve:load('modules/groom/controller/controller.js')
        })
        .state('app.houseDetail',{
            url:'/houseDetail',
            templateUrl:'modules/houseDetail/view/houseDetail.html'
        })
        .state('app.houses',{
            url:'/houses',
            templateUrl:'modules/houses/view/houses.html'
        })
        .state('app.housesBuy',{
            url:'/housesBuy',
            templateUrl:'modules/housesBuy/view/housesBuy.html'
        })
        .state('app.housesPic',{
            url:'/housesPic',
            templateUrl:'modules/housesPic/view/housesPic.html'
        })
        .state('app.housesPicBuy',{
            url:'/housesPicBuy',
            templateUrl:'modules/housesPicBuy/view/housesPicBuy.html'
        })
        .state('app.myInt',{
            url:'/myInt',
            templateUrl:'modules/myInt/view/myInt.html'
        })
        .state('app.myPurse',{
            url:'/myPurse',
            templateUrl:'modules/myPurse/view/myPurse.html'
        })
        .state('app.personal',{
            url:'/personal',
            templateUrl:'modules/personal/view/personal.html'
        })
        .state('app.personalPage',{
            url:'/personalPage',
            templateUrl:'modules/personalPage/view/personalPage.html'
        })
        .state('app.recommend',{
            url:'/recommend',
            templateUrl:'modules/recommend/view/recommend.html'
        })
        .state('app.takeOff',{
            url:'/takeOff',
            templateUrl:'modules/takeOff/view/takeOff.html'
        })
        .state('app.redPaper-1',{
            url:'/redPaper-1',
            templateUrl:'modules/redPaper-1/view/redPaper-1.html'
        })
        .state('app.redPaper-2',{
            url:'/redPaper-2',
            templateUrl:'modules/redPaper-2/view/redPaper-2.html'
        })
        .state('app.storeroom',{
            url:'/storeroom',
            templateUrl:'modules/storeroom/view/storeroom.html'
        })
        .state('app.task',{
            url:'/task',
            templateUrl:'modules/task/view/task.html',
            controller:'taskController',
            resolve:load('modules/task/controller/task.js')
        })
        .state('app.nominate',{
            url:'/nominate',
            templateUrl:'modules/nominate/view/nominate.html'
        })
        .state('app.carry_client',{
            url:'/carry_client',
            templateUrl:'modules/carry_client/view/carry_client.html',
            resolve:load('modules/carry_client/controller/controller.js')
        })
        .state('app.credit_add',{
            url:'/credit_add',
            templateUrl:'modules/credit_add/view/credit_add.html'
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

































