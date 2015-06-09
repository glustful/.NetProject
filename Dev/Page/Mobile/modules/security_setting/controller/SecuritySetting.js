/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('SecuritySettingController',function($scope,$http,$state){
    $scope.password ={
        oldPassword:'',
        newPassword:''
    } ;
    $scope.saveInfo=function(){
        $http.post(SETTING.ApiUrl+'/User/ChangePassword',$scope.password,{'withCredentials':true}).success(function(data){
        $state.go('app.setting')
        })
    }

})

function check()
{
    var pass1 = document.getElementById("NewPassword1");
    var pass2 = document.getElementById("NewPassword2");
    var tips= document.getElementById("errorTip");
    if(pass1.value!=pass2.value)
    {
        tips.innerHTML="两次密码不一致";
    }else{
        tips.innerHTML="";
    }
    if(pass1.value==""||pass2.value=="")
    {
        tips.innerHTML="";
    }
}




var countdown=60;
function settime(obj) {
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