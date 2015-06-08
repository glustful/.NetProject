/**
 * Created by gaofengming on 2015/5/28.
 */
var app = angular.module("zergApp");
app.controller('SecuritySettingController',['$scope','$http','AuthService',function($scope,$http,AuthService){
    //$scope.user={
    //    Brokername:'',
    //    Realname:'',
    //    Nickname:'',
    //    Sexy:'',
    //    Sfz:'',
    //    Email:'',
    //    Phone:'',
    //    Headphoto:''
    //};
    $scope.password ={
        oldPassword:'',
        newPassword:''
    } ;
//    $scope.currentuser= AuthService.CurrentUser();
//    $scope.PinPassword=function(){
//        $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByUserId?userId='+$scope.currentuser.UserId,{'withCredentials':true})
//            .success(function(response) {
//                $scope.user=response;
//                $http.post(SETTING.ApiUrl+'/BrokerInfo/SendSMS',$scope.user.Phone,{'withCredentials':true}).success(function(data){
//                    alert(data);
//                    $scope.PinSMS=data
//            })
//    })
//
//};
    $scope.saveInfo=function(){
        $http.post(SETTING.ApiUrl+'/User/ChangePassword',$scope.password,{'withCredentials':true}).success(function(data){
            alert(data);
        })
    };


}])
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