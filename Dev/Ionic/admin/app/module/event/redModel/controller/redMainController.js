/**
 * Created by Yunjoy on 2015/8/6.
 * 红包模板总处理
 */

app.controller('keyResController', ['$scope', '$http', '$state', function($scope, $http) {
    $scope.sech={
        page:0,
        pageCount:0,
        pageSize:0,
        type:""
    };

    $scope.GetKeyResList=function(){
        $http.get(SETTING.ZergWcApiUrl+"/AutoRes/GetKeyRes",{
            params:$scope.sech,   //参数
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.list=data.List;
        });
    };
    $scope.GetKeyResList();
}]);