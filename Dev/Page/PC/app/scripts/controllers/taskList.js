/**
 * Created by 黄秀宇 on 2015/5/12.
 */
angular.module('app').controller('taskIndexController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        Taskname: '',
        Tasktype: ''
    };
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList',{params:{taskname:$scope.searchCondition.Taskname,
            tasktype:$scope.searchCondition.Tasktype}}).success(function(data){
           console.log(data);
            $scope.list = data;
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
   }
]);
