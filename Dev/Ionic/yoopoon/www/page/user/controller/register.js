

app.controller('register',['$http','$scope','$state',function($http,$scope,$state){

    $scope.signer ={
        UserName:'',
        Password:'',
        SecondPassword:''
    }
    $scope.sign = function(){
        $http.post(SETTING.ApiUrl +'/Member/AddMember',$scope.signer,{'withCredentials':true}).success(function(data){
                if(data.Status==false){
                    $scope.tip=data.Msg;
                    return;
                }
            else{
                    $state.go('page.login');
                }
        })
    };

}]);

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