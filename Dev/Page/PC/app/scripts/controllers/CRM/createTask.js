app.controller('taskaddcontroller',['$http','$scope','$stateParams','$modal',function($http,$scope,$stateParams,$modal) {
    $scope.addcondition={

        Id:0,
        Taskname:'',
        Endtime:'',
        TaskTypeId:0,
        TaskTagId:0,
        TaskAwardId:0,
        TaskPunishmentId:0,
        describe:'',
        TaskPunishment:'',
        TaskType:'',
        awardName:'',
        Type:'edit',
        Status: '',
        warm:''
    };
    $scope.addcondition.warm="带有*为必填项";
    //绑定任务
    if($stateParams.id){

    var getTaskDetail  = function() {
      //  $http.get(SETTING.ApiUrl+'/Task/TaskDetail'+{params:{Id:$stateParams.id},'withCredentials':true}).success(function(data){
            $http.get(SETTING.ApiUrl+'/Task/TaskDetail',{params:{id:$stateParams.id}, 'withCredentials':true}).success(function(data){

            $scope.taskModel1 = data;
            $scope.addcondition.Taskname=data.Taskname;
            $scope.addcondition.tagName=$scope.taskModel1.tagName;
            $scope.addcondition.Endtime=$scope.taskModel1.Endtime;
            $scope.addcondition.awardName=$scope.taskModel1.awardName;
            $scope.addcondition.TaskType=$scope.taskModel1.TaskType;
            $scope.addcondition.TaskPunishment=$scope.taskModel1.TaskPunishment;
            $scope.addcondition.Describe=$scope.taskModel1.Describe;
            $scope.addcondition.TaskAwardId=$scope.taskModel1.TaskAwardId;
            $scope.addcondition.TaskPunishmentId=$scope.taskModel1.TaskPunishmentId;
            $scope.addcondition.TaskTypeId=$scope.taskModel1.TaskTypeId;
            $scope.addcondition.TaskTagId=$scope.taskModel1.TaskTagId;
            $scope.addcondition.Id=$stateParams.id;
        });
    };
    $scope.gettaskdetail=getTaskDetail();
    getTaskDetail();
    }
    //修改任务

    var UpdateTaskResult  = function() {

        $http.post(SETTING.ApiUrl+'/Task/AddTask',$scope.addcondition,{'withCredentials':true
        }).success(function(data){

            if(data.Status){

                var modalInstanc = $modal.open({
                    templateUrl: 'myModalContent.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        msg: function () {
                            return data.Msg;
                        }
                    }
                });           }
            else{ var modalInstanc = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return data.Msg;
                    }
                }
            });}

        });
    };
$scope.Uptask=UpdateTaskResult;
    //发布任务
var dd=function (){
    if($scope.addcondition.Taskname==''){
        $scope.addcondition.warm="任务名称不能为空";
        $scope.delTipsName=dTips;
        $scope.delTipsTime="";
        $scope.delTipsType="";
        $scope.delTipsTag="";
        $scope.delTipsAward="";
        $scope.delTipsPunish="";
        document.getElementById("taskname").focus();
    }
   else if($scope.addcondition.Endtime ==""){
        $scope.addcondition.warm="结束时间不能为空";
        $scope.delTipsName="";
        $scope.delTipsTime=dTips;
        $scope.delTipsType="";
        $scope.delTipsTag="";
        $scope.delTipsAward="";
        $scope.delTipsPunish="";
        document.getElementById("endtime").focus();
    }
   else if($scope.addcondition.TaskTypeId==0){
        $scope.addcondition.warm="请选择任务类型";
        $scope.delTipsName="";
        $scope.delTipsTime="";
        $scope.delTipsType=dTips;
        $scope.delTipsTag="";
        $scope.delTipsAward="";
        $scope.delTipsPunish="";
        document.getElementById("type").focus();
    }
   else if($scope.addcondition.TaskTagId==0){
        $scope.addcondition.warm="请选择任务目标";
        $scope.delTipsName="";
        $scope.delTipsTime="";
        $scope.delTipsType="";
        $scope.delTipsTag=dTips;
        $scope.delTipsAward="";
        $scope.delTipsPunish="";
        document.getElementById("tag").focus();}
   else if($scope.addcondition.TaskAwardId==0){
        $scope.addcondition.warm="请选择任务奖励";
        $scope.delTipsName="";
        $scope.delTipsTime="";
        $scope.delTipsType="";
        $scope.delTipsTag="";
        $scope.delTipsAward=dTips;
        $scope.delTipsPunish="";
        document.getElementById("award").focus();
    }
   else if($scope.addcondition.TaskPunishmentId==0){
        $scope.addcondition.warm="请选择任务惩罚";
        $scope.delTipsName="";
        $scope.delTipsTime="";
        $scope.delTipsType="";
        $scope.delTipsTag="";
        $scope.delTipsAward="";
        $scope.delTipsPunish=dTips;
        document.getElementById("punish").focus();
    }
else if(!$scope.addcondition.Endtime){
        $scope.addcondition.warm="日期格式错误";
        document.getElementById("endtime").focus();
    }

    else{
        getTaskResult();
    }

}
    var getTaskResult  = function() {
        $scope.addcondition.type='add',
            $http.post(SETTING.ApiUrl+'/Task/AddTask',$scope.addcondition,{
            'withCredentials':true
        }).success(function(data){
           if(data.Status){

               $scope.addcondition.Taskname='';
               $scope.addcondition.Endtime='';
               $scope.addcondition.Describe='';
               $scope.addcondition.warm=data.Msg;
               $scope.delTipsName=dTips;
               $scope.delTipsTime=dTips;
               $scope.delTipsType=dTips;
               $scope.delTipsTag=dTips;
               $scope.delTipsAward=dTips;
               $scope.delTipsPunish=dTips;
              // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
                else{

               $scope.addcondition.warm=data.Msg;

           }

        });
    };
    $scope.gettaskRe=dd;
    //删除提示
    var dTips=function (){
        $scope.addcondition.warm="";
    }

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
        $http.get(SETTING.ApiUrl+'/Task/TaskAwardList',{ 'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.taskaward=data;
        });

    };
    getTaskAward();
    //绑定任务惩罚
    var getTaskPunishment  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskPunishmentList',{ 'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.taskpunishment=data;
        });

    };
    getTaskPunishment();
    // 结束时间

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        dTips();
        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        class: 'datepicker'
    };

    $scope.initDate = new Date();
    $scope.formats = ['yyyy/MM/dd', 'shortDate'];
    $scope.format = $scope.formats[0];


}
]);