/**
 * Created by chenda on 2015/5/27.
 */
app.controller("tuijianController",['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $scope.BrokerRECClientEntity={
        ClientInfo:'1',
        Qq:'',
        Type:'',
        Brokername:'',
        Brokerlevel:'',
        ProjectName:'',
        ProjectId:1,
        Houses:'',
        HouseType:'',
        Clientname:'',
        Phone:'',
        Note:''
    };
    //$scope.BrokerRECClient.Broker_Id=$stateParams.Broker_Id;
    $scope.BrokerRECClient.ProjectId=$stateParams.projectid;
    $scope.BrokerRECClientEntity.Houses=$stateParams.name;
    $scope.BrokerRECClientEntity.HouseType=$stateParams.type;
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
