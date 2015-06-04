/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http){

    $scope.registerSubmit = function(){

        var username = document.getElementById("username").value;
        var userpassword = document.getElementById("userPassword").value;
        var phoneNumber = document.getElementById("phoneNumber").value;
        $scope.register={

            Brokername:username,
            UserName:username,
            Phone:phoneNumber,
            Password:userpassword
        }
        console.log($scope.register);
        $http.post(SETTING.ApiUrl+'/User/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
            console.log(data);
        });
    }

})