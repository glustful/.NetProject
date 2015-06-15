/**
 * Created by gaofengming on 2015/6/12.
 */

app.controller('invitecontroller',['$scope','$state','$http',function($scope,$state,$http){
    $scope.invitecod='';
    $scope.invite= function(){

        $state.go('user.register',{yqm: $scope.invitecod});
        //$http.post(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByInvitationCode',$scope.invitecod,{'withCredentials':true}).success(function(data){
        //  if(data.invitationCode!=undefined && data.invitationCode!="")
        //  {
        //
        //     $state.go('user.register',{yzm:data.invitationCode});
        //  }else{
        //
        //      alert("该邀请码不存在");
        //
        //  }
        //
        //})
        //
        //
    }
}])