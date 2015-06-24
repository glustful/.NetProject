/**
 * Created by chenda on 2015/5/27.
 */
app.controller('daikeController',['$http','$scope','$stateParams','AuthService','$state',function($http,$scope,$stateParams,AuthService,$state) {
    $scope.BrokerLeadClient={
        AddUser:null,
        Broker:null,
        Projectname:'',
        Projectid:null,
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
    $scope.BrokerLeadClient.AddUser = $scope.currentUser.UserId;
    $scope.BrokerLeadClient.Projectid=$stateParams.Projectid;
    $scope.BrokerLeadClient.Projectname=$stateParams.name;
    //$scope.BrokerLeadClient.Broker= $scope.currentUser.UserId;
    $scope.BrokerLeadClient.Houses=$stateParams.name;
    $scope.BrokerLeadClient.HouseType=$stateParams.type;
    var getBrokerResult  = function() {
        console.log(  $scope.BrokerLeadClient);
        $http.post(SETTING.ApiUrl+'/BrokerLeadClient/Add',$scope.BrokerLeadClient).success(function(data){
            if(data.Status){
                $state.go("app.nominate")
            }else{
                alert(data.Msg)
            }
        });
    };
    $scope.add=getBrokerResult;

}])