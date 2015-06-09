/**
 * Created by gaofengming on 2015/6/1.
 */
    var app = angular.module("zergApp");
app.controller('LoginController',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.user={
        name:"",
        password:''
    }
    $scope.Login = function(){
            if($scope.user.name==null||$scope.user.password==null) {
                $scope.errorTips = '用户名或密码不能为空';
                return;
            } else {
                AuthService.doLogin($scope.user.name,$scope.user.password,function(){
                $state.go('app.partner_list')
                },function(data){
                    $scope.errorTips = data.Msg;
                })
    }

}}]);

