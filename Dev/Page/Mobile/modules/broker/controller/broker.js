/** create by 杨波 2015.6.5 创富英雄榜**/
app.controller('BrokerTopThreeController',['$scope','$http',function($scope,$http){
    var BrokerTopThree=function() {
        $http.get(SETTING.ApiUrl + '/BrokerInfo/OrderByBrokerTopThree', {'withCredentials': true}).success(function (data) {
//           $scope.ii=0;
//           for(var i=0;i<data.List.length-1;i++)
//            {
//                $scope.list.push(data.List[i]);
//                $scope.ii=i+1;
//            }
//           console.log($scope.li);
           $scope.list = data.List;
            console.log($scope.list);
        })
    };
    BrokerTopThree();
}]);