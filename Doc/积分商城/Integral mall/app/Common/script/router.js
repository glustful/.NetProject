app.config(['$stateProvider','$urlRouterProvider','MAIN_CONFIG',function($stateProvider,$urlRouterProvider,MAIN_CONFIG){
    $urlRouterProvider.otherwise('/index');
    $stateProvider
        .state('index',{
            url:'/index',
            templateUrl:'Modules/index/index.html'
        })
        .state('intDetail',{
            url:'/intDetail',
            templateUrl:'Modules/intDetail/intDetail.html',
            data:{title:'积分详情'}
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