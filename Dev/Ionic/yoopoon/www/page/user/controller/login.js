/**
 * Created by gaofengming on 2015/9/15.
 */
app.controller('login',['$scope','$state',function($scope,$state){
    console.log("121212");
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