/**
 * Created by gaofengming on 2015/9/15.
 */
app.controller('login',['$scope','$state','AuthService','$ionicLoading','$timeout',function($scope,$state,AuthService,$ionicLoading,$timeout){
    $scope.user={
        userName:'',
        password:''
    };
    $scope.login = function(){
        //登录成功
        AuthService.doLogin($scope.user.userName,$scope.user.password,function(data){
            $ionicLoading.show({
                template:"登录中，请稍后..."
            });
            $timeout(function(){
                $ionicLoading.hide();
                $state.go('page.me');
            },2000);


        },
            //登录失败
            function(data){
            $ionicLoading.show({
                template:data.Msg,
                noBackdrop:true
            });
            $timeout(function(){
                $ionicLoading.hide();
            },2000);
        })
    }
}]);