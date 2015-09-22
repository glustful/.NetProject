/**
 * Created by Administrator on 2015/9/7.
 */



app.controller('TabShoppingCtrl', ['$http', '$scope', '$stateParams', '$state', '$timeout', '$ionicLoading', 'cartservice', function ($http, $scope, $stateParams, $state, $timeout, $ionicLoading, cartservice) {

    //console.log(cartservice.GetAllcart());
    $scope.wxPay = function () {
        $ionicLoading.show({
            template: "微信支付未开通",
            duration: 3000
        });
    };

    $scope.alipay = function () {

        var myDate = new Date();

        var tradeNo = myDate.getTime();
        var alipay = navigator.alipay;

        alipay.pay({
            "seller": "yunjoy@yunjoy.cn", //卖家支付宝账号或对应的支付宝唯一用户号
            "subject": "测试支付", //商品名称
            "body": "测试支付宝支付", //商品详情
            "price": "0.01", //金额，单位为RMB
            "tradeNo": tradeNo, //唯一订单号
            "timeout": "30m", //超时设置
            "notifyUrl": "http://www.baidu.com"
        }, function (result) {

            $ionicLoading.show({
                template: "支付宝返回结果=" + result,
                noBackdrop: true,
                duration: 5000
            });
        }, function (message) {
            $ionicLoading.show({
                template: "支付宝支付失败=" + message,
                noBackdrop: true,
                duration: 5000
            });

        });
    };

//页面跳转

    $scope.go = function (state) {
        window.location.href = state;
    };

    //    搜索功能
    $scope.showSelect = false;
    $scope.isShow = false;
    $scope.showInput = function () {
        $scope.showSelect = true;
        $scope.isShow = true;
    };



    //region地址获取
    $scope.Condition = {
        Page: 1,
        father:true,
        Parent_Id:''
    };
    $scope.pare=[];

    var getAddress=function(){
        $http.get(SETTING.ApiUrl+'/CommunityArea/Get',{
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data3) {
            if (data3.List != "") {
                $scope.addrss = data3.List;
                $scope.selected=data3.List[0].Id;//如果想要第一个值
                //for( i=0;i<data3.List.length;i++){
                //    if(data3.List[i].Parent=null)
                //    {
                //        $scope.pare.push (data3.List[i].Parent);
                //    }}
                //alert($scope.pare);
            }
        });
    }
    getAddress();

    $scope.SCondition = {

        Parent_Id:''
    };
    $scope.click=function(){
        $scope.SCondition.Parent_Id=$scope.selected
        $http.get(SETTING.ApiUrl+'/CommunityArea/Get',{
          params:$scope.SCondition,
            'withCredentials': true
        }).success(function(data){
            $scope.zilei=data.List;
        })

    }
    //endregion

    //region商品大图获取

    $scope.Condition = {
        IsDescending: true,
        OrderBy: 'OrderByOwner',
        IsRecommend: '1'
        //ProductId:''
    };
    var getProductList = function () {
        $http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data1) {
            $scope.list = data1.List[0];
        });
        ;
    };
    getProductList();
    $scope.getList = getProductList;
//endregion

    //region 商品获取
    $scope.items = [];
    $scope.searchCondition = {
        IsDescending: true,
        OrderBy: 'OrderByAddtime',
        Page: 1,
        PageCount: 5
        //ProductId:''
    };

    var getList = function () {
        $http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
            params: $scope.searchCondition,
            'withCredentials': true
        }).success(function (data) {
            if (data.List != "") {
                $scope.items = data.List;
            }
        });
    };
  getList();
//endregion
    //region 商品加载
    $scope.loadmore=true;
    $scope.load_more = function () {
        $timeout(function () {
            $scope.searchCondition.Page += 1;
            $http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
                params: $scope.searchCondition,
                'withCredentials': true
            }).success(function (data) {

                if (data.List != "") {
                    for (var i = 0; i < data.List.length; i++) {
                        $scope.items.push(data.List[i]);
                    }
                    if($scope.items.length==data.TotalCount)
                    {
                        $scope.loadmore=false;
                    }
                }
                $scope.$broadcast("scroll.infiniteScrollComplete");
            });
        }, 1000)
    };


    //endregion

    //region 图片轮播
    $scope.channelName = 'banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg', {
        params: {ChannelName: $scope.channelName},
        'withCredentials': true
    }).success(function (data) {
        $scope.content = data;
    });
    //endregion

    //region 购物车

    $scope.cartinfo = {
        id: null,
        name: null,
        count: null
    };
    // 添加商品
    $scope.AddCart = function(data)
    {
        $scope.cartinfo.id=data.row.Id;
        $scope.cartinfo.name=data.row.Name;
        $scope.cartinfo.mainimg=data.row.MainImg;
        $scope.cartinfo.price=data.row.Price;
        $scope.cartinfo.newprice=data.row.NewPrice;
        $scope.cartinfo.count=1;
        cartservice.add($scope.cartinfo);
    }
    $scope.AddCart1 = function(list)
    {
        $scope.cartinfo.id=$scope.list.Id;
        $scope.cartinfo.name=$scope.list.Name;
        $scope.cartinfo.mainimg=$scope.list.MainImg;
        $scope.cartinfo.price=$scope.list.Price;
        $scope.cartinfo.newprice=$scope.list.NewPrice;
        $scope.cartinfo.count=1;
        cartservice.add($scope.cartinfo);
    };
    //endregion


    $scope.searchname = '';
    document.getElementById('search').onblur = function () {
        $state.go("page.search_product", {productName: $scope.searchname});
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
        $scope.go=function(state){
            window.location.href=state;
        };
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
    $scope.CommentList = [];//保存从服务器查来的任务，可累加
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



        }
//        加入购物车
        $scope.changIng=false;
        $scope.AddCart=function(){
            $scope.changIng=true;
        }
//       立即购买
        $scope.buyHide=true;
        $scope.mask=false;
        $scope.innerAfter=false;
        $scope.buyNew=function(){
            $scope.mask=true;
            $scope.buyHide=false;
//            var t=setTimeout("$scope.innerAfter=true",1000)
            $scope.innerAfter=true
        }
         //关闭
        $scope.close=function(){
            $scope.mask=!$scope.mask;
            $scope.buyHide=! $scope.buyHide;
        }
//        数量
        $scope.numbers=1;
        $scope.addNumbers=function(){
//            if($scope.numbers>=1)
            $scope.numbers=$scope.numbers+1;
//            else{
//                $scope.numbers=1;
//            }
        }
        $scope.deNumbers=function(){
            if($scope.numbers>=2)
            $scope.numbers-=1;
            else{
                $scope.numbers=1;
            }
        }
}])
app.controller('SearchProductCtr',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $scope.search={
        Name:$stateParams.productName
    }
    $http.get(SETTING.ApiUrl+"/CommunityProduct/Get",{
        params:$scope.search,
        'withCredentials':true
    }).success(function(data){
        $scope.productList=data.List
    })
}])







