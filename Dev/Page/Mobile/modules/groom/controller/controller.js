/**
 * Created by chenda on 2015/5/27.
 */
app.controller("tuijianController",['$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService){
    $scope.BrokerRECClientEntity={
        UserId:null,
        ClientInfo:null,
        Qq:'',
        Type:'',
        Brokername:'',
        Brokerlevel:'',
        ProjectName:'',
        ProjectId:null,
        Houses:'',
        HouseType:'',
        Clientname:'',
        Phone:'',
        Note:''
    };
    $scope.currentUser=AuthService.CurrentUser();
    $scope.BrokerRECClientEntity.UserId = $scope.currentUser.UserId;
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