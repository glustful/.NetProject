/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('SecuritySettingController',['$scope','$http','$state',function($scope,$http,$state){
    $scope.password ={
        OldPassword:'',
        Password:'',
        SecondPassword:'',
        MobileYzm:'',
        Hidm:''
    }
    //提交密码修改信息
    $scope.saveInfo=function(){
        $http.post(SETTING.ApiUrl+'/User/ChangePassword',$scope.password,{'withCredentials':true}).success(function(data){

                console.log(data);

               if(data.Status==false){
                   $scope.changeError=data.Msg;
               }
            else{
                   $state.go('app.setting')
               }


        })
    };
    //获取验证码
    //$scope.SmsType='1';
    $scope.pwSms=function(){
        settime();
        $http.post(SETTING.ApiUrl+'/SMS/SendSmsForbroker','1',{'withCredentials':true}).success(function(data){

            if (data.Message=="1")
            {
                $scope.password.Hidm=data.Desstr;
            }else{
                $scope.changeError="短信发送失败，请与客户联系！";
            }
        })
    }
}]);

//两次密码验证是否一致
function check()
{
    var pass1 = document.getElementById("NewPassword1");
    var pass2 = document.getElementById("NewPassword2");
    var tips= document.getElementById("errorTip");
    if(pass1.value!=pass2.value)
    {
        tips.innerHTML="两次密码输入不一致，请重新输入！";
    }else{
        tips.innerHTML="";
    }
    if(pass1.value==""||pass2.value=="")
    {
        tips.innerHTML="";
    }
}

//获取验证码计时
var countdown=60;
function settime(obj) {
    var obj= document.getElementById("pw_sms");
    if (countdown == 0) {
        obj.removeAttribute("disabled");
        obj.innerHTML="获取验证码";
        obj.style.background="#fc3b00";
        countdown = 60;
        return;
    } else {
        obj.setAttribute("disabled", true);

        obj.style.background="#996c33";
        obj.innerHTML="重新发送(" + countdown + ")";
        countdown--;
    }
    setTimeout(function() {
            settime(obj) }
        ,1000)
}

//注销
app.controller('loginOutController',['$scope','$http','$state',function($scope,$http,$state){

    $scope.loginOut=function(){
        $http.post(SETTING.ApiUrl+'/User/Logout',{'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.Status==true){
                $state.go('app.home')
            }
        })
    }
}])