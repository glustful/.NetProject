/**
 * Created by Yunjoy on 2015/9/10.
 */
app.controller('serviceOrderListController', ['$scope', 'repository', function ($scope, repository) {
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
        repository.get("serviceOrder", $scope.condition).success(function (data) {
            $scope.list = data.List;
            $scope.condition = data.Condition;
            $scope.totalPages = data.TotalPages;
        });
    };

    getList();
    $scope.getList = getList;
}]);