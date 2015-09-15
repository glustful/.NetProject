var apiurl = "http://localhost:8907/API/";
app.controller('addchildmenulistCtr', ['$scope', '$http', '$state', 'list',
	function($scope, $http, $state, list) {
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
				fatherid:list
			};
			$http.post(apiurl + 'Menu/EditChildMenu', $scope.add).success(function(data) {
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