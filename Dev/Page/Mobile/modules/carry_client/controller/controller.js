/**
 * Created by chenda on 2015/5/27.
 */
app.controller('daikeController',['$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService) {
    $scope.BrokerLeadClient={
        Id:null,
        Broker:null,
        Projectname:'',
        ProjectId:null,
        Brokername:'',
        Appointmenttime:'',
        Houses:'',
        HouseType:'',
        Clientname:'',
        Phone:'',
        Note:'',
        Stats:'0'

    };
    $scope.currentUser=AuthService.CurrentUser();
    $scope.BrokerLeadClient.Id = $scope.currentUser.UserId;
    $scope.BrokerLeadClient.ProjectId=$stateParams.Projectid;
    $scope.BrokerLeadClient.Houses=$stateParams.name;
    $scope.BrokerLeadClient.HouseType=$stateParams.type;
    var getBrokerResult  = function() {
        console.log(  $scope.BrokerLeadClient);
        $http.post(SETTING.ApiUrl+'/BrokerLeadClient/Add',$scope.BrokerLeadClient).success(function(data){
            if(data.Status){
                alert(data.Msg)
            }
        });
    };
    $scope.add=getBrokerResult;
}])