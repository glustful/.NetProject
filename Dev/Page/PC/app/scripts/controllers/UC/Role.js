/**
 * Created by Yunjoy on 2015/5/19.
 */
/**
 * Created by Yunjoy on 2015/5/9.
 */
angular.module("app").controller('RoleIndexController', [
    '$http','$scope','$state','$modal',function($http,$scope,$state,$modal) {

        var getRoleList = function() {
            $http.get(SETTING.ApiUrl+'/Role/GetRoles',{
                'withCredentials':true
            }).success(function(data){
                $scope.list = data;
            });
        };
        $scope.getList = getRoleList;
        getRoleList();

        $scope.del = function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要删除吗？";}
                }
            });
            modalInstance.result.then(function(){
                $http.get(SETTING.ApiUrl + '/Role/Delete',{
                        params:{
                            id:$scope.selectedId
                        },
                        'withCredentials':true
                    }
                ).success(function(data) {
                        if (data.Status) {
                            getRoleList();
                        }
                    });
            })
        };

        $scope.gotoNew = function(){
            $state.go("page.UC.role.create");
        }
    }
]);

angular.module("app").controller('RoleDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Role/GetDetail/' + $stateParams.id).success(function(data){
        $scope.RoleModel =data;
    });
}]);

angular.module("app").controller('RoleCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.RoleModel = {
        Id: 0,
        RoleName:'',
        Description:'',
        Status:''
    };

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Role/Create',$scope.RoleModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.UC.role.index");
            }
            else{
                $scope.Message=data.Msg;
            }
        });
    }
}]);

angular.module("app").controller('RoleEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Role/GetDetail/' + $stateParams.id).success(function(data){
        $scope.RoleModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Role/Edit',$scope.RoleModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.UC.role.index");
            }
        });
    }
}]);

angular.module("app").controller('RolePermissionController',['$http','$scope','$stateParams','$state',function(http,scope,stateParams,state){
    http.get(SETTING.ApiUrl + '/Role/GetRolePermission?roleId=' + stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        scope.Model =data;
    });

    scope.Save = function(){
        http.post(SETTING.ApiUrl + '/Role/SavePermission?roleId=' + stateParams.id,scope.Model,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                state.go("page.UC.role.index");
            }
        });
    }
}]);