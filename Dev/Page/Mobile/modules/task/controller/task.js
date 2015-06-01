/**
 * Created by 黄秀宇 on 2015/5/26.
 */
app.controller('taskController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        Taskname: '',

        Id:0,
        page: 1,
        pageSize: 10
    };
    $scope.addcondition={
        TaskId:0,
        BrokerId:2,//经纪人ID
        Taskschedule:'1',
        Type:'add'
    }
$scope.warm="";
    //查询任务
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.totalCount>0){
                $scope.warm="";
                $scope.list = data.list;
                $scope.searchCondition.page=data.condition.Page;
                $scope.searchCondition.pageSize=data.condition.PageCount;
                $scope.totalCount=data.totalCount;}
            else{
                $scope.warm="没有任务可接";
            }
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
    //接受任务
var addlist=function(id){
    $scope.addcondition.TaskId=id;
        $http.post(SETTING.ApiUrl+'/Task/AddTaskList/',$scope.addcondition).success(function(data){
            console.log(data);
            if(data.Status){
               }
            else{
                $scope.warm="没有任务可接";
            }
        });
    };
    $scope.addTaskList = addlist;

}]);

