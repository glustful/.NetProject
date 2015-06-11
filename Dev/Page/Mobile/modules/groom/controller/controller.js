/**
 * Created by chenda on 2015/5/27.
 */
app.controller("tuijianController",['$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService){
    $scope.BrokerRECClientEntity={
        AddUser:null,
        ClientInfo:null,
        Qq:'',
        Type:'',
        Brokername:'',
        Brokerlevel:'',
        ProjectName:'',
        Projectid:null,
        Houses:'',
        HouseType:'',
        Clientname:'',
        Phone:'',
        Note:''
    };
    $scope.currentUser=AuthService.CurrentUser();
    $scope.BrokerRECClientEntity.AddUser = $scope.currentUser.UserId;
    $scope.BrokerRECClientEntity.Projectid=$stateParams.Projectid;
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
