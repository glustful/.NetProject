/**
 * Created by 黄秀宇 on 2015/5/15.
 */
app.controller('taskconfigcontroller',['$http','$scope',function($http,$scope) {
    $scope.mainCondition={
        Id:0,
        Typename:'',
        Typedes:'',
        Tagname:'',
        Tagdes:'',
        Awardname:'',
        Awardes:'',
        Punishmentname:'',
        Punishmentdes:'',
        TaskTypeId:0,
        TaskTagId:0,
        TaskAwardId:0,
        TaskPunishmentId:0,
        describe:'',
        Type:'add',
        Status: ''
    };
    $scope.typeCondition={
        Id:0,
        Name:'',
        Describe:'',
        Type:'add',
        Status: ''
};
    $scope.tagCondition={
        Id:0,
        Name:'',
        Describe:'',
        Value:'',
        Type:'add',
        Status: ''
    };
    $scope.awardCondition={
        Id:0,
        Name:'',
        Describe:'',
        Value:'',
        Type:'add',
        Status: ''
    };
    $scope.punishmentCondition={
        Id:0,
        Name:'',
        Describe:'',
        Value:'',
        Type:'add',
        Status: ''
    };

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

    //添加任务类型

    var getTypeResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskTpye',$scope.typeCondition,{

        }).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }

        });
    };
    $scope.addType=getTypeResult;
    //添加任务目标

    var getTagResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskTag',$scope.tagCondition,{

        }).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }

        });
    };
    $scope.AddTaskTag=getTagResult;
    //添加任务奖励

    var getAwardResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskAward',$scope.awardCondition,{

        }).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }

        });
    };
    $scope.AddTaskAward=getAwardResult;
    //添加任务惩罚

    var getPunishResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskPunishment',$scope.punishmentCondition,{

        }).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }

        });
    };
    $scope.AddTaskPunishment=getPunishResult;
    //删除任务类型

    var DelTaskType  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskType/',{params:{id:$scope.mainCondition.TaskTypeId}}).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delType = DelTaskType;
    //删除任务目标

    var DelTaskTag  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskTag/',{params:{id:$scope.mainCondition.TaskTagId}}).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delTag = DelTaskTag;
    //删除任务奖励

    var DelTaskAward  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskAward/',{params:{id:$scope.mainCondition.TaskAwardId}}).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delAward = DelTaskAward;
    //删除任务惩罚

    var DelTaskPunish  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskPunishment/',{params:{id:$scope.mainCondition.TaskPunishmentId}}).success(function(data){
            if(data.Status){
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delPunish = DelTaskPunish;
}
]);
