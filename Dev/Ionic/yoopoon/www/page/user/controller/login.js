/**
 * Created by gaofengming on 2015/9/15.
 */
app.controller('loginCtrl',['$scope','AuthService','$state',function($scope,AuthService,$state){
    $scope.user={
        userName:'',
        password:''
    }
    $scope.login = function(){
        AuthService.doLogin($scope.user.userName,$scope.user.password,function(data){
            console.log(data);
            $state.go('');
        },function(data){
            $scope.errorTip=data.Msg;
        })
    }
}])