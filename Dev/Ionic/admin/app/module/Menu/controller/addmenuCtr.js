var apiurl = "http://localhost:8907/API/";
app.controller('addmenuCtr', ['$scope', '$http', '$state',
	function($scope, $http, $state) {
		$scope.add = function() {
			var name = $("#name").val();
			var value = $("#value").val();
			var type = $("#type").val();
			if(!name)
			{
				return;
			}
			if(!type)
			{
				return;
			}
			if (type == "click" || type == "view") {
				if(!value)
				{
					return;
				}
			}
			$scope.add = {
				type: type,
				name: name,
				key: value,
				edittype: 'add',
				portalId:1520,
			};
			if(!value)
				{
					$scope.add.key="http://www.iyookee.cn";
				}
			$http.post(apiurl + 'Menu/EditMenu', $scope.add).success(function(data) {
				var msg = data.msg;
				if (msg == 'ok') {
					window.location.reload();
				} else {
					alert(msg);
				}
			});
		};
	}
]);