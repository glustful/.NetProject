/** create by 杨波 2015.6.5 创富英雄榜**/
app.controller('BrokerTopThreeController',['$scope','$http',function($scope,$http){
    var BrokerTopThree=function() {
        $http.get(SETTING.ApiUrl + '/BrokerInfo/OrderByBrokerTopThree', {'withCredentials': true}).success(function (data) {
            $scope.list = data.List;
            console.log($scope.list);
        })
    };
    BrokerTopThree();
}]);