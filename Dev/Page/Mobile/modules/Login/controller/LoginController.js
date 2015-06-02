/**
 * Created by gaofengming on 2015/6/1.
 */
//angular.module("LoginController", ['$scope','$state','AuthService',function($scope,$state,AuthService){
//        $scope.Login = function(){
//            AuthService.doLogin($scope.user.name,$scope.user.password,function(){
//                $state.go('app.partner_list')
//            })
//        }
//]);
    var app = angular.module("zergApp");
app.controller('LoginController',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.Login = function(){
        AuthService.doLogin($scope.user.name,$scope.user.password,function(){
          $state.go('app.partner_list')
        })
    }
}]);
