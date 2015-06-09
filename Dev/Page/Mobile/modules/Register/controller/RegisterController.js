/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http,$state){
    $scope.register={
        UserName:'',
        Password:'',
        SecondPassword:'',
        Phone:'',
        MobileYzm:'',
        Hidm:''
    }
    $scope.registerSubmit = function(){
        $http.post(SETTING.ApiUrl+'/User/AddBroker',$scope.register,{'withCredentials':true}).success(function(data){
            console.log(data);
            $state.go('user.login')
        })
    }


    $scope.YZM={
        Mobile:'',
        SmsType:'0'
    }
    $scope.GetSMS = function(){
        if($scope.register.Phone!=""  && $scope.register.Phone!=undefined)
        {
             if(!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test($scope.register.Phone))
            {
                alert('请输入有效的手机号码！');
                return false;
            }else
            {
                settime();
             $scope.YZM.Mobile=$scope.register.Phone;
             $http.post(SETTING.ApiUrl+'/SMS/SendSMS', $scope.YZM,{'withCredentials':true}).success(function(data){

              alert(data);
                 if (data.Message=="1")
                 {
                     $scope.register.Hidm=data.Desstr;
                 }else{
                     alert("短信发送失败，请与客户联系！");
                 }
            });
            }

        }else{
            alert("请输入您的手机号码");
        }

    }
})

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