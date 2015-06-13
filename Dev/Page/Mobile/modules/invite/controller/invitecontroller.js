/**
 * Created by gaofengming on 2015/6/12.
 */

app.controller('invitecontroller',['$scope','$state','$http',function($scope,$state,$http){
    $scope.invitecod='';
    $scope.invite= function(){
        $http.post(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByInvitationCode',$scope.invitecod,{'withCredentials':true}).success(function(data){
          if(data.invitationuserid!=undefined && data.invitationuserid!="")
            $state.go('user.register');
        })
    }
}])