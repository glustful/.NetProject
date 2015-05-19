/**
 * Created by lhl on 2015/5/16 驻厂秘书管理
 */
angular.module("app").controller('zcIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            page: 1,
            pageSize: 10
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };
        $scope.getList();
    }
]);



angular.module("app").controller('zcDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){

    //个人信息
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBroker?id=' + $stateParams.userid).success(function(data){
        $scope.BrokerModel =data;
    });



}]);