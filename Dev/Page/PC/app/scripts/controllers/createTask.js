app.controller('taskaddcontroller',['$http','$scope',function($http,$scope) {
    $scope.addcondition={

        Id:0,
        Taskname:'',
        Endtime:'',
        TaskTypeId:0,
        TaskTagId:0,
        TaskAwardId:0,
        TaskPunishmentId:0,
        describe:'',
        Type:'add',
        Status: ''
    };
    //发布任务

    var getTaskResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTask',$scope.addcondition,{
            //'withCredentials':true
        }).success(function(data){
           if(data.Status){
              // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }

        });
    };
    $scope.gettaskRe=getTaskResult;
    //绑定任务类型
    var getTaskType  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskTypeList').success(function(data){
            console.log(data);
            $scope.taskType=data;
        });

    };
    getTaskType();
    //绑定任务目标
    var getTaskTag  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskTagList').success(function(data){
            console.log(data);
            $scope.tasktag=data;
        });

    };
    getTaskTag();
    //绑定任务奖励
    var getTaskAward  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskAwardList').success(function(data){
            console.log(data);
            $scope.taskaward=data;
        });

    };
    getTaskAward();
    //绑定任务惩罚
    var getTaskPunishment  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskPunishmentList').success(function(data){
            console.log(data);
            $scope.taskpunishment=data;
        });

    };
    getTaskPunishment();
}
]);