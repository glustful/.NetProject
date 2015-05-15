/**
 * Created by 黄秀宇 on 2015/5/12.
 */
app.controller('taskIndexController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        Taskname: '',
        Tasktype: ''
    };
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:{taskname:$scope.searchCondition.Taskname}}).success(function(data){
           console.log(data);
            $scope.list = data;
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
   }
]);
app.controller('taskListcontroller',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.searchCondition1 = {
        Taskschedule:'',
        id:$stateParams.id
    };

    var getTaskList1  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListBytaskId?id='+$stateParams.id).success(function(data){

            console.log(data);
            $scope.taskModel = data;
        });
    };

    getTaskList1();
    var getTaskListSer  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1}).success(function(data){

            console.log(data);
            $scope.taskModel = data;
        });
    };
    $scope.gettasklist=getTaskListSer;
}
]);

app.controller('taskDetailcontroller',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {

    var getTaskDetail  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskDetail?id='+$stateParams.id).success(function(data){

            console.log(data);
            $scope.taskModel1 = data;
        });
    };
    $scope.gettaskdetail=getTaskDetail();
    getTaskDetail();
}
]);


