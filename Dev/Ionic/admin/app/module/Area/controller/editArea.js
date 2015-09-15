app.controller('AreaEditCtr', ['$http', '$scope', '$state', '$stateParams',function ($http, $scope, $state,$stateParams) {
    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?id=" + $stateParams.id, {
        'withCredentials': true  //跨域
    }).success(function (data) {
        $scope.list = data;
    })

    $scope.Create=function(){
        $http.put(SETTING.ZergWcApiUrl + '/CommunityArea/Put', $scope.list, {
            'withCredentials': true
        }).success(function (data) {
            if (data.Status) {
                $state.go("app.area.show");
            }
        })}
    }
])