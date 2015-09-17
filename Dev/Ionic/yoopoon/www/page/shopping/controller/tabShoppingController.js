/**
 * Created by Administrator on 2015/9/7.
 */



app.controller('TabShoppingCtrl',['$http','$scope','$stateParams','$timeout','$ionicLoading','cartservice',function($http,$scope,$stateParams,$timeout,$ionicLoading,cartservice){

    //console.log(cartservice.GetAllcart());
    $scope.wxPay = function(){
        $ionicLoading.show({
            template:"微信支付未开通",
            duration:3000
        });
    };
    //商品信息
    //$scope.cartinfo={
    //    id:null,
    //    name:null,
    //    count:null
    //};
    //添加商品
    //$scope.AddCart = function()
    //{
    //    //赋值
    //
    //    cartservice.add(cartinfo);
    //}
    $scope.alipay = function(){
        var myDate = new Date();

        var tradeNo = myDate.getTime();   
        var alipay = navigator.alipay;
  
        alipay.pay({
            "seller" : "yunjoy@yunjoy.cn", //卖家支付宝账号或对应的支付宝唯一用户号
            "subject" : "测试支付", //商品名称
            "body" : "测试支付宝支付", //商品详情
            "price" : "0.01", //金额，单位为RMB
            "tradeNo" : tradeNo, //唯一订单号
            "timeout" : "30m", //超时设置
            "notifyUrl" : "http://www.baidu.com"
            }, function(result) {
                
                    $ionicLoading.show({
                       template: "支付宝返回结果="+result,
                        noBackdrop: true,
                        duration: 5000
                    });
            }, function(message) {
                 $ionicLoading.show({
                  template: "支付宝支付失败="+message,
                   noBackdrop: true,
                  duration: 5000
                });
               
            });
    };



    $scope.go=function(state){
        window.location.href=state;
    };

    //    搜索功能
    $scope.showSelect = false;
    $scope.isShow = false;
    $scope.showInput = function () {
        $scope.showSelect = true;
        $scope.isShow = true;
    };
    $scope.AddGWCAction = function()
    {
        //显示图标
        var actionDOM = document.getElementById("gwcaction");
        actionDOM.style.visibility = "visible";
        //执行动画
        var abc = actionDOM.className;
        actionDOM.className = abc+"Gwcactive";
        //执行完毕动画后，隐藏图标
        $timeout(show,1000);
        function show()
        {
            actionDOM.style.visibility = "hidden";
        }

    }
    //region商品大图获取

    $scope.Condition = {
        IsDescending:true,
        OrderBy:'OrderByOwner'

        //ProductId:''
    };
    var getProductList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data1) {
            $scope.list = data1.List[0];
        });;
    };
    getProductList();
    $scope.getList=getProductList;
//endregion

    //region 商品获取
    $scope.items = [];
    $scope.searchCondition = {
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        Page:1,
        PageCount:5
        //ProductId:''
    };

    var getList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.searchCondition,
            'withCredentials': true
        }).success(function (data) {
            if(data.List!="") {
                $scope.items = data.List;
            }
        });
    };
    getList();
//endregion


    //region 商品加载
    $scope.load_more = function(){
        $timeout(function(){
            $scope.searchCondition.Page+=1;
            $http.get('http://localhost:50597/api/CommunityProduct/Get', {
                params: $scope.searchCondition,
                'withCredentials': true
            }).success(function (data) {
                if(data.List!="") {
                    for (var i = 0; i < data.List.length; i++) {
                        $scope.items.push(data.List[i]);
                    }
                }

                $scope.$broadcast("scroll.infiniteScrollComplete");
            });


        },1000)
    };
    //endregion


    //region 图片轮播
    $scope.channelName='banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });
    //endregion
    }]);
app.controller('ShoppingListCtrl',['$http','$scope',function($http,$scope){

    //
    //region 获取商品列表
    $scope.sech={
        Page:1,
       // PageCount:5,
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        CategoryId:3,
       // Name:'',
        PriceBegin:'',
        PriceEnd:''
    };
    $scope.orderByPrice=function(){
        $scope.sech.OrderBy='OrderByPrice';
        getProduct();
    }
    $scope.orderByOwner=function(){
        $scope.sech.OrderBy='OrderByOwner';
        getProduct();
    }

    var getProduct=function() {
        $http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
            params: $scope.sech,
            'withCredentials': true  //跨域
        }).success(function (data) {
            $scope.list = data.List;
            //$scope.sech.Page = data.Condition.Page;
            //$scope.sech.PageCount = data.Condition.PageCount;
            //$scope.totalCount = data.TotalCount;
        });
    }
    getProduct();
    //region 条件排序
    $scope.select='0';
    $scope.selected='0';
    $scope.change = function(x){
       if(x==1)
       {
           $scope.orderByPrice();
       }
       else if(x==2)
        {
            $scope.orderByOwner();
        }
    }
    $scope.productShow=true;
    $scope.productPrice=false;
    $scope.selectPrice=function(){

//            document.getElementById("list").style.display="none";
            $scope.productShow=false;
            $scope.productPrice=true;
        }

    $scope.submit=function(){
        document.getElementById("price").setAttribute("class","");
        $scope.productPrice=false;
        $scope.productShow=true;
        getProduct();
    }
    //endregion
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
}])
app.controller('ProductDetail',['$http','$scope','$stateParams','$timeout',
    function($http,$scope,$stateParams,$timeout){
    //region 轮播图
    $scope.channelName='banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });
    //endregion
    //region 获取商品详情
    $http.get(SETTING.ApiUrl+"/CommunityProduct/Get?id="+$stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.product=data.ProductModel;
    })
    //endregion
    //region 获取评论
    $scope.comcon={
        Page:0,
        PageCount:2,
        ProductId:$stateParams.id
    }
    $scope.tipp = "查看更多评论";
    //var loading = false
    //    ,pages=2;                      //判断是否正在读取内容的变量
    $scope.CommentList = [];//保存从服务器查来的任务，可累加
    //var pushContent= function() {                    //核心是这个函数，向$scope.posts
    //    if (!loading && $scope.comcon.Page < pages) {                         //如果页面没有正在读取
    //        loading = true;                     //告知正在读取
    //        $http.get(SETTING.ApiUrl + "/ProductComment/Get", {
    //            params: $scope.comcon,
    //            'withCredentials': true
    //        }).success(function(data) {
    //            pages =Math.ceil(data.TotalCount /$scope.comcon.PageCount);
    //            for (var i = 0; i <= data.Model.length - 1; i++) {
    //                $scope.CommentList.push(data.Model[i]);
    //                $http.get(SETTING.ApiUrl+"Member/GetMemberByUserId?userId="+data.Model[i].AddUser,{
    //                    'withCredentials':true
    //                }).success(function(data){
    //                    $scope.member=data;
    //                })
    //            }
    //            loading = false;            //告知读取结束
    //            if ($scope.CommentList.length == data.TotalCount) {//如果所有数据已查出来
    //                $scope.tipp = "已经是最后一页了";
    //            }
    //            $scope.Count=data.TotalCount;
    //            console.log(data.Model);
    //        });
    //        $scope.comcon.Page++;                             //翻页
    //    }
    //};
    //pushContent();
    //$scope.more=pushContent;
        var morecomment = function(){
            $timeout(function(){
                $scope.comcon.Page+=1;
                $http.get(SETTING.ApiUrl + "/ProductComment/Get", {
                    params: $scope.comcon,
                    'withCredentials': true
                }).success(function (data) {
                    if(data.Model!="") {
                        for (var i = 0; i < data.Model.length; i++) {
                            $scope.CommentList.push(data.Model[i]);
                        }
                    }
                    $scope.Count=data.TotalCount;
                });
            },1000)
        };
        morecomment();
        $scope.more=morecomment;

    //endregion
     //region 加载图文详情
    $scope.hasmore =true;
    $scope.load_detail = function(){
        $timeout(function(){
            if(!$scope.hasmore){
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
            $http.get(SETTING.ApiUrl+"/ProductDetail/Get?id="+$stateParams.id,{
                'withCredentials': true
            }).success(function(data){
                $scope.productDetail=data;
                $scope.hasmore=false
            });
        },1000);
    };
}])
app.controller('SearchProductCtr',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $scope.search={
        Name:$stateParams.name
    }
    $http.get(SETTING.ApiUrl+"/CommunityProduct/Get",{
        params:$scope.search,
        'withCredentials':true
    }).success(function(data){
        $scope.productList=data.List
    })
}])





