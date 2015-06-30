/**
 * Created by 黄秀宇 on 2015/5/12.
 */
app.controller('taskIndexController',['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.searchCondition = {
        Taskname: '',

        Id:0,
        page: 1,
        pageSize: 10
    };
    //查询任务
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            if(data.totalCount>0){
                console.log(data);
                $scope.visibleif=true;
                $scope.tips="";
            $scope.list = data.list;
            $scope.searchCondition.page=data.condition.Page;
            $scope.searchCondition.pageSize=data.condition.PageCount;
            $scope.searchCondition.totalCount=data.totalCount;}
            else{
                $scope.visibleif=false;
                $scope.tips="没有数据";
            }
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
    //删除任务
    $scope.delTask= function (id) {
        $scope.selectedId = id;
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(function () {
            $http.get(SETTING.ApiUrl +'/Task/DelTask/', {
                    params: {
                        id: $scope.selectedId
                    }, 'withCredentials':true
                }
            ).success(function (data) {
                    if (data.Status) {
                        getTaskList();
                        var modalInstanc = $modal.open({
                            templateUrl: 'myModalContent.html',
                            controller: 'ModalInstanceCtrl',
                            resolve: {
                                msg: function () {
                                    return data.Msg;
                                }
                            }
                        });

                    }
                    else{
                        var modalInstanc = $modal.open({
                            templateUrl: 'myModalContent.html',
                            controller: 'ModalInstanceCtrl',
                            resolve: {
                                msg: function () {
                                    return data.Msg;
                                }
                            }
                        });
                      //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                        }
                });
        });
    }
//   var DelTaskbyId = function(id11) {
//        $http.get(SETTING.ApiUrl+'/Task/DelTask/',{params:{id:id11}, 'withCredentials':true}).success(function(data){
//            if(data.Status){
//                getTaskList();
//
//                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
//            }
//        });
//    };
//    $scope.delTask = DelTask;
   }
]);
app.controller('taskListcontroller',['$http','$scope','$stateParams','$modal',function($http,$scope,$stateParams,$modal) {
    $scope.searchCondition1 = {
        brokerName:'',
        id:$stateParams.id,
        page: 1,
        pageSize: 10
    };
//加载时绑定，绑定任务列表,根据接受者查询该任务任务列表
    var getTaskList1  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1, 'withCredentials':true}).success(function(data){
            if(data.totalCount>0){
                $scope.visibleif=true;
                $scope.tips="";
            $scope.taskModel = data.list;
            $scope.searchCondition1.page=data.condition.Page;
            $scope.searchCondition1.pageSize=data.condition.PageCount;
            $scope.searchCondition1.totalCount=data.totalCount;}
            else{
                $scope.visibleif=false;
                $scope.tips="没有数据";
}

        });
    };
$scope.getList1=getTaskList1 ;
    getTaskList1();
    /*
    //根据接受者查询该任务任务列表
    var getTaskListSer  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1}).success(function(data){

            console.log(data);
            $scope.taskModel = data.list;
            $scope.searchCondition1.page=data.condition.Page;
            $scope.searchCondition1.pageSize=data.condition.PageCount;
            $scope.searchCondition1.totalCount=data.totalCount;

        });
    };
    $scope.gettaskl
    ist=getTaskListSer;*/
}
]);

//app.controller('taskDetailcontroller',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
//
//    var getTaskDetail  = function() {
//        $http.get(SETTING.ApiUrl+'/Task/TaskDetail?id='+$stateParams.id).success(function(data){
//
//            console.log(data);
//            $scope.taskModel1 = data;
//        });
//    };
//    $scope.gettaskdetail=getTaskDetail();
//    getTaskDetail();
//    // 结束时间
//    $scope.open = function ($event) {
//        $event.preventDefault();
//        $event.stopPropagation();
//
//        $scope.opened = true;
//    };
//
//    $scope.dateOptions = {
//        formatYear: 'yy',
//        startingDay: 1,
//        class: 'datepicker'
//    };
//
//    $scope.initDate = new Date();
//    $scope.formats = ['yyyy/MM/dd', 'shortDate'];
//    $scope.format = $scope.formats[0];
//}
//]);


