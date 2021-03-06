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
app.controller('taskController',['$http','$scope','AuthService','$timeout',function($http,$scope,AuthService,$timeout) {
    $scope.searchCondition = {
         Id:0,
        page: 0,
        pageSize: 10,
        type:""
    };
    $scope.addcondition={
        TaskId:0,
        BrokerId:2,//经纪人ID
        Taskschedule:'1',
        Type:'add',
        UserId:0
    }
    $scope.warm="";
    $scope.tipp="正在加载。。。";
    $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息

    //查询任务
   var loading = false
       ,pages=2;                      //判断是否正在读取内容的变量
    $scope.posts = [];//保存从服务器查来的任务，可累加
$scope.tcount=0;//保存任务总数
      function pushContent() {                    //核心是这个函数，向$scope.posts
        //添加内容
          $scope.searchCondition.type="all";
            if (!loading && $scope.searchCondition.page < pages) {                         //如果页面没有正在读取
                loading = true;                     //告知正在读取
                $http.get(SETTING.ApiUrl+'/Task/TaskListMobile/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data) {
                    if (!data.Status) {
                    pages = data.totalCount / 10 ;//计算总页数
                    $scope.tcount=data.totalCount;//保存任务个数
                    console.log(data);
                    for (var i = 0; i <= data.list.length - 1; i++) {
                        $scope.posts.push(data.list[i]);
                    }
                    loading = false;            //告知读取结束
                        $scope.tipp="加载更多"+"("+$scope.posts.length+"/"+data.totalCount+")";
                        if ($scope.posts.length == data.totalCount) {//如果所有数据已查出来
                            $scope.tipp = "没有更多了,共("+$scope.tcount+")条";
                        }

                    } else{
                        $scope.tipp = "没有任务";
            }
                    });
                $scope.searchCondition.page++;                             //翻页
                if ($scope.searchCondition.page > pages) {
                    $scope.tipp = "没有更多了,共("+$scope.tcount+")条";
                };

            }
            else {
                $scope.tipp = "没有更多了,共("+$scope.tcount+")条";
            }
    };
    pushContent();//初始化加载
    //自动加载定时器方法,得注入$timeout
//   $scope.more=pushContent;//手动加载更多
//    function pushContentMore(){
//       if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
//       {
//           pushContent();//if判断有没有滑动到底部，到了加载
//       }
//        $timeout(pushContentMore, 1000);//定时器，每隔一秒循环调用自身函数
//    }
//    pushContentMore();//触发里面的定时器
    //自动加载jq方法
$(document).ready(//文档加载完后执行里面的函数
    function (){
        $(window).scroll(function(){//滑动时执行
            if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
            {
                pushContent();//if判断有没有滑动到底部，到了加载
            }
        })
    }
);
$scope.more=pushContent;
    //接受任务
    var addlist=function(id){
        $scope.addcondition.TaskId=id;
        $scope.addcondition.UserId=$scope.currentuser.UserId ;
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
//app.controller('ClockCtrl',['$http','$scope','time',function($http,$scope, time) {
// $scope.time= time ;
//
//}]);
//app.factory('time', function($timeout) {
//    var time={};
//
//    (function tick() {
//        time.now= new Date().toString();
//        $timeout(tick, 1000);
//
//    })();
//    return time;
//});
//app.directive('whenScrolled', function() {
//    return function ss(scope, element, attrs) {
//        var raw = element[0];
//        alert(raw.scrollTop);
//        angular.element($window).bind('scroll', function () {
//            alert(raw.scrollTop);
//            if (raw.scrollTop+raw.offsetHeight >= raw.scrollHeight) {
//                scope.$apply(attrs.whenScrolled);
//            }
//
//        });
//      //  $timeout(alert(raw.scrollTop), 500);
//    };
//});
