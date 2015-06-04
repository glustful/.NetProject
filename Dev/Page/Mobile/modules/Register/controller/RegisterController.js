/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http){

    $scope.registerSubmit = function(){
        var usertype = '经纪人';
        var username = document.getElementById("username").value;
        var userpassword = document.getElementById("userPassword").value;
        var phoneNumber = document.getElementById("phoneNumber").value;
        $scope.register={
            UserType:usertype,
            Brokername:username,
            UserName:username,
            Phone:phoneNumber,
            Password:userpassword
        }
        console.log($scope.register);
        $http.post(SETTING.ApiUrl+'/AdminRecom/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
            console.log(data);
        });
    }

})