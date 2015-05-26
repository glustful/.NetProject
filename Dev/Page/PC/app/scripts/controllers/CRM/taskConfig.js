/**
 * Created by 黄秀宇 on 2015/5/15.
 */

app.controller('taskconfigcontroller',['$http','$scope','$modal',function($http,$scope,$modal) {
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
        $http.get(SETTING.ApiUrl+'/Task/TaskTypeList',{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.taskType=data;
        });

    };
    getTaskType();
    //绑定任务目标
    var getTaskTag  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskTagList',{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.tasktag=data;
        });

    };
    getTaskTag();
    //绑定任务奖励
    var getTaskAward  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskAwardList',{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.taskaward=data;
        });

    };
    getTaskAward();
    //绑定任务惩罚
    var getTaskPunishment  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskPunishmentList',{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.taskpunishment=data;
        });

    };
    getTaskPunishment();

    //添加任务类型

    var getTypeResult  = function() {
        $http.post(SETTING.ApiUrl+'/Task/AddTaskTpye',$scope.typeCondition,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $scope.typeCondition.Name='';
                $scope.typeCondition.Describe='';
                getTaskType();
                $scope.warnType=data.Msg;
                 }
            else{ $scope.warnType=data.Msg;}

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
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $scope.tagCondition.Name='';
                $scope.tagCondition.Describe='';
                $scope.tagCondition.Value='';
                $scope.warnTag=data.Msg;
                getTaskTag();
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnTag=data.Msg;}

        });
    };
    var tagTest=function(){
        if($scope.tagCondition.Name==""){
            $scope.warnTag='请输入目标名称';

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
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $scope.awardCondition.Name='';
                $scope.awardCondition.Describe='';
                $scope.awardCondition.Value='';
                $scope.warnAward=data.Msg;
                getTaskAward();

                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnAward=data.Msg;}

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
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $scope.punishmentCondition.Name='';
                $scope.punishmentCondition.Describe='';
                $scope.punishmentCondition.Value='';
                getTaskPunishment();
                $scope.warnPunish=data.Msg;
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{ $scope.warnPunish=data.Msg;}

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
        $http.get(SETTING.ApiUrl+'/Task/DelTaskType/',{params:{id:$scope.mainCondition.TaskTypeId}, 'withCredentials':true}).success(function(data){
            if(data.Status){
                getTaskType();
                $scope.mainCondition.TaskTypeId=0;
                $scope.warnType=data.Msg;
               }
            else{
                $scope.warnType=data.Msg;
            }
        });
    };
    $scope.delType=function(){
        if($scope.mainCondition.TaskTypeId>0){
            openType();
        }
        else{$scope.warnType='请选择删除对象';}
    }
      var openType= function () {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
           modalInstance.result.then(DelTaskType);
    }
    //删除任务目标

    var DelTaskTag  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskTag/',{params:{id:$scope.mainCondition.TaskTagId}, 'withCredentials':true}).success(function(data){
            if(data.Status){
                getTaskTag();
                $scope.mainCondition.TaskTagId=0;
                $scope.warnTag=data.Msg;
                }
            else{
                $scope.warnTag=data.Msg;
            }
        });
    };
    $scope.delTag=function(){
        if($scope.mainCondition.TaskTagId>0){
            openopenTag();
        }
        else{$scope.warnTag='请选择删除对象';}
    }
    var openopenTag = function () {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(DelTaskTag);
    }


    //删除任务奖励

    var DelTaskAward  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskAward/',{params:{id:$scope.mainCondition.TaskAwardId}, 'withCredentials':true}).success(function(data){
            if(data.Status){
                getTaskAward();
                $scope.mainCondition.TaskAwardId=0;
                $scope.warnAward =data.Msg;
        }
            else{
                $scope.warnAward =data.Msg;
            }
        });
    };
    $scope.delAward=function(){
        if($scope.mainCondition.TaskAwardId>0){
            openAward();
        }
        else{$scope.warnAward='请选择删除对象';}
    }
   var openAward = function () {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(DelTaskAward);
    }

    //删除任务惩罚

    var DelTaskPunish  = function() {
        $http.get(SETTING.ApiUrl+'/Task/DelTaskPunishment/',{params:{id:$scope.mainCondition.TaskPunishmentId}, 'withCredentials':true}).success(function(data){
            if(data.Status){
                getTaskPunishment();
                $scope.warnPunish=data.Msg;
                $scope.mainCondition.TaskPunishmentId=0;
                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
            else{
                $scope.warnPunish=data.Msg;
            }
        });
    };
    $scope.delPunish=function(){
        if($scope.mainCondition.TaskPunishmentId>0){
            penPunish();
        }
        else{$scope.warnPunish='请选择删除对象';}
    }
   var penPunish= function () {
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(DelTaskPunish);
    }

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