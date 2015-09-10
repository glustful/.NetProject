var apiurl = "http://localhost:8907/API/";
app.controller('updatemenuCtr', ['$scope', '$http', '$state', '$modal',
	function($scope, $http, $state, $modal) {
		$scope.content = "";
		$scope.update = function() {
			$http.post(apiurl + 'Menu/CreateMenu').success(function(data) {
				if (data.errmsg == "ok") {
					$scope.content = "更新成功!";
				} else {
					$scope.content = data;
				}

			});
		};
	}
]);