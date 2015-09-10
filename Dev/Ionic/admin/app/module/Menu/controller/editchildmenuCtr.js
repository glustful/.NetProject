var apiurl = "http://localhost:8907/API/";
app.controller('editchildmenulistCtr', ['$scope', '$http', '$state', 'list',
	function($scope, $http, $state, list) {
		$scope.child = {
			id: list.Id,
			type: "",
			name: list.Name,
			key: list.Key,
			edittype: 'edit',
		};
		$scope.ok = function() {
			if(!$scope.child.name)
			{
				return;
			}
			if(!$scope.child.type)
			{
				return;
			}
			if ($scope.child.type == "click" || $scope.child.type == "view") {
				if(!$scope.child.key)
				{
					return;
				}
			}
			$http.post(apiurl + 'Menu/EditChildMenu', $scope.child).success(function(data) {
				var msg = data.msg;
				if (msg == 'ok') {
					window.location.reload();
				} else {
					alert(msg);
				}
			});
		}
	}
]);