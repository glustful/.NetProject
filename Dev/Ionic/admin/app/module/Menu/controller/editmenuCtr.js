var apiurl = "http://localhost:8907/API/";
app.controller('editmenuCtr', ['$scope', 'list', '$http', '$state',
	function($scope, list, $http, $state) {
		$scope.info = {
			id: list.Id,
			type: "",
			name: list.Name,
			key: list.Key,
			edittype: 'edit',
		};
		$scope.ok = function() {
			if(!$scope.info.name)
			{
				return;
			}
			if(!$scope.info.type)
			{
				return;
			}
			if ($scope.info.type == "click" || $scope.info.type == "view") {
				if(!$scope.info.key)
				{
					return;
				}
			}
			//设置为编辑
			//$scope.info.edittype = 'edit';
			$http.post(apiurl + 'Menu/EditMenu', $scope.info).success(function(data) {
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