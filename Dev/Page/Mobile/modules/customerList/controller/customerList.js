/**
 * Created by 黄秀宇 on 2015/6/1.
 */
app.controller('cusListController',['$http','$scope','AuthService',function($http,$scope,AuthService) {
    $scope.searchCondition = {
        id:'',
        page: 1,
        pageSize: 10
    };
    $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
    //查询客户
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.list = [];//保存从服务器查来的任务，可累加

    var getcustomerList  = function() {
        $scope.searchCondition.id=$scope.currentuser.userId ;
        if (!loading &&  $scope.searchCondition.page < pages) {                         //如果页面没有正在读取
            loading = true;                     //告知正在读取
        $http.get(SETTING.ApiUrl+'/ClientInfo/GetStatusByUserId/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
           $scope.list=data.list;
        }
        );}
    };
    getcustomerList();
    $scope.more=getcustomerList;
//隐藏显示元素
    $scope.visible = false;
    $scope.toggle = function (id) {
       $("#"+id).slideToggle("slow");
    };

  }]);

