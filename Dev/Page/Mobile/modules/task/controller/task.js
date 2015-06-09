///**
// * Created by 黄秀宇 on 2015/5/26.
// */
//app.controller('taskController',['$http','$scope',function($http,$scope) {
//    $scope.searchCondition = {
//        Taskname: '',
//
//        Id:0,
//        page: 1,
//        pageSize: 10
//    };
//    $scope.addcondition={
//        TaskId:0,
//        BrokerId:2,//经纪人ID
//        Taskschedule:'1',
//        Type:'add'
//    }
//$scope.warm="";
//    //查询任务
//    var getTaskList  = function() {
//        $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
//            console.log(data);
//            if(data.totalCount>0){
//                $scope.warm="";
//                $scope.list = data.list;
//                $scope.searchCondition.page=data.condition.Page;
//                $scope.searchCondition.pageSize=data.condition.PageCount;
//                $scope.totalCount=data.totalCount;}
//            else{
//                $scope.warm="没有任务可接";
//            }
//        });
//    };
//    $scope.getList = getTaskList;
//    getTaskList();
//    //接受任务
//var addlist=function(id){
//    $scope.addcondition.TaskId=id;
//        $http.post(SETTING.ApiUrl+'/Task/AddTaskList/',$scope.addcondition).success(function(data){
//            console.log(data);
//            if(data.Status){
//               }
//            else{
//                $scope.warm="没有任务可接";
//            }
//        });
//    };
//    $scope.addTaskList = addlist;
//
//}]);
//
/**
 * Created by 黄秀宇 on 2015/5/26.
 */
app.controller('taskController',['$http','$scope','AuthService',function($http,$scope,AuthService) {
    $scope.searchCondition = {
         Id:0,
        page: 1,
        pageSize: 10,
        type:""
    };
    $scope.addcondition={
        TaskId:0,
        UserId:0,//用户ID
        Taskschedule:'1',
        Type:'add'
    }
    $scope.warm="";
    $scope.tipp="加载更多。。。"
    //查询任务
   var  loading = false
       ,pages= 2,total=0;                      //判断是否正在读取内容的变量
    $scope.posts = [];

    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容
          $scope.searchCondition.type="all";
            if (!loading && $scope.searchCondition.page < pages)//如果页面没有正在读取
            {
                loading = true;                     //告知正在读取
                $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data) {
                    if (!data.Status)
                    {
                        $scope.searchCondition.page++;     //翻页
                    pages = data.totalCount / 10 + 1;//计算一共几页
                    total=data.totalCount;//保存任务总数
                    for (var i = 0; i <= data.list.length - 1; i++)
                    {
                        $scope.posts.push(data.list[i]);
                    }
                    loading = false;            //告知读取结束
                        $scope.tipp="加载更多"+$scope.posts.length+"/"+data.totalCount;
                }
                    else{
                        $scope.tipp = "目前没有任务，先去洗洗睡哟";
            }
                    }
                );

                if ($scope.searchCondition.page > pages)
                {
                    $scope.tipp = "没有更多了"+"(共"+total+"条)";
                }
            }
            else
            {
                $scope.tipp = "没有更多了"+"(共"+total+"条)";
            }
    };
    pushContent();
    $scope.more=pushContent;
    //接受任务
    var addlist=function(id){
        $scope.addcondition.TaskId=id;
       $scope.addcondition.UserId=AuthService.CurrentUser().UserId ;
        $http.post(SETTING.ApiUrl+'/Task/AddTaskList/',$scope.addcondition).success(function(data){
            console.log(data);
            if(data.Status){
            }
            else{
                //$scope.warm="没有任务可接";
            }
        });
    };
    $scope.addTaskList = addlist;
}]);

