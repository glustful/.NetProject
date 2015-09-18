/**
 * Created by Yunjoy on 2015/9/15.
 */
//ע��
app.controller('register',['$http','$scope','$state','AuthService','$ionicLoading','$timeout',function($http,$scope,$state,AuthService,$ionicLoading,$timeout){
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
                $ionicLoading.show({
                    template:data.Msg,
                    noBackdrop:true
                });
                $timeout(function(){
                    $ionicLoading.hide();
                },3000);
            }
            else{
                AuthService.doLogin($scope.signer.UserName,$scope.signer.Password,function(){
                    $ionicLoading.show({
                        template:"ע��ɹ�����¼ing..."
                    });
                    $timeout(function(){
                        $state.go("page.me");
                        $ionicLoading.hide();
                    },1000);
                })
                console.log(data.Msg);
            }
        })
    }
}])
//��������������֤
function check()
{
    var pass1 = document.getElementById("FPassword");
    var pass2 = document.getElementById("SPassword");
    var tips= document.getElementById("errorTip");
    if(pass1.value!=pass2.value)
    {
        tips.innerHTML="�����������벻һ�£����������룡";
    }else{
        tips.innerHTML="";
    }
    if(pass1.value==""||pass2.value=="")
    {
        tips.innerHTML="����������";
    }
}
