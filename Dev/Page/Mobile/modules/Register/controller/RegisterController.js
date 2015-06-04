/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http){
    $scope.register={
        UserType:'æ≠ºÕ»À',
        Brokername:'zhangsan',
        UserName:'xiaosan',
        Phone:'13629609670',
        Password:'123456'
    }
    $scope.registerSubmit=function(){
        $http.post(SETTING.ApiUrl+'/AdminRecom/AddBroker'+$scope.register).success(function(data){
        });
    }

})