/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http,$state){
    $scope.register={
        Brokername:'',
        UserName:'',
        Phone:'',
        Password:''
    }
    $scope.pinGet = function(){
        $http.post(SETTING.ApiUrl+'/BrokerInfo/SendSMS',$scope.register.Phone,{'withCredentials':true}).success(function(data){
            alert(data);
            $scope.PinSMS=data
        });
    }
    $scope.registerSubmit = function(){
        //if($scope.Brokername==null||$scope.UserName==null||$scope.Phone==null||$scope.Password==null){
        //     $scope.register_error='Honey，请填完再走嘛！';
        //    return;
        //}
        //else{
            $http.post(SETTING.ApiUrl+'/User/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
                console.log(data);
                $state.go('user.login')
            })
        //}

    }

})