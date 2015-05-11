/**
 * Created by Yunjoy on 2015/5/9.
 */
angular.module("app").controller('AdvertisementIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            title: '',
            page: 1,
            pageSize: 10,
            totalPage:1
        };

        var getAdvertisementList = function() {
            $http.get(SETTING.ApiUrl+'/Advertisement/Index',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data;
            });
        };
        $scope.getList = getAdvertisementList;
        getAdvertisementList();
    }
]);

angular.module("app").controller('AdvertisementDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Advertisement/Detailed/' + $stateParams.id).success(function(data){
        $scope.AdvertisementModel =data;
    });
}]);

angular.module("app").controller('AdvertisementCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.AdvertisementModel = {
        Id: 0,
        Title: '',
        Detail:'',
        Continue:'',
        ContentId:0
    };

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Advertisement/Create',$scope.AdvertisementModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.Advertisement.index");
            }
        });
    }
}]);

angular.module("app").controller('AdvertisementEditController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Advertisement/Detailed/' + $stateParams.id).success(function(data){
        $scope.AdvertisementModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Advertisement/Edit',$scope.AdvertisementModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.Advertisement.index");
            }
        });
    }
}]);