/**
 * Created by 黄秀宇 on 2015/5/12.
 */
app.controller('taskIndexController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        Taskname: '',
        Tasktype: '',
        Id:0
    };
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:{Taskname:($scope.searchCondition.Taskname)}}).success(function(data){
           console.log(data);
            $scope.list = data.list;
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
    //删除任务

   var DelTask = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTask/',{params:{id:$scope.searchCondition.Id}}).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delTask = DelTask;
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
            $scope.taskModel = data.list;
        });
    };

    getTaskList1();
    var getTaskListSer  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1}).success(function(data){

            console.log(data);
            $scope.taskModel = data.list;
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


