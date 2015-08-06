/**
 * Created by lhl on 2015/5/12 推荐经纪人
 */

angular.module("app").controller('RecommendIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
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

