/**
 * Created by Yunjoy on 2015/5/9.
 */
angular.module("app").controller('AdvertisementIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            key: '',
            page: 1,
            pageSize: 10,
            totalPage:1
        };

        var getAdvertisementList = function() {
            $http.get(SETTING.ApiUrl+'/Setting/Index',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data;
            });
        };
        $scope.getList = getAdvertisementList;
        getAdvertisementList();

        $scope.Save = function(){

        }
    }
]);
