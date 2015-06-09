/**
 * Created by Administrator on 2015/6/8.
 */
app.controller('personController',['$http','$scope','AuthService',function($http,$scope,AuthService) {
    $scope.searchCondition = {
        Id:0,
        page: 1,
        pageSize: 10,
        type:""
    };
    $scope.addcondition={
        TaskId:0,
        BrokerId:2,//经纪人ID
        Taskschedule:'1',
        Type:'add'
    }
    $scope.warm="";
    $scope.tipp="加载更多。。。"
    //查询任务
    var page = 1                                //读取的页数
        , loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.posts = [];
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容
        $scope.searchCondition.type="today";
        if (!loading && page < pages) {                         //如果页面没有正在读取
            loading = true;                     //告知正在读取

            $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
                  if(!data.Status) {
                      pages = data.totalCount / 10 + 1;
                      console.log(data);
                      for (var i = 0; i <= data.list.length - 1; i++) {
                          $scope.posts.push(data.list[i]);
                      }
                      loading = false;            //告知读取结束
                      $scope.ttcount = data.totalCount;

            } else{
                      $scope.ttcount = "无";
            }});
            page++;                             //翻页
            if (page > pages) {
                $scope.tipp = "没有更多了";
            }
        }
        else {
            $scope.tipp = "没有更多了";
        }
    };
    pushContent();
    //$scope.more=pushContent;


    //接受任务
    var addlist=function(id){
        $scope.addcondition.TaskId=id;
        $scope.addcondition.brokerId=AuthService.userId ;
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
