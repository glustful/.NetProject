
/**
 * Created by lhl on 2015/5/28.
 */
app.controller('myPurseController',function($scope,$http){

    $http.get(SETTING.ApiUrl+'/BankCard/SearchAllBankByUser').success(function(response) {
        $scope.List = response.List;
        $scope.AmountMoney=response.AmountMoney;
    });


})


app.controller('withdrawalsController',function($scope,$http){
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
        $http.post(SETTING.ApiUrl+'/BankCard/AddBankCard', $scope.TxDecimal, {'withCredentials': true}).success(function(datas) {
            alert(datas.toString());
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
                console.log("短信发送失败，请与客户联系！");
            }
        });
    }

})


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