/**
 * Created by lhl on 2015/5/12 等级配置
 */
angular.module("app").controller('configureIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name:'',
            page: 1,
            pageSize: 1
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/Level/SearchLevel',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };
        $scope.getList();

    }
]);

angular.module("app").controller('configureCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.LevelModel = {
        Id: 0,
        Name:'',
        Url:'',
        Describe:''
    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Level/DoCreate',$scope.LevelModel,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.index");
            }else{
                    alert(data.Msg);
            }

        });
    }
}]);

angular.module("app").controller('configureEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Level/GetLevel/' + $stateParams.id).success(function(data){
        $scope.LevelModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Level/DoEdit',$scope.LevelModel,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.index");
            }else{
                alert(data.Msg);
            }
        });
    }
}]);







angular.module("app").controller('configureSetIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name:'',
            page: 1,
            pageSize: 1
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/LevelConfig/SearchLevelConfig',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };
        $scope.getList();

    }
]);

angular.module("app").controller('configureSetCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.LevelConfig = {
        Id: 0,
        Name:'',
        Value:'',
        Describe:''
    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/LevelConfig/DoCreate',$scope.LevelConfig,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.indexset");
            }else{
                alert(data.Msg);
            }

        });
    }
}]);

angular.module("app").controller('configureSetEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/LevelConfig/GetLevelConfig/' + $stateParams.id).success(function(data){
        $scope.LevelConfig =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/LevelConfig/DoEdit',$scope.LevelConfig,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.indexset");
            }else{
                alert(data.Msg);
            }
        });
    }
}]);





