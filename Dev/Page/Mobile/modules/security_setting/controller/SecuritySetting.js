/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('SecuritySettingController',function($scope,$http){
    $scope.password ={
        oldPassword:'',
        newPassword:''
    } ;
    $scope.saveInfo=function(){
        $http.post(SETTING.ApiUrl+'/User/ChangePassword',$scope.password,{'withCredentials':true}).success(function(data){

        alert(data);
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