/**
 * Created by lhl on 2015/5/12  合伙人列表
 */

angular.module("app").controller('PartnerIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            pageSize: 10,
            totalPage:1
        };

        var getPartnerList = function() {
            $http.get(SETTING.ApiUrl+'/PartnerList/SearchPartnerList',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data;
            });
        };
        $scope.getList = getPartnerList;
        getPartnerList();
    }
]);

