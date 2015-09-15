app.controller('AreaCreateCtr', ['$http', '$scope', '$state', function ($http, $scope, $state) {
    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?id=" + $scope.id, {
        'withCredentials': true
    }).success(function (data) {
        $scope.list = data;
    })
    $scope.area = {
        Codeid: '',
        Name: '',
        Parentid:''
    }
    $scope.save = function () {
        $http.post(SETTING.ZergWcApiUrl + "/CommunityArea/Post", $scope.area, {
            'withCredentials': true
        }).success(function (data) {
            if (data.Status) {
                $state.go("app.area.show")
            }
        })
    }
}])