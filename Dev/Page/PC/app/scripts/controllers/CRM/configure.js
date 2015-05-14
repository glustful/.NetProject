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




angular.module("app").controller('configureIndexsetController', [
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {


        $http.get(SETTING.ApiUrl+'/RecommendAgent/PartnerListDetailed?userId=' + $stateParams.userId).success(function(data){
            $scope.list = data;
        });

    }
]);

