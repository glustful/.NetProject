/**
 * Created by Administrator on 2015/6/10.
 */
app.controller('BrandController',['$http','$scope','$stateParams','$timeout',function($http,$scope,$stateParams,$timeout){
        var condition = {
            condition:$stateParams.condition==undefined?'':$stateParams.condition,
            //condition:$stateParams.condition,
            page:0,
            PageCount:10
        };
    $scope.tipp="正在加载......";
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.BrandList = [];//保存从服务器查来的任务，可累加
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容

        if (!loading && condition.page < pages) {                         //如果页面没有正在读取
            loading = true;                     //告知正在读取
            $http.get(SETTING.ApiUrl+'/Brand/SearchBrand',{params:condition,'withCredentials':true}).success(function(data) {
                pages =Math.ceil(data.Count /condition.PageCount);
                for (var i = 0; i <= data.List.length - 1; i++) {
                    $scope.BrandList.push(data.List[i]);
                }
                loading = false;            //告知读取结束
//                    $scope.tipp="加载更多"+"("+$scope.posts.length+"/"+data.totalCount+")";
                $scope.tipp="加载更多......";
                if ($scope.BrandList.length == data.Count) {//如果所有数据已查出来
                    $scope.tipp = "已经是最后一页了";
                }
                $scope.Count=data.Count;
            });
            condition.page++;                             //翻页
        }
//        else {
//            $scope.tipp = "已经是最后一页了";
//        }

    };
    pushContent();
    //$scope.more=pushContent;
    function pushContentMore(){
        if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
        {
            pushContent();//if判断有没有滑动到底部，到了加载
        }
        $timeout(pushContentMore, 2500);//定时器，每隔一秒循环调用自身函数
    }
    pushContentMore();//触发里面的定时器
}]);