/**
 * Created by chenda on 2015/5/27.
 */
app.controller("tuijianController",['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $scope.BrokerRECClientEntity={
        ClientInfo:'1',
        Qq:'4444',
        Type:'0',
        Brokername:'',
        Brokerlevel:'',
        ProjectName:'',
        Projectid:'2',
        Houses:'JSJY',
        HouseType:'SSYT',
        Clientname:'nike',
        Phone:13888888888,
        Note:'hello'
    };
    //$scope.BrokerRECClient.Broker_Id=$stateParams.Broker_Id;
    //$scope.BrokerRECClient.Brokername=$stateParams.Brokername;
    //$scope.BrokerRECClient.Projectid=$stateParams.Projectid;
    //$scope.BrokerRECClient.Projectname=$stateParams.Projectname;
    var getBrokerResult  = function() {
        console.log(  $scope.BrokerRECClientEntity);
        $http.post(SETTING.ApiUrl+'/BrokerRECClient/Add', $scope.BrokerRECClientEntity).success(function(data){
            if(data.Status){
                alert(data.Msg)
            }
        });
    };
    $scope.add=getBrokerResult;
}])
