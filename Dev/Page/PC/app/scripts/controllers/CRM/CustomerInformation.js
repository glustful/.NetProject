/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('CInfoListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"等待上访",
            clientName:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/ClientInfo/GetClientInfoList',{params:$scope.searchCondition}).success(function(data){
                $scope.Brokerlist = data.list1;
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);

//详细信息
angular.module("app").controller('CIDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/ClientInfo/ClientInfo/' + $stateParams.id).success(function (data) {
            $scope.ARDetialModel = data;
            console.log($scope.ARDetialModel);
        });

    }
]);