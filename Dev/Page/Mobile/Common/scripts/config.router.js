'use strict';

/**
 * @ngdoc function
 * @name app.config:uiRouter
 * @description
 * # Config
 * Config for the router
 */
var routeApp = angular.module('routeApp', ['ngRoute']);
routeApp.config(
    ['$routeProvider', function ($routeProvider) {
        $routeProvider
          .when('/Index', {
              templateUrl: 'modules/Index/view/index.html',
              data: { title: '首页' },
              controller: 'modules/Index/render/index.js'
              //resolve: load(['modules/Index/render/index.js'])
              //              access:["admin"]
          })

          .when('/Activity', {
              templateUrl: 'modules/activity/view/activity.html',
              data: { title: 'activity' }
              //              resolve: load(['scripts/controllers/chart.js','scripts/controllers/vectormap.js']),
              //              access:["admin"]
          })

          .otherwise({
              redirectTo: 'app/Index'
          })

        //-----------------------end-------------------

    }
    ]
  );
