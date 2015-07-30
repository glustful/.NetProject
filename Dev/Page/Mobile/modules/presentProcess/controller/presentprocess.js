

app.controller('presentProcessController',['$http','$scope','AuthService','$state',function($http,$scope,AuthService,$state) {
    var getPresentProcess  = function() {
        $http.get(SETTING.ApiUrl+'/BrokerWithdraw/GetAllBrokerWithdrawByUser/',{'withCredentials':true}).success(function(data){
            $scope.list=data.List;
        });
    };
    getPresentProcess();
}])
