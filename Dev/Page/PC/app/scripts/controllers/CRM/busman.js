/**
 * Created by lhl on 2015/5/16 商家管理
 */
angular.module("app").controller('busmanIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"商家",
            page: 1,
            pageSize: 10
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };
        $scope.getList();
    }
]);



angular.module("app").controller('busmanDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
    //个人信息
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBroker?id=' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.BusmanModel =data;
    });

}]);