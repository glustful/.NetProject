/**
 * Created by chenda on 2015/5/27.
 */
app.controller("tuijianController",['$http','$scope','$stateParams','AuthService','$state',function($http,$scope,$stateParams,AuthService,$state){
    $scope.BrokerRECClientEntity={
        AddUser:null,
        ClientInfo:null,
        Broker:null,
        Qq:'',
        Type:'',
        Brokername:'',
        Brokerlevel:'',
        Projectname:'',
        Projectid:null,
        Houses:'',
        HouseType:'',
        Clientname:'',
        Phone:'',
        Note:''
    };
    $scope.currentUser=AuthService.CurrentUser();
    $scope.BrokerRECClientEntity.AddUser = $scope.currentUser.UserId;
    $scope.BrokerRECClientEntity.Broker = $scope.currentUser.UserId;
    $scope.BrokerRECClientEntity.ClientInfo = $scope.currentUser.UserId;
    $scope.BrokerRECClientEntity.Projectid=$stateParams.Projectid;
    $scope.BrokerRECClientEntity.ProjectName=$stateParams.name;
    $scope.BrokerRECClientEntity.Houses=$stateParams.name;
    $scope.BrokerRECClientEntity.HouseType=$stateParams.type;

    var getBrokerResult  = function() {
        console.log(  $scope.BrokerRECClientEntity);
        $http.post(SETTING.ApiUrl+'/BrokerRECClient/Add', $scope.BrokerRECClientEntity).success(function(data){
            if(data.Status){
                $state.go("app.storeroom")
            }else{
              alert(data.Msg)
            }
        });
    };
    $scope.add=getBrokerResult;
}])
