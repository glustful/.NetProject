/**
 * Created by Yunjoy on 2015/5/6.
 */
angular.module('app').controller('LoginControl',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.Login = function(){
        AuthService.doLogin($scope.user.name,$scope.user.password,function(){
            $state.go('page.settings');
        },function(){
            //todo:失败时候显示错误信息
        })
    }
}]);