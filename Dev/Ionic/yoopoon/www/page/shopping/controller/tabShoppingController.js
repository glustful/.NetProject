/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope','$timeout',function($http,$scope,$timeout){
$scope.test = function(){
    alert(55);
};
    $scope.go=function(state){
        window.location.href=state;
    };


    //region鍟嗗搧澶у浘鑾峰彇
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
        });;
    };
    getProductList();
    $scope.getList=getProductList;
//endregion

    //region 鍟嗗搧鑾峰彇
    $scope.items = [];
    $scope.searchCondition = {
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        Page:1,
        PageCount:10
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

    $scope.items = [];
    var base = 0;
    //$scope.items = [];
    //var base = 0;
    //$scope.load_more = function(){
    //    $timeout(function(){
    //        for(var i=0;i<10;i++)
    //            $scope.items.push(["item ",base].join(""));
    //        $scope.$broadcast("scroll.infiniteScrollComplete");
    //    },500);
    //};


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
            })
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },1000)
    };
    //endregion

    //region    婊氬姩鍒锋柊
 //   $scope.load_more = function(){
 //       $timeout(function(){
 //           $scope.searchCondition.Page+=1;
 //           $http.get('http://localhost:50597/api/CommunityProduct/Get', {
 //               params: $scope.searchCondition,
 //               'withCredentials': true
 //           }).success(function (data) {
 //               // $scope.product = data.List;
 //               //  items = data.List;
 //               if(data.List!="") {
 //                   for (var i = 0; i < data.List.length; i++) {
 //                       $scope.items.push(data.List[i]);
 //                   }
 //               }
 //           })
 //           $scope.$broadcast("scroll.infiniteScrollComplete");
 //       },1000);
 //};
 //   //endregion
 //
 //   //region 杞挱鍥剧墖
 //   $scope.channelName='banner';
 //   $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
 //       $scope.content=data;
 //   });
    }]);
app.controller('ShoppongListCtrl',['$http','$scope',function($http,$scope){
    //region 获取商品列表
    $scope.sech={
        Page:1,
        PageCount:10,
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        CategoryId:3
    };
    //$scope.orderByPrice=function(){
    //    $scope.sech.OrderBy='OrderByPrice';
    //    getProduct();
    //}
    //$scope.orderByPrice=function(){
    //    $scope.sech.OrderBy='OrderByOwner';
    //    getProduct();
    //}
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
}])
app.controller('ProductDetail',['$http','$scope','$stateParams','$timeout','$ionicLoading',function($http,$scope,$stateParams,$timeout,$ionicLoading){
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
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.CommentList = [];//保存从服务器查来的任务，可累加
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
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
    };
    pushContent();
    $scope.more=pushContent;
    //endregion
    $scope.hasmore = false;
    $scope.load_detail = function(){
        $timeout(function(){
            if(!$scope.hasmore){
                $scope.$broadcast('scroll.infiniteScrollComplete');
                return;
            }
            $http.get(SETTING.ApiUrl+"/ProductDetail/Get?id="+$stateParams.id,{
                'withCredentials': true
            }).success(function(data){
                $scope.productDetail=data;
                $scope.hasmore=false
            });
        },1000);
    };
    $scope.moreDataCanBeLoaded = function(){
        return $scope.hasmore=true;
        load_detail()
    }

  //加入购物车动画
    $scope.AddGWCAction = function()
    {
        //显示图标
        var actionDOM = document.getElementById("gwcaction");
        actionDOM.style.visibility = "visible";
        //执行动画
        var abc = actionDOM.className;
        actionDOM.className = abc+"Gwcactive";
        function hasClass( actionDOM,Gwcactive ){
            return !!actionDOM.className.match( new RegExp( "(\\s|^)" + Gwcactive + "(\\s|$)") );
        };
        function addClass( actionDOM,Gwcactive ){
            if( !hasClass( actionDOM,Gwcactive ) ){
                actionDOM.className += " " + Gwcactive;
            };
        };
        function removeClass( actionDOM,Gwcactive ){
            if( hasClass( actionDOM,Gwcactive ) ){
                actionDOM.className = actionDOM.className.replace( new RegExp( "(\\s|^)" + Gwcactive + "(\\s|$)" ), " " );
            };
        };

    }

}])




