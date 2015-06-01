/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('personsettingController',function($scope,$http){
    $scope.user = {
        userType: 122,
        name: "ggg",
        phone: 2445254,
        page: 1,
        pageSize: 10
    }
    $scope.newuser = {
        Id:11,
        Brokername: "afaf",
        phone: 525424,
        Sfz:1234567891,
        page: 1,
        pageSize: 10
    }
    $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{params: $scope.user})
        .success(function(response) {$scope.users = response.List[0];

        });
    $scope.save = function()
    {
        $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.newuser)
            .success(function(data) {
            });
    }
})