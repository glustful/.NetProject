
/**
 * Created by lhl on 2015/5/28.
 */
app.controller('recommendBrokerController',function($scope,$http){

    $scope.newuser = {
        userId:2
    }
    $http.get(SETTING.ApiUrl+'/RecommendAgent/GetRecommendAgentListByUserId',{params: $scope.newuser}).success(function(response) {
            $scope.List = response;
        });


})


app.controller('recommendBrokerAddController',['$http','$scope','$state',function($http,$scope,$state){

    $scope.Crete = function()
    {
        $http.post(SETTING.ApiUrl+'/RecommendAgent/SendRecommendAgentSms', $scope.phone, {'withCredentials': true}).success(function(datas) {
           alert(datas.toString());
     });
    };

}]);

