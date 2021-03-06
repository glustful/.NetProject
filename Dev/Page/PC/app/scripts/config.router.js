'use strict';

/**
 * @ngdoc function
 * @name app.config:uiRouter
 * @description
 * # Config
 * Config for the router
 */
angular.module('app')
  .run(
    [           '$rootScope', '$state', '$stateParams','AuthService',
      function ( $rootScope,   $state,   $stateParams ,AuthService) {
          $rootScope.$state = $state;
          $rootScope.$stateParams = $stateParams;

          $rootScope.$on('$stateChangeStart', function (event,next) {
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
    ])
    .config( function ($httpProvider) {
        $httpProvider.interceptors.push('httpRequestInterceptor')})
  .config(
    ['$stateProvider', '$urlRouterProvider', 'MODULE_CONFIG',
      function ($stateProvider, $urlRouterProvider, MODULE_CONFIG) {
        $urlRouterProvider
          .otherwise('/page/Trading/product/product');
          //.otherwise('/homepage');
        $stateProvider
          .state('page', {
            url: '/page',
            views: {
              '': {
                templateUrl: 'views/layout.html'
              },
              'aside': {
                templateUrl: 'views/aside.html'
              },
              'content': {
                templateUrl: 'views/content.html'
              }
            }
          })

            .state('page.profile', {
              url: '/profile',
              templateUrl: 'views/pages/profile.html',
                  data: { title: 'Profile', theme: { primary: 'green' } }
            })

            .state('page.blank', {
              url: '/blank',
              templateUrl: 'views/pages/blank.html',
                  data: { title: 'Blank' }
            })
            .state('page.document', {
              url: '/document',
              templateUrl: 'views/pages/document.html',
                  data: { title: 'Document' }
            })
            .state('homepage', {
                url: '/homepage',
                templateUrl: 'views/pages/homepage.html'
            })
            .state('404', {
              url: '/404',
              templateUrl: 'views/pages/404.html'
            })
            .state('505', {
              url: '/505',
              templateUrl: 'views/pages/505.html'
            })
            .state('access', {
              url: '/access',
              template: '<div class="indigo bg-big"><div ui-view class="fade-in-down smooth"></div></div>'
            })
            .state('access.signin', {
                url: '/signin',
                templateUrl: 'views/pages/signin.html',
                controller: 'LoginControl',
                resolve: load('scripts/controllers/UC/Login.js')
            })
            .state('access.signup', {
              url: '/signup',
              templateUrl: 'views/pages/signup.html'
            })
            .state('access.forgot-password', {
              url: '/forgot-password',
              templateUrl: 'views/pages/forgot-password.html'
            })
            .state('access.lockme', {
              url: '/lockme',
              templateUrl: 'views/pages/lockme.html',
                controller: 'LogoutControl',
                resolve: load('scripts/controllers/UC/Logout.js')
            })

            //-------------------------yangbo----------------
            .state('app.AgentManagement', {
                url: '/AgentManagement',
                templateUrl: 'views/pages/AgentManagement.html',
                  data: { title: '经纪人管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.BusinessManagement', {
                url: '/BusinessManagement',
                templateUrl: 'views/pages/BusinessManagement.html',
                  data: { title: '商家管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.AdminManagement', {
                url: '/AdminManagement',
                templateUrl: 'views/pages/AdminManagement.html',
                  data: { title: 'Admin管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.cw', {
                url: '/cw',
                templateUrl: 'views/pages/cw.html',
                  data: { title: '财务账号管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.zc', {
                url: '/zc',
                templateUrl: 'views/pages/zc.html',
                  data: { title: '驻场秘书账号管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.dk', {
                url: '/dk',
                templateUrl: 'views/pages/dk.html',
                  data: { title: '驻场秘书账号管理' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.AdminWork', {
                url: '/AdminWork',
                templateUrl: 'views/pages/AdminWork.html',
                  data: { title: '待审核推荐' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.BusinessWork', {
                url: '/BusinessWork',
                templateUrl: 'views/pages/BusinessWork.html',
                  data: { title: '待上访记录' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.SecretaryWork', {
                url: '/SecretaryWork',
                templateUrl: 'views/pages/SecretaryWork.html',
                  data: { title: '洽谈中业务' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.FinanceWork', {
                url: '/FinanceWork',
                templateUrl: 'views/pages/FinanceWork.html',
                  data: { title: '洽谈失败' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('app.failed', {
                url: '/failed',
                templateUrl: 'views/pages/failed.html',
                  data: { title: '洽谈成功' },
                  resolve: load(['scripts/controllers/chart.js', 'scripts/controllers/vectormap.js'])
            })

            .state('ui.component.Admin', {
                url: '/Admin',
                templateUrl: 'views/ui/component/Admin.html',
                  data: { title: 'Admin' }
            })

            .state('ui.component.business', {
                url: '/business',
                templateUrl: 'views/ui/component/business.html',
                  data: { title: '商家' }
            })

            .state('ui.material.recommended', {
                url: '/recommended',
                templateUrl: 'views/ui/material/recommended.html',
                  data: { title: '带客推荐' }
            })

            /* .state('ui.material.icon', {
             url: '/icon',
             templateUrl: 'views/ui/material/icon.html',
             data : { title: 'Icons' }
             })*/

            .state('ui.material.strike', {
                url: '/strike',
                templateUrl: 'views/ui/material/strike.html',
                  data: { title: '所有成交' }

            })

            .state('ui.form.comBroker', {
                url: '/comBroker',
                templateUrl: 'views/ui/form/comBroker.html',
                  data: { title: '推荐经纪人' }
            })

            .state('ui.table.partner', {
                url: '/partner',
                templateUrl: 'views/ui/table/partner.html',
                  data: { title: '合伙人', theme: { primary: 'blue' } }
            })

            .state('ui.map.admins', {
                url: '/admins',
                templateUrl: 'views/ui/map/admins.html',
                  data: { title: '等级配置' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })

            //.state('page.setting', {
            //    url: '/setting',
            //    templateUrl: 'views/pages/setting.html',
            //      data: { title: '短信列表', theme: { primary: 'green' } }
            //})
            .state('ui.map.list', {
                url: '/list',
                templateUrl: 'views/ui/map/list.html',
                  data: { title: '任务列表' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })

            .state('ui.map.task', {
                url: '/task',
                templateUrl: 'views/ui/map/task.html',
                  data: { title: '任务配置' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })







              .state('page.CRM', {
                  url: '/CRM',
                  template: '<div ui-view></div>'
            })
              .state('page.CRM.AgentManager', {
                  url: '/AgentManager',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.AgentManager.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/AgentManager/index.html',
              data: { title: '经纪人管理' },
              resolve:load(['scripts/controllers/CRM/agentmanager.js',
                  'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.AgentManager.detailed', {
              url: '/detailed?userid',
              templateUrl: 'views/pages/CRM/AgentManager/detailed.html',
              data: { title: '详情页' },
               resolve:load('scripts/controllers/CRM/agentmanager.js')
            })

              .state('page.CRM.BusMan', {
                  url: '/BusMan',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.BusMan.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/BusMan/index.html',
                  data: { title: '商家管理' },
                resolve:load(['scripts/controllers/CRM/busman.js',
                    'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.BusMan.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/BusMan/detailed.html',
                  data: { title: '详情页' },
                resolve:load('scripts/controllers/CRM/busman.js')
            })

              .state('page.CRM.AdmMan', {
                  url: '/AdmMan',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.AdmMan.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/AdmMan/index.html',
                  data: { title: 'Admin管理' },
                resolve:load(['scripts/controllers/CRM/AdmMan.js',
                    'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.AdmMan.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/AdmMan/detailed.html',
                  data: { title: '详情页' },
                resolve:load('scripts/controllers/CRM/AdmMan.js')
            })
            .state('page.CRM.AdmMan.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/AdmMan/create.html',
                data: { title: '新建管理员账号' },
                resolve:load('scripts/controllers/CRM/AdmMan.js')
            })

              .state('page.CRM.CW', {
                  url: '/CW',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.CW.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/CW/index.html',
                  data: { title: '财务账号管理' },
                resolve:load(['scripts/controllers/CRM/cw.js',
                    'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.CW.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/CW/detailed.html',
                  data: { title: '详情页' },
                resolve:load('scripts/controllers/CRM/cw.js')
            })
            .state('page.CRM.CW.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/CW/create.html',
                data: { title: '添加页' },
                resolve:load('scripts/controllers/CRM/cw.js')
            })

              .state('page.CRM.ZC', {
                  url: '/ZC',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.ZC.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/ZC/index.html',
                  data: { title: '驻场秘书账号管理' },
                resolve:load(['scripts/controllers/CRM/zc.js',
                    'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.ZC.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/ZC/detailed.html',
                  data: { title: '详情页' },
                resolve:load('scripts/controllers/CRM/zc.js')
            })
            .state('page.CRM.ZC.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/ZC/create.html',
                data: { title: '新建用户' },
                resolve:load('scripts/controllers/CRM/zc.js')
            })

              .state('page.CRM.DK', {
                  url: '/DK',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.DK.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/DK/index.html',
                  data: { title: '带客人员账号管理' },
                controller:"dkIndexController",
                resolve:load(['scripts/controllers/CRM/dk.js',
                    'scripts/controllers/CRM/inforManagement.js'])
            })
            .state('page.CRM.DK.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/DK/detailed.html',
                  data: { title: '详情页' },
                controller:"dkDetailedController",
                resolve:load('scripts/controllers/CRM/dk.js')
            })
            .state('page.CRM.DK.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/DK/create.html',
                data: { title: '新建用户' },
                controller:"UserCreateController",
                resolve:load('scripts/controllers/CRM/dk.js')
            })

              .state('page.CRM.WaitCheck', {
                url: '/WaitCheck',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.WaitCheck.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/WaitCheck/index.html',
                data : { title: '待审核推荐' },
                controller:"WaitListController",
                resolve:load('scripts/controllers/CRM/WaitCheck.js')
            })
            .state('page.CRM.WaitCheck.check', {
                url: '/check?id',
                templateUrl: 'views/pages/CRM/WaitCheck/check.html',
                data : { title: '审核页' },
                controller:"ARDetialController",
                resolve:load('scripts/controllers/CRM/WaitCheck.js')
            })
            .state('page.CRM.WaitCheck.pass', {
                url: '/pass',
                templateUrl: 'views/pages/CRM/WaitCheck/pass.html',
                data: { title: '通过页' }
            })
            .state('page.CRM.WaitCheck.refuse', {
                url: '/refuse',
                templateUrl: 'views/pages/CRM/WaitCheck/refuse.html',
                data: { title: '拒绝页' }
            })

            .state('page.CRM.DkRecord', {
                url: '/DkRecord',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.DkRecord.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/DkRecord/index.html',
                data : { title: '带客审核' },
                controller:"DkRecordController",
                resolve:load('scripts/controllers/CRM/DkRecord.js')
            })
            .state('page.CRM.DkRecord.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/DkRecord/detailed.html',
                data: { title: '详情页' },
                controller:"DKRDetailedController",
                resolve:load('scripts/controllers/CRM/DkRecord.js')
            })


              .state('page.CRM.WaitPetition', {
                  url: '/WaitPetition',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.WaitPetition.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/WaitPetition/index.html',
                  data: { title: '推荐待上访记录' },
                controller:"PetitionListController",
                resolve:load('scripts/controllers/CRM/WaitPetition.js')
            })

            .state('page.CRM.WaitPetition.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/WaitPetition/detailed.html',
                data: { title: '详情页' },
                controller:"WPDetialController",
                resolve:load('scripts/controllers/CRM/WaitPetition.js')
            })

            /////////////////////带客待上访记录///////////////////////////
            .state('page.CRM.DKWaitPetition', {
                url: '/DKWaitPetition',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.DKWaitPetition.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/DKWaitPetition/index.html',
                data: { title: '带客待上访记录' },
                controller:"WaitVistController",
                resolve:load('scripts/controllers/CRM/WaitPetitionDK.js')
            })

            .state('page.CRM.DKWaitPetition.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/DKWaitPetition/detailed.html',
                data: { title: '详情页' },
                controller:"DKDetialController",
                resolve:load('scripts/controllers/CRM/WaitPetitionDK.js')
            })

            /////////////////////////////////////////////////////////////


              .state('page.CRM.talking', {
                  url: '/talking',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.talking.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/talking/index.html',
                  data: { title: '推荐洽谈中业务' },
                controller:"TalkingListController",
                resolve:load('scripts/controllers/CRM/Talking.js')
            })
            .state('page.CRM.talking.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/talking/detailed.html',
                  data: { title: '详情页' },
                controller:"TaklDetialController",
                resolve:load('scripts/controllers/CRM/Talking.js')
            })

            ///////////////////////新增带客洽谈////////////////////////////////
            .state('page.CRM.DKtalking', {
                url: '/DKtalking',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.DKtalking.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/DKtalking/index.html',
                data: { title: '带客洽谈中业务' },
                controller:"DKTalkingList",
                resolve:load('scripts/controllers/CRM/DKtalking.js')
            })
            .state('page.CRM.DKtalking.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/DKtalking/detailed.html',
                data: { title: '详情页' },
                controller:"DKTaklDetial",
                resolve:load('scripts/controllers/CRM/DKtalking.js')
            })

            //////////////////////////////////////////////////////////////////
              .state('page.CRM.fail', {
                  url: '/fail',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.fail.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/fail/index.html',
                  data: { title: '推荐洽谈失败' },
                controller:"FailListController",
                resolve:load('scripts/controllers/CRM/fail.js')
            })
            .state('page.CRM.fail.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/fail/detailed.html',
                  data: { title: '详情页' },
                controller:"FailDetialController",
                resolve:load('scripts/controllers/CRM/fail.js')
            })




            /////////////////////////////////////带客成功失败 chen/////////////////////////////////////////////////////////

            .state('page.CRM.DKFail', {
                url: '/DKFail',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.DKFail.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/DKFail/index.html',
                data: { title: '推荐洽谈失败' },
                controller:"DKFailListController",
                resolve:load('scripts/controllers/CRM/fail.js')
            })
            .state('page.CRM.DKFail.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/DKFail/detailed.html',
                data: { title: '详情页' },
                controller:"DKFailDetialController",
                resolve:load('scripts/controllers/CRM/fail.js')
            })

            .state('page.CRM.DKSuccess', {
                url: '/DKSuccess',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.DKSuccess.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/DKSuccess/index.html',
                data: { title: '洽谈成功' },
                controller:"DKSuccessController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })
            .state('page.CRM.DKSuccess.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/DKSuccess/detailed.html',
                data: { title: '详情页' },
                controller:"DKSuccessDetialController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })
            .state('page.CRM.DKSuccess.BLPay', {
                url: '/BLPay?id',
                templateUrl: 'views/pages/CRM/DKSuccess/Pay.html',
                data: { title: '洽谈成功打款' },
                controller:"BLPayController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })


            .state('page.CRM.PlayMoney', {
                url: '/PlayMoney',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.PlayMoney.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/PlayMoney/index.html',
                data: { title: '财务打款' },
                controller:"playMoney",
                resolve:load('scripts/controllers/CRM/PlayMoney.js')
            })
            .state('page.CRM.PlayMoney.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/PlayMoney/detailed.html',
                data: { title: '详情页' },
                controller:"playMoneyDetails",
                resolve:load('scripts/controllers/CRM/PlayMoney.js')
            })

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
              .state('page.CRM.success', {
                  url: '/success',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.success.index', {
              url: '/index',
              templateUrl: 'views/pages/CRM/success/index.html',
                  data: { title: '洽谈成功' },
                controller:"SuccessListController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })
            .state('page.CRM.success.detailed', {
              url: '/detailed?id',
              templateUrl: 'views/pages/CRM/success/detailed.html',
                  data: { title: '详情页' },
                controller:"SuccessDetialController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })
            .state('page.CRM.success.BRECPay', {
                url: '/BRECPay?id',
                templateUrl: 'views/pages/CRM/success/BRECPay.html',
                data: { title: '洽谈成功打款' },
                controller:"BRECPayController",
                resolve:load('scripts/controllers/CRM/Success.js')
            })


              .state('page.CRM.CustomerInformation', {
                  url: '/CustomerInformation',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.CustomerInformation.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/CustomerInformation/index.html',
                  data: { title: '带客推荐' },
                controller:"CInfoListController",
                resolve:load('scripts/controllers/CRM/CustomerInformation.js')
            })
            .state('page.CRM.CustomerInformation.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/CustomerInformation/detailed.html',
                  data: { title: '详情页' },
                controller:"CIDetialController",
                resolve:load('scripts/controllers/CRM/CustomerInformation.js')
            })



              .state('page.CRM.strike', {
                  url: '/strike',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.strike.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/strike/index.html',
                data: { title: '所有成交' },
                controller:"SCInfoListController",
                resolve:load('scripts/controllers/CRM/strike.js')
            })
            .state('page.CRM.strike.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/strike/detailed.html',
                data: { title: '详情页' },
                controller:"SCIDetialController",
                resolve:load('scripts/controllers/CRM/strike.js')
            })

              .state('page.CRM.recommend', {
                  url: '/recommend',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.recommend.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/recommend/index.html',
                data : { title: '推荐人列表' },
                resolve:load('scripts/controllers/CRM/Recommend.js')
            })
            .state('page.CRM.recommend.detailed', {
                url: '/detailed?id',
                templateUrl: 'views/pages/CRM/recommend/detailed.html',
                resolve:load('scripts/controllers/CRM/Recommend.js'),
                data : { title: '推荐人详情页' }
            })


              .state('page.CRM.partner', {
                  url: '/partner',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.partner.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/partner/index.html',
                data : { title: '合伙人列表' },
                resolve:load('scripts/controllers/CRM/Partner.js')
            })
            .state('page.CRM.partner.detailed', {
                url: '/detailed?userId',
                templateUrl: 'views/pages/CRM/partner/detailed.html',
                data : { title: '合伙人详情页' },
                resolve:load('scripts/controllers/CRM/Partner.js')
            })
            .state('page.CRM.partner.former', {
                url: '/former?PartnersId',
                templateUrl: 'views/pages/CRM/partner/former.html',
                data : { title: '他的上家详情' },
                resolve:load('scripts/controllers/CRM/Partner.js')
            })


            .state('page.CRM.BankSet', {
                url: '/BankSet',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.BankSet.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/BankSet/index.html',
                data: { title: '银行列表' },
                //controller: 'Message',
                resolve: load('scripts/controllers/CRM/bankset.js')
            })
            .state('page.CRM.BankSet.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/BankSet/create.html',
                data : { title: '银行新建' },
                resolve:load('scripts/controllers/CRM/bankset.js')
            })
            .state('page.CRM.BankSet.edit', {
                url: '/edit?id',
                templateUrl: 'views/pages/CRM/BankSet/edit.html',
                data : { title: '银行编辑' },
                resolve:load('scripts/controllers/CRM/bankset.js')
            })



              .state('page.CRM.configure', {
                  url: '/configure',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.configure.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/configure/index.html',
                data : { title: '等级列表' },
                resolve:load('scripts/controllers/CRM/configure.js')
            })
            .state('page.CRM.configure.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/configure/create.html',
                data : { title: '等级新建' },
                resolve:load(['scripts/controllers/CRM/configure.js','angularFileUpload'])
            })
            .state('page.CRM.configure.edit', {
                url: '/edit?id',
                templateUrl: 'views/pages/CRM/configure/edit.html',
                data : { title: '等级编辑' },
                resolve:load(['scripts/controllers/CRM/configure.js','angularFileUpload'])
            })


            .state('page.CRM.configure.indexset', {
                url: '/indexset?id',
                templateUrl: 'views/pages/CRM/configure/indexset.html',
                data : { title: '等级设置' },
                resolve:load('scripts/controllers/CRM/configure.js')
            })
            .state('page.CRM.configure.setcreate', {
                url: '/setcreate',
                templateUrl: 'views/pages/CRM/configure/setcreate.html',
                data : { title: '新建等级配置' },
                resolve:load('scripts/controllers/CRM/configure.js')
            })
            .state('page.CRM.configure.setedit', {
                url: '/setedit?id',
                templateUrl: 'views/pages/CRM/configure/setedit.html',
                data : { title: '编辑等级配置' },
                resolve:load('scripts/controllers/CRM/configure.js')
            })



              .state('page.CRM.MessageConfigure', {
                  url: '/MessageConfigure',
                  template: '<div ui-view></div>'
            })
            .state('page.CRM.MessageConfigure.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/MessageConfigure/index.html',
                  resolve: load('scripts/controllers/CRM/Message.js'),
                  data: { title: '短信配置' }
            })

            .state('page.CRM.MessageConfigure.create', {
                url: '/create',
                templateUrl: 'views/pages/CRM/MessageConfigure/create.html',
                data : { title: '短信配置新建' },
                resolve:load('scripts/controllers/CRM/Message.js')
            })
            .state('page.CRM.MessageConfigure.edit', {
                url: '/edit?id',
                templateUrl: 'views/pages/CRM/MessageConfigure/edit.html',
                data : { title: '短信配置编辑' },
                resolve:load('scripts/controllers/CRM/Message.js')
            })











            .state('page.CRM.MessageList', {
                url: '/MessageList',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.MessageList.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/MessageList/index.html',
                data: { title: '短信列表' },
                //controller: 'Message',
                resolve: load('scripts/controllers/CRM/Message.js')
            })

            .state('page.CRM.TaskList', {
                url: '/TaskList',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.TaskList.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/TaskList/index.html',

                data : { title: '任务主页' },

                resolve:load(['scripts/controllers/CRM/taskList.js'])
            })
            .state('page.CRM.TaskList.taskList', {
                url: '/taskList?id',
                templateUrl: 'views/pages/CRM/TaskList/taskList.html',
                data : { title: '任务列表' },
                resolve:load(['scripts/controllers/CRM/taskList.js'])
            })
            .state('page.CRM.TaskList.createTask', {
                url: '/createTask?taskModel',
                templateUrl: 'views/pages/CRM/TaskList/createTask.html',
                data : { title: '添加任务' },
                resolve:load(['scripts/controllers/CRM/createTask.js'])
            })
            .state('page.CRM.TaskList.taskDetail', {
                url: '/taskDetail?id',
                templateUrl: 'views/pages/CRM/TaskList/taskDetail.html',
                data : { title: '任务详情' },
                resolve:load(['scripts/controllers/CRM/createTask.js'])

            })
            .state('page.CRM.TaskConfigure',{
                url:'/TaskConfigure',
                template:'<div ui-view></div>',
                data: { title: '任务列表' }
            })
            .state('page.CRM.TaskConfigure.index', {
                url: '/index',
                templateUrl: 'views/pages/CRM/TaskConfigure/index.html',

                data : { title: '任务配置' },
                resolve:load([
                    'scripts/controllers/CRM/taskConfig.js'])

            })
            .state('page.CMS', {
                url: '/CMS',
                template: '<div ui-view></div>'
            })
            .state('page.CMS.tag',{
              url:'/tag',
              template:'<div ui-view></div>',
              resolve: load('scripts/controllers/CMS/Tag.js')
            })
            .state('page.CMS.tag.index', {
              url: '/index',
              templateUrl: 'views/pages/CMS/tag/index.html',
                data: { title: '标签页' }
            })
            .state('page.CMS.tag.edit', {
              url: '/edit?id',
              templateUrl: 'views/pages/CMS/tag/edit.html',
                  data : { title: '编辑页' }
            })
            .state('page.CMS.tag.create', {
              url: '/create',
              templateUrl: 'views/pages/CMS/tag/create.html',
                  data : { title: '新建页' }
            })

              .state('page.CMS.ad',{
                url:'/ad',
                template:'<div ui-view></div>',
                resolve:load('scripts/controllers/CMS/Ad.js')
              })
            .state('page.CMS.ad.index', {
              url: '/index',
              templateUrl: 'views/pages/CMS/ad/index.html',
                  data : { title: '广告页' }
            })
            .state('page.CMS.ad.edit', {
              url: '/edit',
              templateUrl: 'views/pages/CMS/ad/edit.html',
                  data : { title: '编辑页' }
            })
            .state('page.CMS.ad.create', {
              url: '/create',
              templateUrl: 'views/pages/CMS/ad/create.html',
                  data : { title: '新建页' }
            })

              .state('page.CMS.channel',{
                url:'/channel',
                template:'<div ui-view></div>',
                resolve:load('scripts/controllers/CMS/Channel.js')
              })
            .state('page.CMS.channel.index', {
              url: '/index',
              templateUrl: 'views/pages/CMS/channel/index.html',
                  data : { title: '栏目管理' }
            })
            .state('page.CMS.channel.edit', {
              url: '/edit?id',
              templateUrl: 'views/pages/CMS/channel/edit.html',
                  data : { title: '编辑页' }
            })
            .state('page.CMS.channel.create', {
              url: '/create',
              templateUrl: 'views/pages/CMS/channel/create.html',
                  data : { title: '新建页' }
            })

            .state('page.CMS.content',{
              url:'/content',
              template:'<div ui-view></div>',
              resolve:load('scripts/controllers/CMS/Content.js')
            })
            .state('page.CMS.content.index', {
              url: '/index',
              templateUrl: 'views/pages/CMS/content/index.html',
                  data : { title: '内容页' }
            })
            .state('page.CMS.content.edit', {
              url: '/edit?id',
              templateUrl: 'views/pages/CMS/content/edit.html',
                  data : { title: '编辑页' }

            })
            .state('page.CMS.content.create', {
              url: '/create',
              templateUrl: 'views/pages/CMS/content/create.html',
                  data : { title: '新建页' },
                  resolve:load('angularFileUpload')
            })

            //.state('page.CMS.set', { url: '/set', template: '<div ui-view></div>' })
            //.state('page.CMS.set.index', {
            //  url: '/index',
            //  templateUrl: 'views/pages/CMS/set/index.html',
            //    data: { title: '设置' },
            //  controller: 'SettingController',
            //    resolve: load('scripts/controllers/CMS/Setting.js')
            //})

            .state('page.CMS.fileManager', { url: '/fileManager', template: '<div ui-view></div>' })
            .state('page.CMS.fileManager.index', {
              url: '/index',
              templateUrl: 'views/pages/CMS/fileManager/index.html',
                  data : { title: '文件管理' }
            })


          //-----------------------end-------------------


          //-----------------------UC--------------------
          .state('page.UC',{
              url:'/UC',
              template:'<div ui-view></div>',
              abstract: true
            })
            .state('page.UC.role',{
              url:'/role',
              template:'<div ui-view></div>',
              resolve:load('scripts/controllers/UC/Role.js'),
              abstract: true
            })
              .state('page.UC.role.index',{
                url:'/index',
                data:{title:'角色列表'},
                templateUrl:'views/pages/UC/Role/index.html'
              })
            .state('page.UC.role.edit',{
              url:'/edit?id',
              data:{title:'编辑角色'},
              templateUrl:'views/pages/UC/Role/edit.html'
            })
            .state('page.UC.role.create',{
              url:'/create',
              data:{title:'新建角色'},
              templateUrl:'views/pages/UC/Role/create.html'
            })
            .state('page.UC.role.permission',{
              url:'/permission?id',
              data:{title:'编辑权限'},
              templateUrl:'views/pages/UC/Role/permission.html'
            })
            .state('page.UC.index',{
                url:'/index',
                data:{title:'用户列表'},
                templateUrl:'views/pages/UC/index.html',
                resolve:load('scripts/controllers/UC/User.js')
            })
            .state('page.UC.edit',{
                url:'/edit?id',
                data:{title:'编辑页'},
                templateUrl:'views/pages/UC/edit.html',
                resolve:load('scripts/controllers/UC/User.js')
            })
          //-----------------------end-------------------

          //---------------------商品管理-------------------------

            .state('page.Trading',{
                url:'/Trading',
                template:'<div ui-view></div>'
            })
            .state('page.Trading.product',{
                url:'/product',
                template:'<div ui-view></div>'
            })
            .state('page.Trading.order',{
                url:'/order',
                template:'<div ui-view></div>'
            })
            .state('page.Trading.CommissionRatio',{
                url:'/CommissionRatio',
                template:'<div ui-view></div>'
            })
            .state('page.Trading.bill',{
                url:'/bill',
                template:'<div ui-view></div>'
            })
            .state('page.Trading.product.product', {
                url: '/product',
                templateUrl: 'views/pages/Trading/product/product.html',
                data : { title: '商品管理' },
                resolve: load(['scripts/controllers/Trading/Product.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.product.edit', {
                url: '/edit?productId',
                templateUrl: 'views/pages/Trading/product/editProduct.html',
                data : { title: '商品编辑' },
                resolve:load(['scripts/controllers/Trading/EditProduct.js','angularFileUpload'])
            })
            .state('page.Trading.product.createProduct', {
                url: '/createProduct',
                templateUrl: 'views/pages/Trading/product/createProduct.html',
                data : { title: '添加商品' },
                resolve: load(['scripts/controllers/Trading/CreateProduct.js','scripts/controllers/vectormap.js','angularFileUpload'])
            })
            .state('page.Trading.product.brand', {
                url: '/brand',
                templateUrl: 'views/pages/Trading/product/brand.html',
                data : { title: '品牌项目' },
                controller: 'BrandListController',
                resolve: load(['scripts/controllers/Trading/Brand.js','angularFileUpload'])
            })
            .state('page.Trading.product.upProductBrand', {
                url: '/upProductBrand?brandId',
                templateUrl: 'views/pages/Trading/product/upProductBrand.html',
                data: { title: '品牌修改' },
                //controller: 'upProductBrandController'
               resolve:load(['scripts/controllers/Trading/UpProductBrand.js','angularFileUpload'])
            })
            .state('page.Trading.product.createbrand', {
                url: '/createbrand',
                templateUrl: 'views/pages/Trading/product/createbrand.html',
                data : { title: '新建品牌' },
                controller: 'CreatBrandController',
                resolve: load(['scripts/controllers/Trading/Brand.js','angularFileUpload'])
            })

            .state('page.Trading.product.brandParameter', {
                url: '/brandParameter',
                templateUrl: 'views/pages/Trading/product/brandParameter.html',
                data : { title: '项目参数' },
                resolve: load(['scripts/controllers/Trading/Brand.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.product.parameter', {
                url: '/parameter',
                templateUrl: 'views/pages/Trading/product/parameter.html',
                data : { title: '分类参数' },
                resolve: load(['scripts/controllers/Trading/Parameter.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.product.area', {
                url: '/area',
                templateUrl: 'views/pages/Trading/area/index.html',
                data : { title: '商品分类' },
                resolve: load(['scripts/controllers/Trading/Area.js','scripts/controllers/vectormap.js'])
            })


            .state('page.Trading.product.classify', {
                url: '/classify',
                templateUrl: 'views/pages/Trading/product/classify.html',
                data : { title: '商品分类' },
                resolve: load(['scripts/controllers/Trading/Classify.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.bill.bill', {
                url: '/bill',
                templateUrl: 'views/pages/Trading/bill/bill.html',
                data : { title: '账单管理' },
                resolve: load(['scripts/controllers/Trading/Bill.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.bill.createBill', {
                url: '/createBill?orderId',
                templateUrl: 'views/pages/Trading/bill/createBill.html',
                data : { title: '生成账单' },
                controller:'createBillController',
                resolve: load(['scripts/controllers/Trading/Bill.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.order.order', {
                url: '/order',
                templateUrl: 'views/pages/Trading/order/order.html',
                data : { title: '订单管理' },
                resolve: load(['scripts/controllers/Trading/Order.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.order.Negotiateorder', {
                url: '/Negotiateorder',
                templateUrl: 'views/pages/Trading/order/NegotiateOrderList.html',
                data : { title: '洽谈后订单' },
                controller:'NegotiateOrderController',
                resolve: load(['scripts/controllers/Trading/Order.js','scripts/controllers/vectormap.js'])
            })
            .state('page.Trading.CommissionRatio.index', {
                url: '/index',
                templateUrl: 'views/pages/Trading/CommissionRatio/index.html',
                data : { title: '佣金比例' },
                //controller:'CommissionController',
                resolve: load('scripts/controllers/Trading/Commission.js')
            })

          /*优惠券*/

            // Coupons  router
            .state('page.event', {
                url: '/event',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons', {
                url: '/Coupons',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons.type', {
                url: '/type',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons.manage', {
                url: '/manage',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons.user', {
                url: '/user',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons.active', {
                url: '/active',
                template: '<div ui-view></div>'
            })
            .state('page.event.Coupons.type.index', {
                url: '/index',
                templateUrl: 'views/pages/event/Coupons/type/index.html',
                resolve:load('scripts/controllers/event/couponCategory.js')
            })
            .state('page.event.Coupons.type.create', {
                url: '/create',
                templateUrl: 'views/pages/event/Coupons/type/create.html',
                resolve:load('scripts/controllers/event/couponCategory.js')
            })
            .state('page.event.Coupons.type.edit', {
                url: '/edit?id',
                templateUrl: 'views/pages/event/Coupons/type/edit.html',
                resolve:load('scripts/controllers/event/couponCategory.js')
            })
            .state('page.event.Coupons.manage.index', {
                url: '/index',
                templateUrl: 'views/pages/event/Coupons/manage/index.html',
                resolve:load('scripts/controllers/event/Coupon.js')
            })
            .state('page.event.Coupons.manage.create', {
                url: '/create',
                templateUrl: 'views/pages/event/Coupons/manage/create.html',
                resolve:load('scripts/controllers/event/Coupon.js')
            })
            .state('page.event.Coupons.manage.edit', {
                url: '/edit?id',
                templateUrl: 'views/pages/event/Coupons/manage/edit.html',
                resolve:load('scripts/controllers/event/Coupon.js')
            })
            .state('page.event.Coupons.user.query', {
                url: '/query',
                templateUrl: 'views/pages/event/Coupons/user/query.html',
                resolve: load('scripts/controllers/event/active.js')

            })
            //.state('page.event.Coupons.user.list', {
            //    url: '/list',
            //    templateUrl: 'views/pages/event/Coupons/user/list.html',
            //    resolve: load('scripts/controllers/event/active.js')
            //
            //})
            .state('page.event.Coupons.active.active', {
                url: '/active',
                templateUrl: 'views/pages/event/Coupons/active/active.html',
                data : { title: '激活优惠券' },
                resolve: load('scripts/controllers/event/active.js')
            })

            .state('page.event.chip', {
                url: '/chip',
                template: '<div ui-view></div>'
            })
            .state('page.event.chip.chip', {
                url: '/chip',
                templateUrl: 'views/pages/event/chip/chip.html',
                data : { title: '众筹列表' },
                resolve: load('scripts/controllers/event/chip.js')
            })
            .state('page.event.chip.chipCreate', {
                url: '/chipCreate',
                templateUrl: 'views/pages/event/chip/chipCreate.html',
                data : { title: '众筹列表' },
                controller: 'creatChipController',
                resolve: load(['scripts/controllers/event/chip.js','angularFileUpload'])
            })
            .state('page.event.chip.chipUp?crowId=', {
                url: '/chipUp?crowId=',
                templateUrl: 'views/pages/event/chip/chipUp.html',
                data : { title: '众筹列表' },
                controller: 'upChipController',
                resolve: load(['scripts/controllers/event/chip.js','angularFileUpload'])
            })
          //活动


            .state('page.CRM.activity', {
                url: '/activity',
                template: '<div ui-view></div>'
            })
            .state('page.CRM.activity.activityList', {
                url: '/activityList',
                templateUrl: 'views/pages/CRM/activity/activityList.html',
                data: { title: '活动列表' },
                resolve: load(['scripts/controllers/CRM/activity.js'])
                //controller:"activityController"
            })
            .state('page.CRM.activity.Edit', {
                url: '/Edit?id',
                templateUrl: 'views/pages/CRM/activity/Edit.html',
                data: { title: '活动编辑' },
                resolve: load(['scripts/controllers/CRM/editActivity.js'])
            })
            .state('page.CRM.activity.Addactivity', {
                url: '/Addactivity',
                templateUrl: 'views/pages/CRM/activity/Addactivity.html',
                data: { title: '新建活动' },
                resolve: load(['scripts/controllers/CRM/Addactivity.js'])
            })










            //.state('page.activity.add',{
            //    url:'/Addactivity',
            //    templateUrl:'views/pages/CRM/activity/Addactivity.html',
            //    data:{title:'添加活动'}
            //})


          //-----------------------end-------------------

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
                                angular.forEach(MODULE_CONFIG, function (module) {
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
      }
    ]
  )
