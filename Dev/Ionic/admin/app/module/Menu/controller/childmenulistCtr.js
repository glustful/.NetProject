var apiurl = "http://localhost:8907/API/";
app.controller('childmenulistCtr', ['$scope', '$http', '$state', '$modal',
	function($scope, $http, $state, $modal) {
		var urlinfo = window.location.href;
		var len = urlinfo.length;
		var phone = urlinfo.indexOf("?");
		var phoneinfo = urlinfo.substr(phone, len);
		var phoneids = phoneinfo.split("=");
		var fatherid = phoneids[1];
		if (!fatherid) {
			return;
		} else {
			$http.post(apiurl + 'Menu/ChildMenuList', fatherid).success(function(response) {
				$scope.childmenulist = response.list;
				$scope.fatherName = response.fname;
				$scope.fatherId = response.fid;
			});
			//添加子菜单
			$scope.addchildmenu = function() {
				var modalInstance = $modal.open({
					templateUrl: 'addchildmenu.html',
					controller: 'addchildmenulistCtr',
					resolve: {
						list: function() {
							return fatherid;
						}
					}
				});
			};
			//编辑
			$scope.edit = function(json) {
				var modalInstance = $modal.open({
					templateUrl: 'editchildmenu.html',
					controller: 'editchildmenulistCtr',
					resolve: {
						list: function() {
							return json;
						}
					}
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
							alert("请先删除子菜单");
						}
					});
				}, function(selectedItem) {
					//取消

				});
			};
		}
	}
]);