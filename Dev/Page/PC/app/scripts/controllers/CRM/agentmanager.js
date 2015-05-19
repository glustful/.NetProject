/**
 * Created by lhl on 2015/5/16 经纪人管理
 */
angular.module("app").controller('agentmanagerIndexController', [
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



angular.module("app").controller('configureDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){

     //个人信息
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBroker?id=' + $stateParams.userid).success(function(data){
        $scope.BrokerModel =data;
    });

    //出入账
    $scope.searchCRZCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getCRZList  = function() {
        $http.get(SETTING.ApiUrl+'/BrokeAccount/GetPointDetailListByUserId',{params:$scope.searchCRZCondition}).success(function(data){
            $scope.listCRZ = data.List;
            $scope.searchCRZCondition.page = data.Condition.Page;
            $scope.searchCRZCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountCRZ = data.totalCount;
        });
    };
    $scope.getCRZList();

    //提现明细
    $scope.searchTXCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getTXList  = function() {
        $http.get(SETTING.ApiUrl+'/BrokerWithdrawDetail/GetBrokerWithdrawDetailListByUserId',{params:$scope.searchTXCondition}).success(function(data){
            $scope.listTX = data.List;
            $scope.searchTXCondition.page = data.Condition.Page;
            $scope.searchTXCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountTX = data.totalCount;
        });
    };
    $scope.getTXList();

    //银行卡明细
    $scope.searchBankCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getBankList  = function() {
        $http.get(SETTING.ApiUrl+'/BankCard/SearchBankCardsByUserID',{params:$scope.searchBankCondition}).success(function(data){
            $scope.listBank = data.List;
            $scope.searchBankCondition.page = data.Condition.Page;
            $scope.searchBankCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountBank = data.totalCount;
        });
    };
    $scope.getBankList();

    //积分明细
    $scope.searchJFCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getJFList  = function() {
        $http.get(SETTING.ApiUrl+'/PointDetail/GetPointDetailByUserId',{params:$scope.searchJFCondition}).success(function(data){
            $scope.listJF = data.List;
            $scope.searchJFCondition.page = data.Condition.Page;
            $scope.searchJFCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountJF = data.totalCount;
        });
    };
    $scope.getJFList();

}]);