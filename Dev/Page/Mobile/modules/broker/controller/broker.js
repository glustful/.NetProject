/** create by 杨波 2015.6.5 创富英雄榜**/
app.controller('BrokerTopThreeController',['$scope','$http','AuthService','$state',function($scope,$http,AuthService,$state){
    var BrokerTopThree=function() {
        $http.get(SETTING.ApiUrl + '/BrokerInfo/OrderByBrokerTopThree', {'withCredentials': true}).success(function (data) {
//           $scope.ii=0;
//           for(var i=0;i<data.List.length-1;i++)
//            {
//                $scope.list.push(data.List[i]);
//                $scope.ii=i+1;
//            }
//           console.log($scope.li);
           $scope.list = data.List;
            console.log($scope.list);
        })

    };
    BrokerTopThree();

    //获取brandlist data  滚动翻页
    $scope.searchCondition = {

        page: 0,
        pageSize:6
    };
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.brandList = [];//保存从服务器查来的任务，可累加
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容
        $scope.searchCondition.type="all";
        loading=false, pages=5;
        //alert(loading);alert(pages);
        if (!loading && $scope.searchCondition.page < pages) {                         //如果页面没有正在读取

            loading = true;                         //告知正在读取
            $http.get(SETTING.ApiUrl+'/Brand/GetAllBrand',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
                if (!data.Status) {
                    pages = data.totalCount / 6 ;

                    console.log(data);
                    for (var i = 0; i <= data.List.length - 1; i++) {
                        $scope.brandList.push(data.List[i]);
                    }
                    loading = false;            //告知读取结束
                    $scope.tipp="加载更多"+"("+$scope.brandList.length+"/"+data.totalCount+")";
                    if ($scope.brandList.length == data.totalCount) {//如果所有数据已查出来
                        $scope.tipp = "没有更多了,共("+$scope.tcount+")条";
                    }
                    $scope.tcount=data.totalCount;
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
            $scope.tipp = "没有更多了";
        }


    };
    pushContent();
    $ (document).ready(//文档加载完后执行里面的函数
        function (){
            $(window).scroll(function(){//滑动时执行
                if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
                {
                    pushContent();//if判断有没有滑动到底部，到了加载
                }
            })
        });

    //经纪人专区图片轮播
    $scope.channelName='banner';
    $http.get(SETTING.ApiUrl+'/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });
}]);