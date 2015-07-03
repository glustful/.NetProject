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
//               console.log(data.list.Status);
                forEach( $scope.list in $scope.list)
                {
                    $scope.statuss = $scope.list;
                    alert($scope.statuss);
                }

           if ($scope.list.Status == "预约中" || $scope.list.Status == "审核中"){
               $scope.statusStyle = "信息提交，等待审核";
           }else if($scope.list.Status == "等待上访"){
               $scope.statusStyle = "审核成功，等待上访";
           }else if ($scope.list.Status == "洽谈中"){
               $scope.statusStyle = "上访成功，洽谈中";
           }else if ($scope.list.Status == "洽谈成功"){
               $scope.statusStyle = "交易成功";
           }
            console.log($scope.statusStyle);
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

