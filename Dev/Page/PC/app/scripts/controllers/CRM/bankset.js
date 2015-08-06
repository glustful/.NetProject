/**
 * Created by lhl on 2015/6/4.
 */

angular.module("app").controller('bankController', ['$http', '$scope', '$state', function ($http, $scope, $state) {
    $scope.searchCondition = {
        page: 1,
        pageSize: 10,
        orderByAll:"OrderById",//排序
        isDes:true//升序or降序,
    };
    $scope.UpOrDownImgClass="fa-caret-down";//升降序图标
    $scope.getList = function (orderByAll) {
        if(orderByAll!=undefined){
            $scope.searchCondition.orderByAll=orderByAll ;
            if($scope.searchCondition.isDes==true)//如果为降序，
            {
                $scope.UpOrDownImgClass="fa-caret-up";//改变成升序图标
                $scope.searchCondition.isDes=false;//则变成升序
            }
            else if($scope.searchCondition.isDes==false)
            {
                $scope.UpOrDownImgClass="fa-caret-down";
                $scope.searchCondition.isDes=true;
            }
        }
        $http.get(SETTING.ApiUrl + '/Bank/SearchBanks', {
            params: $scope.searchCondition ,
            'withCredentials':true
        }).success(function (data) {
            $scope.list = data.List;
            $scope.searchCondition.page = data.Condition.Page;
            $scope.searchCondition.pageSize = data.Condition.PageCount;
            $scope.totalCount = data.totalCount;
        });
    };
    $scope.getList ();
}]);


angular.module("app").controller('bankCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.BankModel = {
        Codeid: ''
    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Bank/AddBank', $scope.BankModel).success(function (data) {
            if(data.Status){
                $state.go("page.CRM.BankSet.index");
            }else{
                alert(data.Msg);
            }

        });
    }
}]);


angular.module("app").controller('bankEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Bank/GetBank/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.BankModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Bank/Update',$scope.BankModel,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.BankSet.index");
            }else{
                alert(data.Msg);
            }
        });
    }
}]);


