/**
 * Created by huangxiuyu on 2015/9/15.
 */
//start----------------------------商品分类 huangxiuyu2015.09.15-------------------------
app.controller('CategoryController',['$scope','$http',function($scope,$http){
    $scope.searchCondition={
        ifid:0
    }
    $scope.selectCategory=function(ifid){
        $scope.searchCondition.ifid=ifid;
        $http.get(SETTING.ApiUrl+'/Category/GetAllTree/',{params:$scope.searchCondition,'withCredentials':
            true}).
            success(function(data){
                $scope.list=data;
                console.log(data);
            })
    };
    $scope.selectCategory(1);

}]);
//end----------------------------商品分类 huangxiuyu2015.09.15-------------------------
