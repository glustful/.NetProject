app.controller('ShowInfo', ['$scope', '$http', '$modal', function ($scope, $http, $modal) {
    $scope.sech = {
        Page: 1,
        PageCount: 10
    }

    var getAreaList = function () {
        $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get", {
            params: $scope.sech,
            'withCredentials': true  //跨域'
        }).success(function (data) {
            $scope.list = data.List;
            $scope.sech = data.Condition;
            $scope.totalCount = data.TotalCount;
        });
    }
    getAreaList();
    $scope.getlist = getAreaList;
    $scope.del = function (id) {
        $scope.selectedId = id;
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () { return "你确定要删除吗？"; }
            }
        });
        modalInstance.result.then(function () {
            $http.delete(SETTING.ZergWcApiUrl + '/CommunityArea/Delete', {
                params: {
                    id: $scope.selectedId
                },
                'withCredentials': true
            }
            ).success(function (data) {
                if (data.Status) {
                    getAreaList();
                    $scope.state = "数据删除成功！";
                }
                else {
                    //$scope.Message=data.Msg;
                    $scope.state = "数据删除失败！";
                }
            });
        });
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }
}
]);