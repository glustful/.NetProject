app.controller('AreaCreateCtr', ['$http', '$scope', '$state', function ($http, $scope, $state) {
    //$http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?id=" + $scope.id, {
    //    'withCredentials': true
    //}).success(function (data) {
    //    $scope.list = data;
    //})
    $scope.text_change = function () {
        $scope.isShow3 = false;
        //$scope.data.area = "";
    }

    $scope.show3 = function () {
        $scope.isShow3 = true;
    }

    $scope.isShow2 = true;
    $scope.isShow3 = true;

    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get",{
        'withCredentials': true
    }).success(function (data) {
        $scope.cities = data.List;
    })

    $scope.area = {
        Codeid: '',
        Name: '',
        Parent: {Id:0}
    }
    $scope.save = function () {
        $scope.area.Parent.Id = $scope.data.city != null ? Parentid = $scope.data.city : $scope.data.area != null ? Parentid = $scope.data.area : $scope.data.province != null ? Parentid = $scope.data.province : null;
        //if ($scope.data.province != null || $scope.data.province != null && $scope.data.area != null || $scope.data.province != null && $scope.data.area != null && $scope.data.city != null) {
            $http.post(SETTING.ZergWcApiUrl + "/CommunityArea/Post", $scope.area, {
                'withCredentials': true
            }).success(function (data) {
                if (data.Status) {
                    //$state.go("app.area.show")
                }
            })
        }//else
        //{
        //    $scope.state = "请选择归属城市！";
        //}
    }
])