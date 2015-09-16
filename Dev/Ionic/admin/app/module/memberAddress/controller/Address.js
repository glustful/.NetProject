app.controller('ShowInfo', ['$scope', '$http', '$modal', function ($scope, $http, $modal) {
    $scope.sech = {
        Page: 1,
        PageCount: 10
    }

    var getAddressList = function () {
        $http.get(SETTING.ZergWcApiUrl + "/MemberAddress/Get", {
            params: $scope.sech,
            'withCredentials': true  //跨域'
        }).success(function (data) {
            $scope.list = data.List;
            $scope.sech = data.Condition;
            $scope.totalCount = data.TotalCount;
        });
    }

    $scope.seach = function () {
        $http.get(SETTING.ZergWcApiUrl + "/MemberAddress/Get?UserName=" + $scope.Condition, {
            params: $scope.sech,
            'withCredentials': true  //跨域'
        }).success(function (data) {
            $scope.list = data.List;
            $scope.sech = data.Condition;
            $scope.totalCount = data.TotalCount;
        });
    }
    getAddressList();
    $scope.getlist = getAddressList;
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
            $http.delete(SETTING.ZergWcApiUrl + '/MemberAddress/Delete', {
                params: {
                    id: $scope.selectedId
                },
                'withCredentials': true
            }
            ).success(function (data) {
                if (data.Status) {
                    getAddressList();
                }
                else {
                    //$scope.Message=data.Msg;
                    $scope.alerts = [{ type: 'danger', msg: data.Msg }];
                }
            });
        });
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }
    }
    ]);