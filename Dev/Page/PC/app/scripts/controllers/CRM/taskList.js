/**
 * Created by 黄秀宇 on 2015/5/12.
 */
app.controller('taskIndexController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        Taskname: '',

        Id:0,
        page: 1,
        pageSize: 10
    };
    //查询任务
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:$scope.searchCondition}).success(function(data){
           console.log(data);
            $scope.list = data.list;
            $scope.searchCondition.page=data.condition.Page;
            $scope.searchCondition.pageSize=data.condition.PageCount;
            $scope.searchCondition.totalCount=data.totalCount;
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
    //删除任务

   var DelTask = function(id11) {
        $http.get(SETTING.ApiUrl+'/Task/DelTask/',{params:{id:id11}}).success(function(data){
            if(data.Status){
                getTaskList();

                // ngDialog.open({ template: 'views/pages/CRM/TaskList/index.html' });
            }
        });
    };
    $scope.delTask = DelTask;
   }
]);
app.controller('taskListcontroller',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.searchCondition1 = {
        brokerName:'',
        id:$stateParams.id,
        page: 1,
        pageSize: 10
    };
//加载时绑定，绑定任务列表,根据接受者查询该任务任务列表
    var getTaskList1  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1}).success(function(data){

            console.log(data);
            $scope.taskModel = data.list;
            $scope.searchCondition1.page=data.condition.Page;
            $scope.searchCondition1.pageSize=data.condition.PageCount;
            $scope.searchCondition1.totalCount=data.totalCount;

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
    $scope.gettasklist=getTaskListSer;*/
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
    // 结束时间
    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

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


