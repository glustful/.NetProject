/**
 * Created by gaofengming on 2015/6/2.
 */
var app = angular.module("zergApp");
app.controller('registerController',['$scope','$state','$http','$stateParams','AuthService',function($scope,$state,$http,$stateParams,AuthService){
   console.log($stateParams.yqm);

    $scope.register={
        Username:'',
        Password:'',
        SecondPassword:'',
        Phone:'',
        MobileYzm:'',
        Hidm:'',
        inviteCode:$stateParams.yqm
    }
    //提交注册信息
    $scope.registerSubmit = function(){
         $scope.register.Username=$scope.register.Phone;
        $http.post(SETTING.ApiUrl+'/User/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.Status==false){
                $scope.errorTips=data.Msg;
                return;
            }
            else{
                AuthService.doLogin($scope.register.Phone,$scope.register.Password,function(){
                   // console.log("$scope.register");
                    $state.go('app.personal_user');
                },function(data){
                   // $scope.errorTips = data.Msg;
                })
               // $state.go('user.login');
            }

        })
    }

    //验证码
    $scope.YZM={
        Mobile:'',
        SmsType:'0'
    }
    //$scope.GetSMS = function(){
    //    if($scope.register.Phone!=""  && $scope.register.Phone!=undefined)
    //    {
    //         if(!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test($scope.register.Phone))
    //        {
    //            alert('请输入有效的手机号码！');
    //            $scope.ErrorTip='请输入有效的手机号码!';
    //            console.log("请输入有效的手机号码");
    //            return false;
    //        }else
    //        {
    //            settime();
    //         $scope.YZM.Mobile=$scope.register.Phone;
    //         $http.post(SETTING.ApiUrl+'/SMS/SendSMS', $scope.YZM,{'withCredentials':true}).success(function(data){
    //             if (data.Message=="1")
    //             {
    //                 $scope.register.Hidm=data.Desstr;
    //             }else{
    //                 $scope.ErrorTip='短信发送失败，请与客户联系！';
    //                 alert("短信发送失败，请与客户联系！");
    //             }
    //        });
    //        }
    //
    //    }else{
    //        $scope.ErrorTip='请输入您的手机号码！';
    //        console.log("请输入您的手机号码");
    //    }
    //
    //}
    $scope.GetSMS = function() {
        if($scope.register.Phone==""  && $scope.register.Phone==undefined) {
            $scope.ErrorTip='请输入手机号码';
            alert("请输入手机号码");
            return false;
        }
        if(!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test($scope.register.Phone))
        {
            $scope.ErrorTip='请输入有效的手机号码';
            return false;
        }
        settime();
        $scope.YZM.Mobile = $scope.register.Phone;
        $http.post(SETTING.ApiUrl + '/SMS/SendSMS', $scope.YZM, {'withCredentials': true}).success(function (data) {

            console.log('data');
            if (data.Message == "1") {
                $scope.register.Hidm = data.Desstr;
            } else {
                //alert("短信发送失败，请与客户联系！");
                console.log("短信发送失败，请与客户联系！");
            }
        });
    }
}])
//计时器
var countdown=60;
function settime() {
    var obj= document.getElementById("btnsms");
    if (countdown == 0) {

        document.getElementById("btnsms").removeAttribute("disabled");
        document.getElementById("btnsms").innerHTML="获取验证码";
        document.getElementById("btnsms").style.background="#fc3b00";
        countdown = 60;
        return;
    } else {
        document.getElementById("btnsms").setAttribute("disabled", true);

        document.getElementById("btnsms").style.background="#996c33";
        document.getElementById("btnsms").innerHTML="重新发送(" + countdown + ")";
        countdown--;
    }
    setTimeout(function() {
            settime(obj) }
        ,1000)
}
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
        tips.innerHTML=undefined;
        tips.innerHTML=undefined;
    }
    if(pass1.value==""||pass2.value=="")
    {
        tips.innerHTML="请输入密码";
    }
}