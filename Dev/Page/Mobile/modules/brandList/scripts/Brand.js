/**
 * Created by Administrator on 2015/6/10.
 */
app.controller('BrandController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
        var condition = {
            condition:$stateParams.condition==undefined?'':$stateParams.condition,
            page:1
        };
        $scope.getList = function() {
            $http.get(SETTING.ApiUrl + '/Brand/SearchBrand', {params: condition, withCredentials: true})
                .success(function (data) {
                    if (data.Count == 0) {
                        $scope.Msg = '未查询到该数据';
                    }
                    else {
                        $scope.BrandList = data.List;
                    }
                });
        };
    $scope.getList();
}]);