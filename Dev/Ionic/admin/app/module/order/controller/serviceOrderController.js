/**
 * Created by Yunjoy on 2015/9/10.
 */
app.controller('serviceOrderListController', ['$http','$scope', 'repository', function ($http,$scope, repository) {
    $scope.condition = {
        Page:1,
        PageCount:10,
        IsDescending:false,
        Ids:[],
        OrderNo:'',
        AddtimeBegin:null,
        AddtimeEnd:null,
        AddUsers:null,
        FleeBegin:null,
        FleeEnd:null,
        ServicetimeBegin:null,
        ServicetimeEnd:null,
        Remark:'',
        OrderBy:''
    };

    var getList = function () {
        repository.get("serviceOrder", $scope.condition).then(function (data) {
            $scope.list = data.List;
            $scope.condition = data.Condition;
            $scope.totalPages = data.TotalPages;
        });
    }
    getList();
    $scope.getList = getList;
    $scope.Create = function (item) {
        $http.put(SETTING.ZergWcApiUrl + '/ServiceOrder/Put', item, {
            'withCredentials': true
        }).success(function (data) {
            getList();
        })
    }
}]);