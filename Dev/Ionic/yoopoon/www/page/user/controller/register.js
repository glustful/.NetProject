/**
 * Created by Yunjoy on 2015/9/15.
 */
//注册
app.controller('register',['$http','$scope','$state','AuthService',function($http,$scope,$state,AuthService){
    $scope.signer ={
        Phone:'',
        UserName:'',
        Password:'',
        SecondPassword:''
    }
    $scope.sign = function(){
        $scope.signer.UserName=$scope.signer.Phone;
        console.log($scope.signer);
        $http.post(SETTING.ApiUrl+'/Member/AddMember',$scope.signer,{'withCredentials':true}).success(function(data){
            if(data.Status==false){
                console.log(data.Msg);
                $scope.tip=data.Msg;
            }
            else{
                AuthService.doLogin($scope.signer.UserName,$scope.signer.Password,function(){
                    $state.go('page.me');
                })
                console.log(data.Msg);
            }
        })
    }
}])
//两次密码输入验证
function check()
{
    var pass1 = document.getElementById("FPassword");
    var pass2 = document.getElementById("SPassword");
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
