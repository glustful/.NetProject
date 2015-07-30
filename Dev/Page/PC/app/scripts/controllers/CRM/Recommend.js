/**
 * Created by lhl on 2015/5/12 推荐经纪人
 */

angular.module("app").controller('RecommendIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            pageSize: 10
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/RecommendAgent/GetRecommendAgentList',{
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

angular.module("app").controller('PartnerDetailedController', [
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {

        alert( $stateParams.id)
        $scope.getInfo = function() {
            $http.get(SETTING.ApiUrl + '/RecommendAgent/GetRecommendAgentListById?id=' + $stateParams.id, {
                'withCredentials': true
            }).success(function (data) {
                $scope.list = data.List;
            });
        };
        $scope.getInfo();
    }
]);

