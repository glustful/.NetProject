/** create 杨波 2015.5.26 **/
//查询个人信息
app.controller('partnerDetailedController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.getCondition = {
        Id:0
    };

    var getPersonalInformation=function(){

        $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBroker?Id='+$stateParams.Id,{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list=data;
        })
    };
    getPersonalInformation();
}
]);