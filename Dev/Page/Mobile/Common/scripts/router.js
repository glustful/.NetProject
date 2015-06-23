/**
 * Created by Yunjoy on 2015/6/6.
 */
var allowState = [
    'app.home',
    'user.login',
    'app.storeroom',
    'app.housesPic',
    'user.register',
    'user.PasswordFound',
    'app.invite',
    'app.housesBuy'

];
app.run(
    ['$rootScope', '$state', '$stateParams', 'AuthService',
        function ($rootScope, $state, $stateParams, AuthService) {
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
            $rootScope.$on('$stateChangeStart', function (event,next) {
                if(allowState.indexOf(next.name) > -1){
                    return;
                }
                if(!AuthService.IsAuthenticated()){
                    event.preventDefault();
                    $state.go('user.login');
                }
                if(next.access !== undefined){
                    if(!AuthService.IsAuthorized(next.access)){
                        event.preventDefault();

                    }
                }
            });
        }
    ]
).config(['$stateProvider', '$urlRouterProvider','MAIN_CONFIG',function($stateProvider,$urlRouterProvider,MAIN_CONFIG){
        $urlRouterProvider
            .otherwise('/app/home');
        $stateProvider
            .state('app',{
                url:'/app',
                templateUrl:'Common/widget/nav/nav.html'
            })
            .state('user',{
                url:'/user',
                templateUrl:'Common/widget/layout/user.html'
            })
            .state('user.login',{
                url:'/login',
                templateUrl:'modules/Login/view/login.html',
                resolve:load('modules/Login/controller/LoginController.js'),
                data:{title:'用户登录'}
            })
            .state('user.register',{
                url:'/register?yqm',
                templateUrl:'modules/Register/view/register.html',
                resolve:load('modules/Register/controller/RegisterController.js'),
                data:{title:'用户注册'}
            })
            .state('user.PasswordFound',{
                url:'/PasswordFound',
                templateUrl:'modules/PasswordFound/view/PasswordFound.html',
                resolve:load('modules/PasswordFound/controller/PasswordController.js'),
                data:{title:'找回密码'}
            })
            .state('app.home',{
                url:'/home',
                templateUrl:'modules/Index/view/Index.html',
                resolve:load(['modules/Index/render/homeController.js'])
            })

            .state('app.activity',{
                url:'/activity',
                templateUrl:'modules/activity/view/activity.html'
            })
            .state('app.broker',{
                url:'/broker',
                templateUrl:'modules/broker/view/broker.html',
                resolve:load('modules/broker/controller/broker.js')
            })
            .state('app.customerList',{
                url:'/customerList',
                templateUrl:'modules/customerList/view/customerList.html'
                ,resolve:load('modules/customerList/controller/customerList.js')
            })
            .state('app.detail',{
                url:'/detail',
                templateUrl:'modules/detail/view/detail.html'
            })
            .state('app.hero',{
                url:'/hero',
                templateUrl:'modules/hero/view/hero.html',
                resolve:load("modules/hero/controller/heroController.js")
            })
            .state('app.setting',{
                url:'/setting',
                templateUrl:'modules/setting/view/setting.html'
            })
            .state('app.person_setting',{
                url:'/person_setting',
                templateUrl:'modules/person_setting/view/person_setting.html',
                resolve:load('modules/person_setting/controller/personsettingController.js'),
                access:['broker','user']
            })

            .state('app.security_setting',{
                url:'/security_setting',
                templateUrl:'modules/security_setting/view/security_setting.html',
                resolve:load('modules/security_setting/controller/SecuritySetting.js'),
                access:['broker','user']
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
                templateUrl:'modules/partner_list/view/partner_list.html',
                resolve:load('modules/partner_list/controller/partner_list.js')
            })
            .state('app.partner_details',{
                url:'/partner_details?Id',
                templateUrl:'modules/partner_details/view/partner_details.html',
                resolve:load('modules/partner_details/controller/partner_details.js')
            })
            .state('app.partner_insert',{
                url:'/partner_insert',
                templateUrl:'modules/partner_insert/view/partner_insert.html',
                resolve:load('modules/partner_insert/controller/partner_insert.js')
            })
            .state('app.partner_insert1',{
                url:'/partner_insert1',
                templateUrl:'modules/partner_insert1/view/partner_insert1.html'
            })
            .state('app.groom',{
                url:'/groom?Projectid&name&type',
                templateUrl:'modules/groom/view/groom.html',
                resolve:load('modules/groom/controller/controller.js')
            })
            .state('app.houseDetail',{
                url:'/houseDetail',
                templateUrl:'modules/houseDetail/view/houseDetail.html'
            })
            .state('app.houses',{
                url:'/houses?BrandId&ProductId',
                templateUrl:'modules/houses/view/houses.html',
                resolve:load('modules/houses/controller/houses.js')
            })
            .state('app.housesBuy',{
                url:'/housesBuy?BrandId',
                templateUrl:'modules/housesBuy/view/housesBuy.html',
                controller:'HousesBuyController',
                resolve:load(['modules/housesBuy/static/scripts/HousesBuy.js'])
            })
            .state('app.housesPic',{
                url:'/housesPic?productId',
                templateUrl:'modules/housesPic/view/housesPic.html',
                controller:'HousesPicController',
                resolve:load('modules/housesPic/scripts/HousesPic.js')
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
                templateUrl:'modules/myPurse/view/myPurse.html',
                resolve:load('modules/myPurse/render/controller.js')
            })
            .state('app.personal',{
                url:'/personal',
                templateUrl:'modules/personal/view/personal.html',
                resolve:load('modules/personal/controller/personal.js')
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
                // controller:'StormRoomController',
                templateUrl:'modules/storeroom/view/storeroom.html',
                resolve:load('modules/storeroom/scripts/StoreRoom.js')
            })
            .state('app.brand',{
                url:'/brand?condition',
                controller:'BrandController',
                templateUrl:'modules/brandList/view/brandList.html',
                resolve:load('modules/brandList/scripts/Brand.js')
            })
            .state('app.task',{
                url:'/task',
                templateUrl:'modules/task/view/task.html',
                resolve:load('modules/task/controller/task.js')
            })
            .state('app.nominate',{
                url:'/nominate',
                templateUrl:'modules/nominate/view/nominate.html',
                resolve:load('modules/nominate/controller/nominate.js')
            })
            .state('app.carry_client',{
                url:'/carry_client?Projectid&name&type',
                templateUrl:'modules/carry_client/view/carry_client.html',
                resolve:load('modules/carry_client/controller/controller.js')
            })
            .state('app.credit_add',{
                url:'/credit_add',
                templateUrl:'modules/credit_add/view/credit_add.html',
                resolve:load('modules/myPurse/render/controller.js')
            })
            .state('app.recommendedBroker',{
                url:'/recommendedBroker',
                templateUrl:'modules/recommendBroker/view/recommendedBroker.html'
            })
            .state('app.addBroker',{
                url:'/addBroker',
                templateUrl:'modules/addBroker/view/addBroker.html',
                resolve:load('modules/addBroker/controller/AddBrokerController.js')
            })
            .state('app.grabPacket',{
                url:'/grabPacket',
                templateUrl:'modules/grabPacket/view/grabPacket.html'
            })
            .state('app.luckPacket',{
                url:'/luckPacket',
                templateUrl:'modules/luckPacket/view/luckPacket.html'
            })
            .state('app.sendPacket',{
                url:'/sendPacket',
                templateUrl:'modules/sendPacket/view/sendPacket.html'
            })
            .state('app.chip',{
                url:'/chip',
                templateUrl:'modules/chip/view/chip.html'
            })
            .state('app.chipDetail',{
                url:'/chipDetail',
                templateUrl:'modules/chipDetail/view/chipDetail.html'
            })
            .state('app.chipEle',{
                url:'/chipEle',
                templateUrl:'modules/chipEle/view/chipEle.html'
            })
            .state('app.chipPartake',{
                url:'/chipPartake',
                templateUrl:'modules/chipPartake/view/chipPartake.html'
            })
            .state('app.Auction',{
                url:'/Auction',
                templateUrl:'modules/Auction/view/Auction.html'
            })
            .state('app.AuctionSpecial',{
                url:'/AuctionSpecial',
                templateUrl:'modules/AuctionSpecial/view/AuctionSpecial.html'
            })
            .state('app.AuctionUserInformation',{
                url:'/AuctionUserInformation',
                templateUrl:'modules/AuctionUserInformation/view/AuctionUserInformation.html'
            })
            .state('app.Coupons',{
                url:'/Coupons',
                templateUrl:'modules/Coupons/view/Coupons.html'
            })
            .state('app.CouponsOwn',{
                url:'/CouponsOwn',
                templateUrl:'modules/CouponsOwn/view/CouponsOwn.html'
            })
            .state('app.withdrawals',{
                url:'/withdrawals',
                templateUrl:'modules/withdrawals/view/withdrawals.html',
                resolve:load('modules/myPurse/render/controller.js')
            })
            .state('app.withdrawalsDetail',{
                url:'/withdrawalsDetail',
                templateUrl:'modules/withdrawalsDetail/view/withdrawalsDetail.html'
            })
            .state('app.NoviceTask',{
                url:'/NoviceTask',
                templateUrl:'modules/NoviceTask/view/NoviceTask.html'
            })
            .state('app.invite',{
                url:'/invite',
                templateUrl:'modules/invite/view/invite.html',
                resolve:load('modules/invite/controller/invitecontroller.js')
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
    }]);