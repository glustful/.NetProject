/**
 * Created by Yunjoy on 2015/7/30.
 */

/* Controllers */
// signin controller
app.controller('signinController', ['$scope', '$http', '$state','AuthService', function($scope, $http, $state,AuthService) {
    $scope.user = {};
    $scope.authError = null;
    $scope.Login = function(){
        AuthService.doLogin($scope.user.name,$scope.user.password,function(){
            $state.go('app.home');
        },function(data){
            $scope.authError = data.Msg;
        })
    }
}])
;