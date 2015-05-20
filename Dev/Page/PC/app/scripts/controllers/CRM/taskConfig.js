/**
 * Created by 黄秀宇 on 2015/5/15.
 */

app.controller('taskconfigcontroller',['$http','$scope',function($http,$scope) {
    $scope.mainCondition={
        Id:0,

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
        Status: '',
        warm:''
    };
    $scope.punishmentCondition={
        Id:0,
        Name:'',
        Describe:'',
        Value:'',
        Type:'add',
        Status: ''
    };
$scope.warnType='';
    $scope.warnTag='';
    $scope.warnAward='';
    $scope.warnPunish='';
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
                $scope.typeCondition.Name='';
                $scope.typeCondition.Describe='';
                getTaskType();
                $scope.warnType='添加成功';
                 }
            else{ $scope.warnType='类型重复，请更换';}

        });
    };
    var typeTest=function(){
        if($scope.typeCondition.Name==""){
            $scope.warnType='请输入类型名称';
        }
        else if($scope.typeCondition.Describe==""){
            $scope.warnType='请输入描述';
        }
        else{getTypeResult(); }
    }
    $scope.addType=typeTest;
    //添加任务目标

    var getTagResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskTag',$scope.tagCondition,{

        }).success(function(data){
            if(data.Status){
                $scope.tagCondition.Name='';
                $scope.tagCondition.Describe='';
                $scope.tagCondition.Value='';
                $scope.warnTag='添加成功';
                getTaskTag();
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnTag='目标名称重复，请更换';}

        });
    };
    var tagTest=function(){
        if($scope.tagCondition.Name==""){
            $scope.warnTag='请输入奖励名称';

        }
        else if($scope.tagCondition.Describe==""){

            $scope.warnTag='请输入描述';
        }
        else if($scope.tagCondition.Value==""){

            $scope.warnTag='请输入目标值';
        }
        else{getTagResult(); }
    }

    $scope.AddTaskTag=tagTest;
    //添加任务奖励

    var getAwardResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskAward',$scope.awardCondition,{

        }).success(function(data){
            if(data.Status){
                $scope.awardCondition.Name='';
                $scope.awardCondition.Describe='';
                $scope.awardCondition.Value='';
                $scope.warnAward='添加成功';
                getTaskAward();

                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnAward='奖励重复，请更换';}

        });
    };
    var awardTest=function(){
        if($scope.awardCondition.Name==""){
            $scope.warnAward='请输入奖励名称';

        }
        else if($scope.awardCondition.Describe==""){

            $scope.warnAward='请输入描述';
        }
        else if($scope.awardCondition.Value==""){

            $scope.warnAward='请输入目标值';
        }
        else{getAwardResult(); }
    }
    $scope.AddTaskAward=awardTest;
    //添加任务惩罚

    var getPunishResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskPunishment',$scope.punishmentCondition,{

        }).success(function(data){
            if(data.Status){
                $scope.punishmentCondition.Name='';
                $scope.punishmentCondition.Describe='';
                $scope.punishmentCondition.Value='';
                getTaskPunishment();
                $scope.warnPunish='添加成功';
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnPunish='惩罚重复，请更换';}

        });
    };
    var punishTest=function(){
        if($scope.punishmentCondition.Name==""){
            $scope.warnPunish='请输入惩罚名称';

        }
        else if($scope.punishmentCondition.Describe==""){

            $scope.warnPunish='请输入描述';
        }
        else if($scope.punishmentCondition.Value==""){

            $scope.warnPunish='请输入目标值';
        }
        else{getPunishResult(); }
    }
    $scope.AddTaskPunishment=punishTest;

    //删除任务类型

    var DelTaskType  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskType/',{params:{id:$scope.mainCondition.TaskTypeId}}).success(function(data){
            if(data.Status){
                getTaskType();
                $scope.warnType='删除成功';
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{
                $scope.warnType='该任务类型使用中，删除失败';
            }
        });
    };
    var delTyleTest=function(){
        if($scope.mainCondition.TaskTypeId>0){
            DelTaskType();
        }
    }
    $scope.delType =delTyleTest ;
    //删除任务目标

    var DelTaskTag  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskTag/',{params:{id:$scope.mainCondition.TaskTagId}}).success(function(data){
            if(data.Status){
                getTaskTag();
                $scope.warnTag='删除成功';
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{
                $scope.warnTag='该任务目标使用中，删除失败';
            }
        });
    };
    var delTagTest=function(){
        if($scope.mainCondition.TaskTagId>0){
            DelTaskTag();
        }
    }
    $scope.delTag =delTagTest ;

    //删除任务奖励

    var DelTaskAward  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskAward/',{params:{id:$scope.mainCondition.TaskAwardId}}).success(function(data){
            if(data.Status){
                getTaskAward();
                $scope.warnAward ='删除成功';

                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{
                $scope.warnAward ='该任务奖励使用中，删除失败';
            }
        });
    };
    var delAwardTest=function(){
        if($scope.mainCondition.TaskAwardId>0){
            DelTaskAward();
        }
    }
    $scope.delAward =delAwardTest ;
    //删除任务惩罚

    var DelTaskPunish  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskPunishment/',{params:{id:$scope.mainCondition.TaskPunishmentId}}).success(function(data){
            if(data.Status){
                getTaskPunishment();
                $scope.warnPunish='删除成功';
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{
                $scope.warnPunish='该任务惩罚使用中，删除失败';
            }
        });
    };
    var delPunishTest=function(){
        if($scope.mainCondition.TaskPunishmentId>0){
            DelTaskPunish();
        }
    }
    $scope.delPunish = delPunishTest;
    //删除提示
    var delW=function(){
        $scope.warnAward ='';
        $scope.warnPunish ='';
        $scope.warnTag ='';
        $scope.warnType ='';
    }
    $scope.delWarn=delW;
}
]);
