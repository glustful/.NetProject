app.controller("heroController",function ($scope,$http) {
	$http.get(SETTING.ApiUrl+'/BrokerInfo/OrderByBrokerList')
    .success(function(response) {$scope.item = response.List; 
    	$scope.com= $scope.item.slice(0);
    	for (i=0;i<3;i++) {
    		$scope.com.shift();
 
    	}
    });
});