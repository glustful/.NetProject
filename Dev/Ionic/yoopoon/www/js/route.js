// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.services' is found in services.js
// 'starter.controllers' is found in controllers.js
var app = angular.module('starter', ['ionic','ngCordova','ngStorage']);
var SETTING = {
BaseUrl:'http://www.iyookee.cn/',
ApiUrl:'http://localhost:50597/api',
ImgUrl:'http://img.iyookee.cn/',
eventApiUrl:'http://www.iyookee.cn/API'
};
app.run(function($ionicPlatform,$ionicHistory,$ionicLoading) {
     $ionicPlatform.ready(function() {
                          // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
                          // for form inputs)
                          if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
                          cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
                          cordova.plugins.Keyboard.disableScroll(true);
                          
                          }
                          if (window.StatusBar) {
                          // org.apache.cordova.statusbar required
                          StatusBar.styleLightContent();
                          //StatusBar.hide();
                          }
                          
                          });
        $ionicPlatform.registerBackButtonAction(function(event) {
          event.preventDefault();

          if ($ionicHistory.currentStateName() === 'page.shopping') {
            window.close();
            ionic.Platform.exitApp();
          } else {
            $ionicHistory.goBack();
          }
          return false;
         }, 101);

        });

app.config(function($stateProvider, $urlRouterProvider) {
        
        // Ionic uses AngularUI Router which uses the concept of states
        // Learn more here: https://github.com/angular-ui/ui-router
        // Set up the various states which the app can be in.
        // Each state's controller can be found in controllers.js
        $stateProvider
        
        // setup an abstract state for the tabs directive
        .state('page', {
               url: '/page',
               abstract: true,
               templateUrl: 'page/tabs.html'
               })
        
        // Each tab has its own nav history stack:
        
        .state('page.shopping', {
               url: '/shopping',
               views: {
               'page-shopping': {
               templateUrl: 'page/shopping/tab-shopping.html',
               controller: 'TabShoppingCtrl'
               }
               }
               })




        .state('page.service', {
               url: '/service',
               views: {
               'page-service': {
               templateUrl: 'page/service/tab-service.html',
               controller: 'serviceIndex'
               }
               }
               })
            .state('page.clear', {
                url: '/service/clear?name',
                views: {
                    'page-service': {
                        templateUrl: 'page/service/clear.html',
                        controller: 'clearservice'
                    }
                }
            })
            .state('page.safe', {
                url: '/service/safe?name',
                views: {
                    'page-service': {
                        templateUrl: 'page/service/safe.html',
                        controller: 'safeservice'
                    }
                }
            })
            .state('page.myService', {
                url: '/service/myService?tabIndex',
                views: {
                    'page-service': {
                        templateUrl: 'page/service/myService.html',
                        controller: 'TabServiceCtrl'

                    }
                }
            })
            .state('page.safe-detail', {
                url: '/service/safe-detail?id&name',
                views: {
                    'page-service': {
                        templateUrl: 'page/service/safe-detail.html',
                        controller:'safedetailservice'
                    }
                }
            })
        .state('page.car', {
               url: '/car',
               views: {
               'page-car': {
               templateUrl: 'page/car/tab-car.html',
               controller: 'TabCarCtrl'
               }
               }
               })
            .state('page.order', {
                url: '/car/order?productId&count',
                views: {
                    'page-car': {
                        templateUrl: 'page/car/order.html',
                        controller: 'submitOrderController'
                    }
                }
            })
        .state('page.car-detail',{
          url: '/car/:chatId',
          views: {
            'page-car':{
              templateUrl: 'page/car-detail.html',
              controller: 'CarDetailCtrl'
            }
          }
        })
        .state('page.me', {
               url: '/me',
               views: {
               'page-me': {
               templateUrl: 'page/me/tab-me.html',
               controller: 'TabMeCtrl'
               }
               }
               })
            .state('page.personalData', {
                url: '/me/personalData',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/personalData.html',
                        controller: 'TabMeCtrl'
                    }
                }
            })
            .state('page.addressAdm', {
                url: '/me/addressAdm',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/addressAdm.html',
                        controller: 'addressAdm'
                    }
                }
            })
            .state('page.selectAddress', {
                url: '/me/selectAddress',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/selectAddress.html',
                        controller: 'TabMeCtrl'
                    }
                }
            })
            .state('page.selectProvince', {
                url: '/me/selectProvince',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/selectProvince.html',
                        controller: 'selProvice'
                    }
                }
            })
            .state('page.selectCity', {
                url: '/me?id&name',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/selectCity.html',
                        controller: 'selectCity'
                    }
                }
            })
            .state('page.selectCounty',{
                url:'/me?id&name',
                views:{
                    'page-me':{
                        templateUrl:'page/me/selectCounty.html',
                        controller:'selectCounty'
                    }
                }
            })
            .state('page.newAddress', {
                url: '/me/newAddress?id&name',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/newAddress.html'

                    }
                }
            })
            .state('page.myOrder', {
                url: '/me/myOrder?tabIndex',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/myOrder.html',
                        controller: 'myorder'
                    }
                }
            })
            .state('page.comment', {
                url: '/me/comment?Id&Status',
                views: {
                    'page-me': {
                        templateUrl: 'page/me/comment.html',
                        controller: 'comment'
                    }
                }
            })
            .state('page.register', {
                url: '/user/register',
                views: {
                    'page-me': {
                        templateUrl: 'page/user/register.html',
                        controller:'register'
                    }
                }
            })
            .state('page.login', {
                url: '/user/login',
                views: {
                    'page-me': {
                        templateUrl: 'page/user/login.html',
                        controller:'login'
                    }
                }
            })
    .state('page.Product-Catagory', {
        url: '/shopping/Product-Catagory',
        views: {
            'page-shopping': {
                templateUrl: 'page/shopping/Product-Catagory.html',
                controller: 'CategoryController'
            }
        }
    })
    .state('page.product-list', {
        url: '/shopping/product-list?id&name',
        views: {
            'page-shopping': {
                templateUrl: 'page/shopping/product-list.html',
                controller: 'ShoppingListCtrl'
            }
        }
    })
    .state('page.product-detail', {
        url: '/shopping/product-detail?id',
        views: {
            'page-shopping': {
                templateUrl: 'page/shopping/product-detail.html',
                controller: 'ProductDetail'
            }
        }
    })
    .state('page.search_product', {
        url: '/search_product?productName',
        views: {
            'page-shopping': {
                templateUrl: 'page/shopping/searchProduct.html',
                controller: 'SearchProductCtr'
            }
        }
    });
        
        // if none of the above states are matched, use this as the fallback
        $urlRouterProvider.otherwise('/page/shopping');
        
});


