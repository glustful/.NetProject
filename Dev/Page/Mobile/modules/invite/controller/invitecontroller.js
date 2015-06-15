/**
 * Created by gaofengming on 2015/6/12.
 */

app.controller('invitecontroller',['$scope','$state','$http',function($scope,$state,$http){
    $scope.invitecod='';
    $scope.invite= function(){
        $state.go('user.register',{yzm:'3434341'});
     //   window.href='/user/register?yzm='+  $scope.invitecod;
//
//        $http.post(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByInvitationCode',$scope.invitecod,{'withCredentials':true}).success(function(data){
//          if(data.invitationuserid!=undefined && data.invitationuserid!="")
//          {
//              $state.go('user.register');
//          }else{
//              window.href='/user/register?yzm=1111';
//           //   $state.go('user.register?yzm=121212');
//          }
//
//        })
//
//


    }
}])