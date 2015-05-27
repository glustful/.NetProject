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
    //查询任务
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.totalCount>0){

                $scope.list = data.list;
                $scope.searchCondition.page=data.condition.Page;
                $scope.searchCondition.pageSize=data.condition.PageCount;
                $scope.searchCondition.totalCount=data.totalCount;}
            else{

            }
        });
    };
    $scope.getList = getTaskList;
    getTaskList();


}
]);