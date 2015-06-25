/**
 * Created by gaofengming on 2015/6/15.
 */

app.controller('AddBrokerController',function($scope,$http){
        $scope.YQM={
            Mobile:'',
            SmsType:'6'
        };
        $scope.Invite= function(){
            settime();
            $http.post(SETTING.ApiUrl+'/SMS/SendSMS',$scope.YQM,{'withCredentials':true}).success(function(data){
                console.log(data);
                if(data== "1"){
                    $scope.Tip="邀请已发送！！"
                }
                else{
                    $scope.Tip="邀请发送失败，请与客服联系！！"
                }

            })
        }
    }

)

//计时器
var countdown=60;
function settime() {
    var obj= document.getElementById("inviteSMS");
    if (countdown == 0) {

        obj.removeAttribute("disabled");
        obj.innerHTML="发送邀请";
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