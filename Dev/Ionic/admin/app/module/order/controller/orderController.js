/**
 * Created by Yunjoy on 2015/9/10.
 */
app.controller('orderListController', ['$scope', 'repository', function ($scope, repository) {
    $scope.condition = {
        Page: 1,
        PageCount: 10,
        IsDescending: false,
        Ids: [],
        No: '',
        Status: '',
        CustomerName: '',
        AdddateBegin: null,
        AdddateEnd: null,
        Addusers: null,
        Updusers: null,
        UpddateBegin: null,
        UpddateEnd: null,
        TotalpriceBegin: null,
        TotalpriceEnd: null,
        ActualpriceBegin: null,
        ActualpriceEnd: null,
        OrderBy: ''
    };

    var getList = function () {
        repository.get("communityOrder", $scope.condition).then(function (data) {
            $scope.list = data.List;
            $scope.condition = data.Condition;
            $scope.totalPages = data.TotalCount;
        });
    };

    getList();
    $scope.getList = getList;
}]);