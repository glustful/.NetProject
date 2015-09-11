'use strict';

/**
 * Config for the router
 */
angular.module('app')

    .run(
    [          '$rootScope', '$state', '$stateParams','AuthService',
        function ($rootScope,   $state,   $stateParams,AuthService) {
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;

            $rootScope.$on('$stateChangeStart', function (event,next) {
                //console.log(AuthService.IsAuthenticated());
                //console.log(next);
                if(next.name==='access.signin' || next.name==='access.signup' || next.name==='access.forgot-password'){
                    return;
                }
                if(!AuthService.IsAuthenticated()){
                    event.preventDefault();
                    $state.go('access.signin');
                }
                if(next.access !== undefined){
                    if(!AuthService.IsAuthorized(next.access)){
                        event.preventDefault();
                        //TODO:跳转到权限提示页
                    }

                }
            });
        }
    ]
)

    .config(
    [          '$stateProvider', '$urlRouterProvider',
      function ($stateProvider,   $urlRouterProvider) {
          
            $urlRouterProvider
                .otherwise('/app/home');                              //程序启动默认界面
            $stateProvider
                .state('app', {
                    abstract: true,
                    url: '/app',
                    templateUrl: 'app/common/layout/app.html'
                })

//=====================================app page======================================//
                .state('app.test', {
                    url: '/test',
                    templateUrl: 'app/module/test/view/test.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/test/controller/testController.js']);
                            }]
                    }
                })
                .state('app.home', {
                    url: '/home',
                    templateUrl: 'app/module/home/view/home.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/home/controller/homeController.js']);
                            }]
                    }
                })
                //--------------会员信息管理
                .state('app.member', {
                    url: '/member',
                    template: '<div ui-view class="fade-in-up"></div>'
                })
                .state('app.member.memlist',{
                    url:'memlist',
                    templateUrl:'app/module/member/view/memlist.html',
                    resolve:{
                        deps:['$ocLazyLoad',
                        function($ocLazyLoad){
                            return $ocLazyLoad.load(['app/module/member/controller/memlist.js']);
                        }]
                    }
                })
                .state('app.member.detail',{
                    url:'detail?id',
                    templateUrl:'app/module/member/view/detail.html',
                    resolve:{
                        deps:['$ocLazyLoad',
                            function($ocLazyLoad){
                                return $ocLazyLoad.load(['app/module/member/controller/memlist.js']);
                            }]
                    }
                })

                //--------------公众号基本设置
                .state('app.autoRes', {
                    url: '/autoRes',
                    template: '<div ui-view class="fade-in-up"></div>'
                })
                .state('app.autoRes.focusRes', {
                    url: '/focusRes',
                    templateUrl: 'app/module/autoRes/view/focusRes.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/autoRes/controller/focusResController.js']);
                            }]
                    }
                })
                .state('app.autoRes.keyRes', {
                    url: '/keyRes',
                    templateUrl: 'app/module/autoRes/view/keyRes.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/autoRes/controller/keyResController.js']);
                            }]
                    }
                })

                .state('app.autoRes.createKeyRes', {
                    url: '/createKeyRes',
                    templateUrl: 'app/module/autoRes/view/createKeyRes.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/autoRes/controller/keyResController.js']);
                            }]
                    }
                })

                .state('app.autoRes.editKeyRes', {
                    url: '/editKeyRes?id',
                    templateUrl: 'app/module/autoRes/view/editKeyRes.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/autoRes/controller/keyResController.js']);
                            }]
                    }
                })

                //--------------基础数据管理
                .state('app.contact', {
                    url: '/contact',
                    template: '<div ui-view class="fade-in-up"></div>'
                })
                .state('app.contact.contactList', {
                    url: '/contactList',
                    templateUrl: 'app/module/contact/view/contactList.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/contact/controller/contactController.js']);
                            }]
                    }
                })
                .state('app.comment', {
                    url: '/contact',
                    template: '<div ui-view class="fade-in-up"></div>'
                })
                .state('app.comment.productComment', {
                    url: '/productComment',
                    templateUrl: 'app/module/comment/view/productComment.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/comment/controller/productCommentController.js']);
                            }]
                    }
                })
                //------------自定义菜单
              .state('app.menu', {
                  url: '/menu',
                  template: '<div ui-view class="fade-in-up"></div>'
              })
              .state('app.menu.menulist', {
                  url: '/menulist',
                  templateUrl: 'app/module/menu/view/menulist.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                          function( $ocLazyLoad ){
                              return $ocLazyLoad.load(['app/module/menu/controller/menulistCtr.js']);
                          }]
                  }
              })
              .state('app.menu.childmenulist', {
                  url: '/childmenulist',
                  templateUrl: 'app/module/menu/view/childmenulist.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                          function( $ocLazyLoad ){
                              return $ocLazyLoad.load(['app/module/menu/controller/childmenulistCtr.js']);
                          }]
                  }
              })
              .state('app.menu.updatemenu', {
                  url: '/updatemenu',
                  templateUrl: 'app/module/menu/view/updatemenu.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                          function( $ocLazyLoad ){
                              return $ocLazyLoad.load(['app/module/menu/controller/updatemenuCtr.js']);
                          }]
                  }
              })


//=====================================event page=======================================//
                .state('event', {
                    url: '/event',
                    templateUrl: 'app/common/layout/app.html'
                })

                //--------------红包模板
                .state('event.redModel', {
                    url: '/redModel',
                    template: '<div ui-view class="fade-in-up"></div>'
                })

                .state('event.redModel.redMain', {
                    url: '/redMain',
                    templateUrl: 'app/module/event/redModel/view/redMain.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/event/redModel/controller/redMainController.js']);
                            }]
                    }
                })



//=====================================deploy page=======================================//
                .state('deploy', {
                    url: '/deploy',
                    templateUrl: 'app/common/layout/app.html'
                })

                .state('deploy.deploy', {
                    url: '/deploy',
                    templateUrl: 'app/module/baseSetting/view/deploy.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/baseSetting/controller/deployController.js']);
                            }]
                    }
                })

                .state('deploy.baseSetting', {
                    url: '/baseSetting',
                    templateUrl: 'app/module/baseSetting/view/baseSetting.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/baseSetting/controller/baseSettingController.js']);
                            }]
                    }
                })
                .state('deploy.createSetting', {
                    url: '/createSetting',
                    templateUrl: 'app/module/baseSetting/view/createSetting.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/baseSetting/controller/baseSettingController.js']);
                            }]
                    }
                })
                .state('deploy.editSetting', {
                    url: '/editSetting?id',
                    templateUrl: 'app/module/baseSetting/view/editSetting.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/baseSetting/controller/baseSettingController.js']);
                            }]
                    }
                })



//=====================================access page======================================//
                .state('access', {
                    url: '/access',
                    template: '<div ui-view class="fade-in-right-big smooth"></div>'
                })

                //404
                .state('access.404', {
                    url: '/404',
                    templateUrl: 'app/common/layout/page_404.html'
                })

                //输入电子邮件重置密码
                .state('access.forgotpwd', {
                    url: '/forgotpwd',
                    templateUrl: 'app/module/page_forgotpwd.html'
                })

                //登录页
                .state('access.signin', {
                    url: '/signin',
                    templateUrl: 'app/module/signin/view/signin.html',
                    resolve: {
                        deps: ['uiLoad',
                            function( uiLoad ){
                                return uiLoad.load( ['app/module/signin/controller/signinController.js'] );
                            }]
                    }
                })

                //注册页
                .state('access.signup', {
                    url: '/signup',
                    templateUrl: 'app/module/page_signup.html',
                    resolve: {
                        deps: ['uiLoad',
                            function( uiLoad ){
                                return uiLoad.load( ['js/controllers/signup.js'] );
                            }]
                    }
                })
                //商品属性管理 app.parameter.parameterList
                .state('app.parameter',{
                    url: '/parameter',
                    template: '<div ui-view class="fade-in-up"></div>',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/Parameter/controller/parameter.js']);
                            }]
                    }
                })
                .state('app.parameter.parameterList',{
                    url:'/parameterList',
                    templateUrl:'app/module/Parameter/view/Index.html'

                })
                .state('app.parameter.createParameter',{
                    url:'/createParameter',
                    templateUrl:'app/module/Parameter/view/Create.html'
                })
                .state('app.parameter.editParameter',{
                    url:'/editParameter?id',
                    templateUrl:'app/module/Parameter/view/Edit.html'
                })

                //商品管理页
                .state('app.product',{
                    url: '/product',
                    template: '<div ui-view class="fade-in-up"></div>',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['app/module/Product/controller/productController.js']);
                            }]
                    }
                })
                .state('app.product.productList',{
                    url:'/productList',
                    templateUrl:'app/module/Product/view/Index.html',
                    data : { title: '商品列表' }
                })
                .state('app.product.createProduct',{
                    url:'/createProct',
                    templateUrl:'app/module/Product/view/Create.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function( $ocLazyLoad ){
                                return $ocLazyLoad.load(['angularFileUpload']);
                            }]
                    }
                })
                .state('app.product.editProduct',{
                    url:'/editProduct?id',
                    templateUrl:'app/module/Product/view/Edit.html',
                    //resolve: {
                    //    deps: ['$ocLazyLoad',
                    //        function( $ocLazyLoad ){
                    //            return $ocLazyLoad.load(['app/module/Product/controller/productController.js']);
                    //        }]
                    //}
                })
                .state('order',{
                    url:'/order',
                    templateUrl: 'app/common/layout/app.html'
                })
                .state('order.list',{
                    url:'/list',
                    templateUrl:'app/module/order/view/list.html',
                    resolve:{
                        deps:['uiLoad',function(uiLoad){
                            return uiLoad.load(['app/module/order/controller/orderController.js']);
                        }]
                    }
                })
                .state('order.serviceList',{
                    url:'/serviceList',
                    templateUrl:'app/module/order/view/serviceList.html',
                    resolve:{
                        deps:['uiLoad',function(uiLoad){
                            return uiLoad.load(['app/module/order/controller/orderServiceController.js']);
                        }]
                    }
                });
        }
    ]
);