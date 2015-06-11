/**
 * Created by lhl on 2015/5/16 代客管理
 */
angular.module("app").controller('dkIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"带客人员",
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



angular.module("app").controller('dkDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
//个人信息
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBroker?id=' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.BusmanModel =data;
    });
}]);

angular.module("app").controller('UserCreateController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){

    $scope.UserModel={

        Password:"",
        Brokername:"",
        Phone:"",
        UserType:"带客人员",
        UserName:""
    };

    $scope.Save = function(){
        console.log($scope.UserModel.UserType);
        $http.post(SETTING.ApiUrl + '/AdminRecom/AddBroker',$scope.UserModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                console.log(data);
            }else{
                console.log("error");
            }
        });
    }
}]);