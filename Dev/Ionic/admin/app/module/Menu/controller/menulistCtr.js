var apiurl = "http://localhost:8907/API/";
app.controller('menulistCtr', ['$scope', '$http', '$state', '$modal',
	function($scope, $http, $state, $modal) {
		var pot=1520;
		$http.post(apiurl + 'Menu/MenuList',pot).success(function(data) {
			$scope.menulist = data.List;
			console.log(data.List);
		});
		//编辑
		$scope.edit = function(json) {
			var modalInstance = $modal.open({
				templateUrl: 'editmenu.html',
				controller: 'editmenuCtr',
				resolve: {
					list: function() {
						return json;
					}
				}
			});
		};
		//新增
		$scope.add = function() {
			var modalInstance = $modal.open({
				templateUrl: 'addmenu.html',
				controller: 'addmenuCtr',
			});
		};
		//删除
		$scope.del = function(id, name) {
			$scope.statusModal = {
				title: "提示",
				warning: "是否删除名为    " + name + "  的菜单!",
				content: "删除该菜单必须先清空子菜单",
				close: "取消",
				ok: "确认删除"
			};
			var modalInstance = $modal.open({
				templateUrl: 'app/common/layout/statusModal/view/statusModal.html',
				controller: 'ModalInstanceCtrl',
				resolve: {
					statusModal: function() {
						return $scope.statusModal;
					}
				}
			});
			modalInstance.result.then(function(selectedItem) {
				//确认
				$http.post(apiurl + 'Menu/DelMenu', id).success(function(data) {
					if (data.msg == 'ok') {
						window.location.reload();
					} else {
						///請先刪除子菜單
							$scope.statusModal = {
								title: "提示",
								warning: "请先删除    " + name + "  的子菜单!",
								close: "取消",
								ok: "确认"
							};
							var modalInstance = $modal.open({
								templateUrl: 'app/common/layout/statusModal/view/statusModal.html',
								controller: 'ModalInstanceCtrl',
								resolve: {
									statusModal: function() {
										return $scope.statusModal;
									}
								}
							});
							modalInstance.result.then(function(selectedItem) {
								//确认

							}, function(selectedItem) {
								//取消

							});
					}
				});
			}, function(selectedItem) {
				//取消

			});
		};
	}
]);