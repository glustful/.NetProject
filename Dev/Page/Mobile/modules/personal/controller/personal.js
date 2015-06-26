/**
 * Created by Administrator on 2015/6/8.
 */
app.controller('personController',['$http','$scope','AuthService','$state',function($http,$scope,AuthService,$state) {

        $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
   var coun=2;//1为经纪人
        //判断是否是经纪人
    $http.get(SETTING.ApiUrl+'/ClientInfo/Getbroker/',{'withCredentials':true})
        .success(function(response) {
          coun=response.count;
            action();
        });
        function action(){
        if(coun===1)//为经纪人状态
   {

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
    $scope.userBroker={
    	Name:'',
    	photo:'',
    	allMoneys:0,
    	levelStr:'',
    	partnerCount:0,
    	refereeCount:0,
    	orderStr:0,
    	customerCount:0
    }
    //获取用户信息

    $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBrokerDetails',{'withCredentials':true})
    .success(function(response) {
    	$scope.userBroker = response;
    	console.log(response);
    	if($scope.userBroker.levelStr==null){
    	$scope.userBroker.levelStr = '铜';}
    	if($scope.userBroker.photo.length<10){
    	//没图片地址显示默认头像
        document.getElementById("img").src = "./modules/personal/static/image/personal/t.png";
    	}
    });
    //获取用户信息
    $scope.warm="";
    $scope.tipp="加载更多。。。"
    //查询任务
    var page = 1                                //读取的页数
        , loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.posts = [];//保存从服务器查来的任务，可累加
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
    //查询客户个数
    var getcustomerList  = function() {
        $http.get(SETTING.ApiUrl+'/ClientInfo/GetClientInfoNumByUserId/',{'withCredentials':true}).success(function(data){
                $scope.number = data.count;
        });
    };
    getcustomerList();

    //接受任务
    var addlist=function(id){
        $scope.addcondition.TaskId=id;
        $scope.addcondition.UserId=AuthService.CurrentUser().UserId ;
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
   }
        else if (coun === 0) {//一般用户
            $state.go("app.person_setting");//调到设置页面
        }
        else if (coun === 2) {//未登录
            $state.go("user.login");//调到登录页面
        }
        }
}
]
);
