/**
 * Created by lhl on 2015/5/12  合伙人列表
 */
alert("1");
angular.module("app").controller('PartnerIndexController', [
    '$http','$scope',function($http,$scope) {

        alert("11");

        $scope.searchCondition = {
            name: '',
            page: 1,
            pageSize: 1
        };

        alert("2");

        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/PartnerList/SearchPartnerList',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };

        $scope.getList();
      //  getPartnerList();
     //   $scope.getList = getPartnerList;
    }
]);

