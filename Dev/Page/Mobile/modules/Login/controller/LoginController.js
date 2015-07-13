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
                AuthService.doLogin($scope.user.name,$scope.user.password,function(){
                  //  console.log("$scope.user");
                $state.go('app.personal');
             //   $state.go("app.personal_user");
                },function(data){
                    $scope.errorTips = data.Msg;
                })
    }
}]);

