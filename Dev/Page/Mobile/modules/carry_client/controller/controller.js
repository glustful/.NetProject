/**
 * Created by chenda on 2015/5/27.
 */
app.controller('daikeController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.BrokerLeadClient={
        Appointmenttime:'2012',
        Broker_Id:1,
        Client_Id:1,
        Brokername:'xingchen'

    };
    var getBrokerResult  = function() {
        $http.post(SETTING.ApiUrl+'/BrokerLeadClient/Add',$scope.BrokerLeadClient).success(function(data){
            $scope.BrokerLeadClient.AppointmentTime=0;
            $scope.BrokerLeadClient.Broker_Id=1;
            //$scope.Brokername='xingchen';
            //$scope.BrokerLeadClient.Broker_Id=$stateParams.Broker_Id;
            //$scope.Brokername=$stateParams.Brokername;
        });
    };
    $scope.add=getBrokerResult;
}])