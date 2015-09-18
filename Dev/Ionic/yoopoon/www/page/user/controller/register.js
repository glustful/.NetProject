

app.controller('register',['$http','$scope',function($http,$scope){

    $scope.signer ={
        UserName:'',
        Password:'',
        SecondPassword:''
    }
    $scope.sign = function(){
        $http.post(SETTING.ApiUrl +'/Member/AddMember',$scope.signer,{'withCredentials':true}).success(function(data){
                if(data.Status==false){
                    $scope.tip=data.Msg;
                }
            else{
                    console.log("注册成功")
                }
            console.log(data);
        })
    };

}]);

//两次密码输入验证
function check()
{
    var pass1 = document.getElementById("FPassword");
    var pass2 = document.getElementById("SPassword");
    var tips= document.getElementById("errorTip");
    if(pass1.value!=pass2.value)
    {
        tips.innerHTML="两次密码输入不一致，请重新输入！";
    }else{
        tips.innerHTML="";
    }
    if(pass1.value==""||pass2.value=="")
    {
        tips.innerHTML="请输入密码";
    }
}