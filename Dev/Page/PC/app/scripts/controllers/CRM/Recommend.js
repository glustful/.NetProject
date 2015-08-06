/**
 * Created by lhl on 2015/5/12 推荐经纪人
 */

angular.module("app").controller('RecommendIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            pageSize: 10,
            orderByAll:"OrderByBBrokername",//排序
            isDes:true//升序or降序,
        };
        //初始化所有图标
        var iniImg=function(){
            $scope.OrderByBBrokername="footable-sort-indicator";
            $scope.OrderByBrokername="footable-sort-indicator";
            $scope.OrderByPresenteebId="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderByBBrokername="fa-caret-down";//升降序图标
        $scope.getList = function (orderByAll) {
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

