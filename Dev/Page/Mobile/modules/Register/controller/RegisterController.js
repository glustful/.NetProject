/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('registerController',function($scope,$http,$state){
    $scope.register={
        UserName:'',
        Password:'',
        SecondPassword:'',
        Mobile:'',
        MobileYzm:'',
        Hidm:''
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


    $scope.YZM={
        Mobile:'',
        SmsType:'0'
    }
    $scope.GetSMS = function(){
        if($scope.register.Mobile!=""  && $scope.register.Mobile!=undefined)
        {
//            var myreg = /^(((13[0-9]{1})|159|153)+\d{8})$/;
//            if(!myreg.test($scope.register.Mobile))
             if(!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test($scope.register.Mobile))
            {
                alert('请输入有效的手机号码！');
                return false;
            }else
            {
             $scope.YZM.Mobile=$scope.register.Mobile;
             $http.post(SETTING.ApiUrl+'/SMS/SendSMS', $scope.YZM,{'withCredentials':true}).success(function(data){

            // $scope.PinSMS=data
             // settime();

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