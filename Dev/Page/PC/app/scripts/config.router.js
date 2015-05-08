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
              if(next.name==='access.signin'){
                  return;
              }
              if(!AuthService.IsAuthenticated()){
                  event.preventDefault();
                  $state.go('access.signin');
              }
          });
      }
    ]
  )
  .config(
    [          '$stateProvider', '$urlRouterProvider', 'MODULE_CONFIG',
      function ( $stateProvider,   $urlRouterProvider,  MODULE_CONFIG ) {
        $urlRouterProvider
          .otherwise('/access/signin');
        $stateProvider
          .state('app', {
            abstract: true,
            url: '/app',
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
           /* .state('app.dashboard', {
              url: '/dashboard',
              templateUrl: 'views/pages/dashboard.html',
              data : { title: 'Dashboard' },
              resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })*/
            //-------------------------yangbo 2015.4.30 start----------------
            .state('app.AgentManagement', {
                url: '/AgentManagement',
                templateUrl: 'views/pages/AgentManagement.html',
                data : { title: '经纪人管理' },
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //-------------------------end-----------------------------------
            //-------------------------yangbo 2015.4.30 start----------------
            .state('app.BusinessManagement', {
                url: '/BusinessManagement',
                templateUrl: 'views/pages/BusinessManagement.html',
                data : { title: '商家管理' },
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //-------------------------end-----------------------------------
           /* .state('app.wall', {
              url: '/wall',
              templateUrl: 'views/pages/dashboard.wall.html',
              data : { title: 'Wall', folded: true }
            })*/
            //=================================yangbao 2015.4.25 start=======================================================
                        .state('app.AdminManagement', {
                            url: '/AdminManagement',
                            templateUrl: 'views/pages/AdminManagement.html',
                            data: { title: 'Admin管理'},
                            resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
                        })  // ���ŷ���ҳ��

            //========================================================================================
            //------------------------------yangbo 2015.4.30 start---------------------------
            .state('app.AdminWork', {
                url: '/AdminWork',
                templateUrl: 'views/pages/AdminWork.html',
                data: { title: '待审核推荐'},
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //-----------------------------------end------------------------------
            //----------------------------------yangbo 2015.4.30 start-----------------
            .state('app.BusinessWork', {
                url: '/BusinessWork',
                templateUrl: 'views/pages/BusinessWork.html',
                data: { title: '待上访记录'},
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //--------------------------------------end-------------------------
            //----------------------------yangbo 2015.4.30 start----------------
            .state('app.SecretaryWork', {
                url: '/SecretaryWork',
                templateUrl: 'views/pages/SecretaryWork.html',
                data: { title: '洽谈中业务'},
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //----------------------------end--------------
            //-------------------yangbo 2015.4.30 start------------
            .state('app.FinanceWork', {
                url: '/FinanceWork',
                templateUrl: 'views/pages/FinanceWork.html',
                data: { title: '洽谈失败'},
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //----------------------------------------end---------------
            //-------------------yangbo 2015.4.30 start------------
            .state('app.failed', {
                url: '/failed',
                templateUrl: 'views/pages/failed.html',
                data: { title: '洽谈成功'},
                resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js'])
            })
            //----------------------------------------end---------------

            .state('app.todo', {
              url: '/todo',
              templateUrl: 'apps/todo/todo.html',
              data : { title: 'Todo', theme: { primary: 'indigo-800'} },
              controller: 'TodoCtrl',
              resolve: load('apps/todo/todo.js')
            })
            .state('app.todo.list', {
                url: '/{fold}'
            })
            .state('app.note', {
              url: '/note',
              templateUrl: 'apps/note/main.html',
              data : { theme: { primary: 'blue-grey'} }
            })
            .state('app.note.list', {
              url: '/list',
              templateUrl: 'apps/note/list.html',
              data : { title: 'Note'},
              controller: 'NoteCtrl',
              resolve: load(['apps/note/note.js', 'moment'])
            })
            .state('app.note.item', {
              url: '/{id}',
              views: {
                '': {
                  templateUrl: 'apps/note/item.html',
                  controller: 'NoteItemCtrl',
                  resolve: load(['apps/note/note.js', 'moment'])
                },
                'navbar@': {
                  templateUrl: 'apps/note/navbar.html',
                  controller: 'NoteItemCtrl'
                }
              },
              data : { title: '', child: true }
            })
          .state('ui', {
            url: '/ui',
            abstract: true,
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
            // components router
            .state('ui.component', {
              url: '/component',
              abstract: true,
              template: '<div ui-view></div>'
            })
             /* .state('ui.component.arrow', {
                url: '/arrow',
                templateUrl: 'views/ui/component/arrow.html',
                data : { title: 'Arrows' }
              })*/
            //------------------------------------yangbo 4.30 start--------------------
            .state('ui.component.Admin', {
                url: '/Admin',
                templateUrl: 'views/ui/component/Admin.html',
                data : { title: 'Admin' }
            })
            //-------------------------end-------------------------
            //----------------------------yangbo 4.30 start--------------
            .state('ui.component.business', {
                url: '/business',
                templateUrl: 'views/ui/component/business.html',
                data : { title: '商家' }
            })
              .state('ui.component.badge-label', {
                url: '/badge-label',
                templateUrl: 'views/ui/component/badge-label.html',
                data : { title: 'Badges & Labels' }
              })
              .state('ui.component.button', {
                url: '/button',
                templateUrl: 'views/ui/component/button.html',
                data : { title: 'Buttons' }
              })
              .state('ui.component.color', {
                url: '/color',
                templateUrl: 'views/ui/component/color.html',
                data : { title: 'Colors' }
              })
              .state('ui.component.grid', {
                url: '/grid',
                templateUrl: 'views/ui/component/grid.html',
                data : { title: 'Grids' }
              })
              .state('ui.component.icon', {
                url: '/icons',
                templateUrl: 'views/ui/component/icon.html',
                data : { title: 'Icons' }
              })
              .state('ui.component.list', {
                url: '/list',
                templateUrl: 'views/ui/component/list.html',
                data : { title: 'Lists' }
              })
              .state('ui.component.nav', {
                url: '/nav',
                templateUrl: 'views/ui/component/nav.html',
                data : { title: 'Navs' }
              })
              .state('ui.component.progressbar', {
                url: '/progressbar',
                templateUrl: 'views/ui/component/progressbar.html',
                data : { title: 'Progressbars' }
              })
              .state('ui.component.streamline', {
                url: '/streamline',
                templateUrl: 'views/ui/component/streamline.html',
                data : { title: 'Streamlines' }
              })
              .state('ui.component.timeline', {
                url: '/timeline',
                templateUrl: 'views/ui/component/timeline.html',
                data : { title: 'Timelines' }
              })
              .state('ui.component.uibootstrap', {
                url: '/uibootstrap',
                templateUrl: 'views/ui/component/uibootstrap.html',
                resolve: load('scripts/controllers/bootstrap.js'),
                data : { title: 'UI Bootstrap' }
              })
            // material routers
            .state('ui.material', {
              url: '/material',
              template: '<div ui-view></div>',
              resolve: load('scripts/controllers/material.js')
            })
              /*.state('ui.material.button', {
                url: '/button',
                templateUrl: 'views/ui/material/button.html',
                data : { title: 'Buttons' }
              })*/
            //------------------------yangbo 4.30 start-----------
              .state('ui.material.recommended', {
                url: '/recommended',
                templateUrl: 'views/ui/material/recommended.html',
                data : { title: '带客推荐' }
              })
            //-----------------------------------------end-------
             /* .state('ui.material.icon', {
                url: '/icon',
                templateUrl: 'views/ui/material/icon.html',
                data : { title: 'Icons' }
              })*/
            //---------------------------yangbo 4.30 start----------
            .state('ui.material.strike', {
             url: '/strike',
             templateUrl: 'views/ui/material/strike.html',
             data : { title: '所有成交' }
             })
            //------------------------end--------------
              .state('ui.material.card', {
                url: '/card',
                templateUrl: 'views/ui/material/card.html',
                data : { title: 'Card' }
              })
              .state('ui.material.form', {
                url: '/form',
                templateUrl: 'views/ui/material/form.html',
                data : { title: 'Form' }
              })
              .state('ui.material.list', {
                url: '/list',
                templateUrl: 'views/ui/material/list.html',
                data : { title: 'List' }
              })
              .state('ui.material.ngmaterial', {
                url: '/ngmaterial',
                templateUrl: 'views/ui/material/ngmaterial.html',
                data : { title: 'NG Material' }
              })
            // form routers
            .state('ui.form', {
              url: '/form',
              template: '<div ui-view></div>'
            })
              /*.state('ui.form.layout', {
                url: '/layout',
                templateUrl: 'views/ui/form/layout.html',
                data : { title: 'Layouts' }
              })*/
            //------------------------yangbo 2015 4.30 start-----------
            .state('ui.form.comBroker', {
                url: '/comBroker',
                templateUrl: 'views/ui/form/comBroker.html',
                data : { title: '推荐经纪人' }
            })
            //--------------------------------------end----------------
              .state('ui.form.element', {
                url: '/element',
                templateUrl: 'views/ui/form/element.html',
                data : { title: 'Elements' }
              })              
              .state('ui.form.validation', {
                url: '/validation',
                templateUrl: 'views/ui/form/validation.html',
                data : { title: 'Validations' }
              })
              .state('ui.form.select', {
                url: '/select',
                templateUrl: 'views/ui/form/select.html',
                data : { title: 'Selects' },
                controller: 'SelectCtrl',
                resolve: load('scripts/controllers/select.js')
              })
              .state('ui.form.editor', {
                url: '/editor',
                templateUrl: 'views/ui/form/editor.html',
                data : { title: 'Editor' },
                controller: 'EditorCtrl',
                resolve: load('scripts/controllers/editor.js')
              })
              .state('ui.form.slider', {
                url: '/slider',
                templateUrl: 'views/ui/form/slider.html',
                data : { title: 'Slider' },
                controller: 'SliderCtrl',
                resolve: load('scripts/controllers/slider.js')
              })
              .state('ui.form.tree', {
                url: '/tree',
                templateUrl: 'views/ui/form/tree.html',
                data : { title: 'Tree' },
                controller: 'TreeCtrl',
                resolve: load('scripts/controllers/tree.js')
              })
              .state('ui.form.file-upload', {
                url: '/file-upload',
                templateUrl: 'views/ui/form/file-upload.html',
                data : { title: 'File upload' },
                controller: 'UploadCtrl',
                resolve: load(['angularFileUpload', 'scripts/controllers/upload.js'])
              })
              .state('ui.form.image-crop', {
                url: '/image-crop',
                templateUrl: 'views/ui/form/image-crop.html',
                data : { title: 'Image Crop' },
                controller: 'ImgCropCtrl',
                resolve: load(['ngImgCrop','scripts/controllers/imgcrop.js'])
              })
              .state('ui.form.editable', {
                url: '/editable',
                templateUrl: 'views/ui/form/xeditable.html',
                data : { title: 'Xeditable' },
                controller: 'XeditableCtrl',
                resolve: load(['xeditable','scripts/controllers/xeditable.js'])
              })
            // table routers
            .state('ui.table', {
              url: '/table',
              template: '<div ui-view></div>'
            })
             /* .state('ui.table.static', {
                url: '/static',
                templateUrl: 'views/ui/table/static.html',
                data : { title: 'Static', theme: { primary: 'blue'} }
              })*/
            //-------------------yangbo 2015.4.30---------
            .state('ui.table.partner', {
                url: '/partner',
                templateUrl: 'views/ui/table/partner.html',
                data : { title: '合伙人', theme: { primary: 'blue'} }
            })
            //-------------------end----------------------
              .state('ui.table.smart', {
                url: '/smart',
                templateUrl: 'views/ui/table/smart.html',
                data : { title: 'Smart' },
                controller: 'TableCtrl',
                resolve: load(['smart-table', 'scripts/controllers/table.js'])
              })
              .state('ui.table.datatable', {
                url: '/datatable',
                data : { title: 'Datatable' },
                templateUrl: 'views/ui/table/datatable.html'
              })
              .state('ui.table.footable', {
                url: '/footable',
                data : { title: 'Footable' },
                templateUrl: 'views/ui/table/footable.html'
              })
              .state('ui.table.nggrid', {
                url: '/nggrid',
                templateUrl: 'views/ui/table/nggrid.html',
                data : { title: 'NG Grid' },
                controller: 'NGGridCtrl',
                resolve: load(['ngGrid','scripts/controllers/nggrid.js'])
              })
              .state('ui.table.uigrid', {
                url: '/uigrid',
                templateUrl: 'views/ui/table/uigrid.html',
                data : { title: 'UI Grid' },
                controller: "UiGridCtrl",
                resolve: load(['ui.grid', 'scripts/controllers/uigrid.js'])
              })
              .state('ui.table.editable', {
                url: '/editable',
                templateUrl: 'views/ui/table/editable.html',
                data : { title: 'Editable' },
                controller: 'XeditableCtrl',
                resolve: load(['xeditable','scripts/controllers/xeditable.js'])
              })
            // chart
            .state('ui.chart', {
              url: '/chart',
              templateUrl: 'views/ui/chart/chart.html',
              data : { title: 'Charts' },
              resolve: load('scripts/controllers/chart.js')
            })
            // map routers
            .state('ui.map', {
              url: '/map',
              template: '<div ui-view></div>'
            })
             /* .state('ui.map.google', {
                url: '/google',
                templateUrl: 'views/ui/map/google.html',
                data : { title: 'Gmap' },
                controller: 'GoogleMapCtrl',
                resolve: load(['ui.map', 'scripts/controllers/load-google-maps.js', 'scripts/controllers/googlemap.js'], function(){ return loadGoogleMaps(); })
              })*/
            //------------------------yangbo 2015.4.30 start-------
              .state('ui.map.admins', {
                url: '/admins',
                templateUrl: 'views/ui/map/admins.html',
                data : { title: '等级配置' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
              })
              //-------------------------end---------------------
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
            /*.state('page.profile', {
              url: '/profile',
              templateUrl: 'views/pages/profile.html',
              data : { title: 'Profile', theme: { primary: 'green'} }
            })*/
            //------------------------yangbo 2015.4.30 start--------
            .state('page.setting', {
                url: '/setting',
                templateUrl: 'views/pages/setting.html',
                data : { title: '短信列表', theme: { primary: 'green'} }
            })
            //-------------------------end----------
            //------------------------yangbo 2015.4.30 start--------
            .state('page.allocation', {
                url: '/allocation',
                templateUrl: 'views/pages/allocation.html',
                data : { title: '短信配置', theme: { primary: 'green'} }
            })
            //-------------------------end----------
            .state('page.settings', {
              url: '/settings',
              templateUrl: 'views/pages/settings.html',
              data : { title: 'Settings' }
            })
            .state('page.blank', {
              url: '/blank',
              templateUrl: 'views/pages/blank.html',
              data : { title: 'Blank' }
            })
            .state('page.document', {
              url: '/document',
              templateUrl: 'views/pages/document.html',
              data : { title: 'Document' }
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
                resolve: load('scripts/controllers/CMS/Login.js')
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
              templateUrl: 'views/pages/lockme.html'
            })
            //---------------------------yangbo 2015.5.6 start------
            .state('ui.map.list', {
                url: '/list',
                templateUrl: 'views/ui/map/list.html',
                data : { title: '任务列表' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
        //------------------------------------end----------
            //---------------------------yangbo 2015.5.6 start------
            .state('ui.map.task', {
                url: '/task',
                templateUrl: 'views/ui/map/task.html',
                data : { title: '任务配置' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
            //------------------------------------end----------
          //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.label', {
                url: '/label',
                templateUrl: 'views/ui/map/label.html',
                data : { title: '标签页' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.ad', {
                url: '/ad',
                templateUrl: 'views/ui/map/ad.html',
                data : { title: '广告页' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.topic', {
                url: '/topic',
                templateUrl: 'views/ui/map/topic.html',
                data : { title: '栏目管理' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.content', {
                url: '/content',
                templateUrl: 'views/ui/map/content.html',
                data : { title: '内容页' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.channel', {
                url: '/channel',
                templateUrl: 'views/ui/map/channel.html',
                data : { title: '频道' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.set', {
                url: '/set',
                templateUrl: 'views/ui/map/set.html',
                data : { title: '设置' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------
            //------------------------yangbo 2015.5.6 start-----------
            .state('ui.map.FM', {
                url: '/FM',
                templateUrl: 'views/ui/map/FM.html',
                data : { title: '文件管理' },
                controller: 'VectorMapCtrl',
                resolve: load('scripts/controllers/vectormap.js')
            })
          //-----------------------end-------------------


          function load(srcs, callback) {
            return {
                deps: ['$ocLazyLoad', '$q',
                  function( $ocLazyLoad, $q ){
                    var deferred = $q.defer();
                    var promise  = false;
                    srcs = angular.isArray(srcs) ? srcs : srcs.split(/\s+/);
                    if(!promise){
                      promise = deferred.promise;
                    }
                    angular.forEach(srcs, function(src) {
                      promise = promise.then( function(){
                        angular.forEach(MODULE_CONFIG, function(module) {
                          if( module.name == src){
                            if(!module.module){
                              name = module.files;
                            }else{
                              name = module.name;
                            }
                          }else{
                            name = src;
                          }
                        });
                        return $ocLazyLoad.load(name);
                      } );
                    });
                    deferred.resolve();
                    return callback ? promise.then(function(){ return callback(); }) : promise;
                }]
            }
          }
      }
    ]
  );
