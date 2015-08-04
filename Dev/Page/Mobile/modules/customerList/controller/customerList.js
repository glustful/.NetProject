/**
 * Created by 黄秀宇 on 2015/6/1.
 */
app.controller('cusListController',['$http','$scope','AuthService','$timeout',function($http,$scope,AuthService,$timeout) {
    $scope.searchCondition = {
        id:'',
        page: 0,
        pageSize: 10
    };

    //查询客户
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.list = [];//保存从服务器查来的任务，可累加
    $scope.warm="";
    $scope.tipp="正在加载。。。";
    $scope.cuscount=0;//保存客户总数

    var pushContent= function() {
        $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
        $scope.searchCondition.id=$scope.currentuser.userId ;
        if (!loading && $scope.searchCondition.page < pages) {

            loading = true;                     //告知正在读取
            $http.get(SETTING.ApiUrl+'/ClientInfo/GetStatusByUserId/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data) {
               if(data.List == "") {
                   $scope.tipp="当前不存在带客信息";
               }
                pages =Math.ceil(data.totalCount /$scope.searchCondition.pageSize);
                for (var i = 0; i <= data.List.length - 1; i++) {
                    $scope.list.push(data.List[i]);
                }
                loading = false;            //告知读取结束
                $scope.tipp="加载更多......";
                if ($scope.list.length == data.totalCount) {//如果所有数据已查出来
                    $scope.tipp = "已经是最后一页了";
                }
                $scope.Count=data.totalCount;
            });
            $scope.searchCondition.page++;                             //翻页
        }
    };
    pushContent();
    //$scope.more=pushContent;
    function pushContentMore(){

        if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
        {
            pushContent();//if判断有没有滑动到底部，到了加载
        }
        $timeout(pushContentMore, 2000);//定时器，每隔一秒循环调用自身函数
    }
    pushContentMore();//触发里面的定时器


    //
    //    var getcustomerList  = function() {
    //    $scope.searchCondition.id=$scope.currentuser.userId ;
    //    if (!loading &&  $scope.searchCondition.page < pages) {                         //如果页面没有正在读取
    //        loading = true;                     //告知正在读取
    //    $http.get(SETTING.ApiUrl+'/ClientInfo/GetStatusByUserId/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
    //            if(data.list==""){
    //                $scope.Tips="当前不存在带客信息！"
    //            }else{
    //                $scope.list=data.list;
    //            }
    //    }
    //    );}
    //};
    //getcustomerList();
    //$scope.more=getcustomerList;
//隐藏显示元素
    $scope.visible = false;
    $scope.toggle = function (id) {
       $("#"+id).slideToggle("slow");
    };

  }]);

