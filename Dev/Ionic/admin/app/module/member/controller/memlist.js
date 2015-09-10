/*

 */
app.controller('memController', ['$scope', '$http',
    function($scope, $http) {
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };

            $http.get(SETTING.ZergWcApiUrl+"/Member/Get",{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){

                $scope.list=data.List;

            });
        $http.get(SETTING.ZergWcApiUrl+"/Member/Get",{
            params:$scope.searchCondition,
            'withCredentials':true
        }).success(function(data){

            $scope.list=data.List;

        });
    }]);