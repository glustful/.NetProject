/**
 * Created by huangxiuyu on 2015/9/17.
 */
app.controller('myorder',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
$scope.Index=$stateParams.tabIndex;
    //alert($scope.Index);
}]);
