
/**
 * Created by lhl on 2015/5/28.
 */
app.controller('myPurseController',['$http','$scope','AuthService','$state',function($http,$scope,AuthService,$state) {
    $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
    alert(  $scope.currentuser);
    $http.get(SETTING.ApiUrl+'/BankCard/SearchAllBankByUser').success(function(response) {
        $scope.List = response.List;
        $scope.AmountMoney=response.AmountMoney;
    });


}]);


app.controller('withdrawalsController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.TxDecimal={
        Bank:'',
        Money:'',
        MobileYzm:'',
        Hidm:''
    }
    $http.get(SETTING.ApiUrl+'/BankCard/SearchAllBankByUserToSelect').success(function(response) {
        $scope.BankList = response.List;
        $scope.AmountMoney=response.AmountMoney;
    });

    $scope.Crete = function()
    {
        $http.post(SETTING.ApiUrl+'/BrokerWithdrawDetail/AddBrokerWithdrawDetail', $scope.TxDecimal, {'withCredentials': true}).success(function(datas) {
            if(datas.status=="1")
            {
                $http.go("");
            }else
            {
                alert("提现错误,请与客户联系");
            }
        });
    };

    $scope.GetSMS = function(){
        settime();
        $http.post(SETTING.ApiUrl+'/SMS/SendSmsForbroker', '4',{'withCredentials':true}).success(function(data){
            if (data.Message=="1")
            {
                $scope.TxDecimal.Hidm=data.Desstr;
            }else{
                //alert("短信发送失败，请与客户联系！");
                alert("短信发送失败，请与客户联系！");
            }
        });
    }

}]);


app.controller('bankAddController',['$http','$scope','$state',function($http,$scope,$state){

    $http.get(SETTING.ApiUrl+'/Bank/SearchAllBank').success(function(response) {
        $scope.BankList = response.List;
    });

     $scope.bankcard={
         Num:"",
         Type:"储蓄卡",
         Bank:'',
         Address:"",
         MobileYzm:'',
         Hidm:''
    };
    $scope.Crete = function()
    {
        if( $scope.bankcard.Num==undefined || $scope.bankcard.Num=="")
        {
            alert("请输入银行卡号");
            return;
        }else
        {
            if ( $scope.bankcard.Num.length < 16 ||  $scope.bankcard.Num.length > 19) {
             alert("银行卡号长度必须在16到19之间");
                return false;
            }
            var num = /^\d*$/;  //全数字
            if (!num.exec( $scope.bankcard.Num)) {
              alert("银行卡号必须全为数字");
                return false;
            }
            //开头6位
            var strBin="10,18,30,35,37,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,58,60,62,65,68,69,84,87,88,94,95,98,99";
            if (strBin.indexOf( $scope.bankcard.Num.substring(0, 2))== -1) {
               alert("银行卡号开头6位不符合规范");
                return false;
            }

        }

        $http.post(SETTING.ApiUrl+'/BankCard/AddBankCard', $scope.bankcard, {'withCredentials': true}).success(function(datas) {
            alert(datas.toString());
        });
    };

    $scope.GetSMS = function(){
                settime();
                $http.post(SETTING.ApiUrl+'/SMS/SendSmsForbroker', '3',{'withCredentials':true}).success(function(data){
                    if (data.Message=="1")
                    {
                        $scope.bankcard.Hidm=data.Desstr;
                    }else{
                        //alert("短信发送失败，请与客户联系！");
                        console.log("短信发送失败，请与客户联系！");
                    }
                });




    }

}]);

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