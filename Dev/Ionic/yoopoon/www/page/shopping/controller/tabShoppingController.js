/**
 * Created by Administrator on 2015/9/7.
 */

app.controller('TabShoppingCtrl',['$http','$scope','$stateParams','$timeout','$ionicLoading',function($http,$scope,$stateParams,$timeout,$ionicLoading){
    $scope.wxPay = function(){
        $ionicLoading.show({
            template:"微信支付未开通",
            duration:3000
        });
    };
    $scope.alipay = function(){
        var alipay = navigator.alipay;
  
        alipay.pay({
            "seller" : "yunjoy@yunjoy.cn", //卖家支付宝账号或对应的支付宝唯一用户号
            "subject" : "测试支付", //商品名称
            "body" : "测试支付宝支付", //商品详情
            "price" : "0.01", //金额，单位为RMB
            "tradeNo" : "11111111111", //唯一订单号
            "timeout" : "30m", //超时设置
            "notifyUrl" : "http://www.baidu.com"
            }, function(result) {
                
                    $ionicLoading.show({
                       template: "支付宝返回结果="+result,
                        noBackdrop: true,
                        duration: 3000
                    });
            }, function(message) {
                 $ionicLoading.show({
                  template: "支付宝支付失败="+message,
                   noBackdrop: true,
                  duration: 3000
                });
               
            });
    };
    //Ã’Â³ÃƒÃ¦ÃŒÃ¸Ã—Âª

    $scope.go=function(state){
        window.location.href=state;
    }


//���¹���ˢ��

    $scope.Condition = {
        IsDescending:true,
        OrderBy:'OrderById',
        IsRecommend:'1'
        //ProductId:''
    };
    var getProductList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data1) {
            $scope.list = data1.List[0];
        })
    }
    getProductList();
    $scope.getList=getProductList;




    $scope.items = [];
    $scope.searchCondition = {
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        Page:1,
        PageCount:6
        //ProductId:''
    };
    var getList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.searchCondition,
            'withCredentials': true
        }).success(function (data) {
           // $scope.product = data.List;
          //  items = data.List;
            if(data.List!="") {
                $scope.items = data.List;
            }
        })
    }
    getList();


//    滚动刷新
$scope.searchCondition.Page+=1;
            $http.get('http://localhost:50597/api/CommunityProduct/Get', {
                params: $scope.searchCondition,
                'withCredentials': true
            }).success(function (data) {
                // $scope.product = data.List;
                //  items = data.List;
                if(data.List!="") {
                    for (var i = 0; i < data.List.length; i++) {
                        $scope.items.push(data.List[i]);

                    }
                    if($scope.items.length==data.TotalCount){
                        $scope.$broadcast("scroll.infiniteScrollComplete");
                    }
                }
            })
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },1000);
 };
    $scope.channelName='banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });

    $scope.load_detail = function(){
 //for(var i=0;i<10;i++,base++)
            //    $scope.items.push(["item ",base].join(""));
           // alert("aaaaaaaaaa");
            $http.get(SETTING.ApiUrl+"/ProductDetail/Get?id="+$stateParams.id,{
                'withCredentials': true
            }).success(function(data){
                $scope.productDetail=data;
            });
            $scope.$broadcast('scroll.infiniteScrollComplete');
        },500);
 };

     

    //
    //region 获取商品列表
    $scope.sech={
        Page:1,
        PageCount:10,
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        CategoryId:3
    };
    $scope.orderByPrice=function(){
        $scope.sech.OrderBy='OrderByPrice';
        getProduct();
    }
    $scope.orderByPrice=function(){
        $scope.sech.OrderBy='OrderByOwner';
        getProduct();
    }
     var getProduct=function() {
         $http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
             params: $scope.sech,
             'withCredentials': true  //跨域
         }).success(function (data) {
             $scope.list = data.List;
             $scope.sech.Page = data.Condition.Page;
             $scope.sech.PageCount = data.Condition.PageCount;
             $scope.totalCount = data.TotalCount;
         });
     }
    getProduct();
    //endregion
    //region 分类Id获取商品
    $scope.getList=function(categoryId){
        $scope.sech.CategoryId=categoryId;
        getProduct()
    };
   //endregion
    //region 获取第三级分类
    $http.get(SETTING.ApiUrl+"/Category/GetChildByFatherId?id="+$scope.sech.CategoryId,{
        'withCredentials':true
    }).success(function (data) {
        $scope.cateList=data;
    })
    //endregion
    //region 获取商品详情
    $http.get(SETTING.ApiUrl+"/CommunityProduct/Get?id="+$stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.product=data.ProductModel;
    })
    //endregion
    //region 获取商品评论
    $scope.comcon={
        Page:0,
        PageCount:2,
        ProductId:$stateParams.id
    }
    $scope.tipp = "查看更多评论";
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.CommentList = [];//保存从服务器查来的任务，可累加
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容
        if (!loading && $scope.comcon.Page < pages) {                         //如果页面没有正在读取
            loading = true;                     //告知正在读取
            $http.get(SETTING.ApiUrl + "/ProductComment/Get", {
                params: $scope.comcon,
                'withCredebtials': true
            }).success(function(data) {

 pages =Math.ceil(data.TotalCount /$scope.comcon.PageCount);
                for (var i = 0; i <= data.Model.length - 1; i++) {
                    $scope.CommentList.push(data.Model[i]);
                }
                loading = false;            //告知读取结束
                if ($scope.CommentList.length == data.TotalCount) {//如果所有数据已查出来
                    $scope.tipp = "已经是最后一页了";
                }
                $scope.Count=data.TotalCount;
                console.log(data.Model);
            });
            $scope.comcon.Page++;                             //翻页
        }
//        else {
//            $scope.tipp = "已经是最后一页了";
//        }
    };

    pushContent();
    $scope.more=pushContent;
    //endregion}]);
}]);





