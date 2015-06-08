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

        //var username = document.getElementById("username").value;
        //var userpassword = document.getElementById("userPassword").value;
        //var phoneNumber = document.getElementById("phoneNumber").value;
        console.log($scope.register);
        $http.post(SETTING.ApiUrl+'/User/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
            console.log(data);
            $state.go('user.login')
        });
    }

})