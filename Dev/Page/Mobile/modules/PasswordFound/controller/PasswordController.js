/**
 * Created by gaofengming on 2015/6/2.
 */


app.controller('PasswordController',['$scope','$http','$state',function($scope,$http,$state){
    $scope.userFound={
        Phone:'',
        Hidm:'',
        Yzm:'',
        first_password:'',
        second_password:''
    }
    $scope.Found= function(){
        console.log($scope.userFound);
        $http.post(SETTING.ApiUrl+'/User/ForgetPassword',$scope.userFound,{'withCredentials':true}).success(function(data){
                if(data.Status==false){
                    $scope.ErrorTip=data.Msg;
                    console.log(data.Msg)
                }
              else{
                    $state.go('user.login')
                }

            }
        )
    }

    //验证码
    $scope.YZM={
        Mobile:'',
        SmsType:'2'
    }
    $scope.GetSMS = function() {
        if($scope.userFound.Phone==""  && $scope.userFound.Phone==undefined) {
            $scope.FoundTip='请输入手机号码';
            alert("请输入手机号码");
            return false;
        }
            if(!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test($scope.userFound.Phone))
            {
                $scope.FoundTip='请输入有效的手机号码';
                return false;
            }
        settime();
        $scope.YZM.Mobile = $scope.userFound.Phone;
        $http.post(SETTING.ApiUrl + '/SMS/SendSMS', $scope.YZM, {'withCredentials': true}).success(function (data) {

            console.log('data');
            if (data.Message == "1") {
                $scope.userFound.Hidm = data.Desstr;
            } else {
                //alert("短信发送失败，请与客户联系！");
                console.log("短信发送失败，请与客户联系！");
            }
        });
    }
}])


//两次密码输入验证
function check()
{
    var pass1 = document.getElementById("first_password");
    var pass2 = document.getElementById("second_password");
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

//计时器
var countdown=60;
function settime() {
    var obj= document.getElementById("btnsms");
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