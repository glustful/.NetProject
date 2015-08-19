/**
 * Created by Administrator on 2015/5/20.
 */
angular.module("app").controller('UserListController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            userName: '',
            page: 1,
            pageSize: 10,
            totalPage: 1,
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序
        };
        var iniImg=function(){
            $scope.OrderByName="footable-sort-indicator";
            $scope.OrderByState="footable-sort-indicator";
        }
        iniImg();
        var getUserList = function (orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
                if($scope.searchCondition.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.searchCondition.isDes=false;//则变成升序
                }
                else if($scope.searchCondition.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.searchCondition.isDes=true;
                }
            }
            $http.get(SETTING.ApiUrl + '/User/GetUserList', {params: $scope.searchCondition}).success(function (data) {
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageSize;
                $scope.totalCount = data.TotalCount;
            });
        };
        $scope.getList = getUserList;
        getUserList();
        $scope.open = function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要删除吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.get(SETTING.ApiUrl + '/User/Delete', {
                        params: {
                            id: $scope.selectedId
                        }
                    }
                ).success(function (data) {
                        if (data.Status) {
                            getUserList();
                        }
                    });
            });
        }
    }
]);
angular.module("app").controller('UserEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/User/Detailed/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.UserModel =data;
    });
    $http.get(SETTING.ApiUrl + '/Role/GetRoles',{
        'withCredentials':true
    }).success(function(data){
        $scope.RoleList =data;
    });
    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/User/EditUser',$scope.UserModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.UC.index");
            }
        });
    }
}]);
