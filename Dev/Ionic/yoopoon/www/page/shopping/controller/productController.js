/**
 * Created by Administrator on 2015/9/18.
 */
app.controller('ShoppingListCtrl', ['$http', '$scope', '$timeout','$stateParams',
    function ($http, $scope, $timeout,$stateParams) {

    $scope.category=$stateParams.name;
    //region 获取商品列表
    $scope.sech = {
        Page: 0,
        PageCount: 5,
        IsDescending: true,
        OrderBy: 'OrderByAddtime',
        CategoryId: $stateParams.id,
        // Name:'',
        PriceBegin: '',
        PriceEnd: ''
    };
    $scope.orderByPrice = function () {
        $scope.sech.OrderBy = 'OrderByPrice';
        // getProduct();
        $scope.sech.Page = 0;
        $scope.list = [];
        $scope.hasmore = true;
        $scope.loadProduct()
    }
    $scope.orderByOwner = function () {
        $scope.sech.OrderBy = 'OrderByOwner';
        $scope.sech.Page = 0;
        $scope.list = [];
        $scope.hasmore = true;
        //getProduct();
        $scope.loadProduct()
    }
    $scope.list = []
    //var getProduct=function() {
    //    $http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
    //        params: $scope.sech,
    //        'withCredentials': true  //跨域
    //    }).success(function (data) {
    //        $scope.list = data.List;
    //        //if($scope.list.length<data.TotalCount)
    //        //{
    //        //    setTimeout(function(){$scope.hasmore=true},500)
    //        //}
    //    });
    //}
    //getProduct();
    //region 加载更多
    $scope.hasmore = true;
    $scope.loadProduct = function () {
        $timeout(function () {
            $scope.sech.Page += 1;
            $http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
                params: $scope.sech,
                'withCredentials': true  //跨域
            }).success(function (data) {
                if(data.List.length==0)
                {
                    $scope.hasmore = false;
                }
                for (var i = 0; i < data.List.length; i++) {
                    $scope.list.push(data.List[i]);
                    if ($scope.list.length == data.TotalCount) {
                        $scope.hasmore = false;
                    }
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                }
                $scope.$broadcast("scroll.infiniteScrollComplete");
            });
        }, 1000)
    };
    //endregion
    //region 条件排序
    $scope.select = '0';
    $scope.selected = '0';
    $scope.change = function (x) {
        if (x == 1) {
            $scope.orderByPrice();
        }
        else if (x == 2) {
            $scope.orderByOwner();
        }
    };
    $scope.productShow = true;
    $scope.productPrice = false;
    $scope.selectPrice = function () {

//            document.getElementById("list").style.display="none";
        $scope.productPrice = !$scope.productPrice;
        if ($scope.productPrice == true) {
            $scope.productShow = false;
            $scope.productPrice = true;
        } else {
            $scope.productShow = true;
        }
    };

    $scope.submit = function () {
        $scope.productPrice = false;
        $scope.productShow = true;
        $scope.sech.Page = 0;
        $scope.list = [];
        $scope.hasmore = true;
        $scope.loadProduct()
    }
//    综合排序
    $scope.reorder = false;
    $scope.reorderAll = function () {
        $scope.reorder = !$scope.reorder;
    };
    //endregion
    //endregion
    //region 分类Id获取商品
    $scope.getList = function (categoryId) {
        $scope.sech.CategoryId = categoryId;
        // getProduct()
        $scope.sech.Page = 0;
        $scope.list = [];
        $scope.hasmore = true;
        $scope.loadProduct()
    };
    //endregion
    //region 获取第三级分类
    $http.get(SETTING.ApiUrl + "/Category/GetChildByFatherId?id=" + $scope.sech.CategoryId, {
        'withCredentials': true
    }).success(function (data) {
        $scope.cateList = data;
    });
    //endregion
}])
app.controller('ProductDetail', ['$http', '$scope', '$state','$stateParams', '$timeout','$ionicSlideBoxDelegate', 'cartservice',
    function ($http, $scope, $state,$stateParams, $timeout, $ionicSlideBoxDelegate,cartservice) {
        //region 轮播图
        $scope.$on('$ionicView.enter', function () {
            $ionicSlideBoxDelegate.start();
        });
        $scope.channelName = 'banner';
        $http.get('http://localhost:50597/api/Channel/GetTitleImg', {
            params: {ChannelName: $scope.channelName},
            'withCredentials': true
        }).success(function (data) {
            $scope.content = data;
        });
        //endregion
        //region 获取商品详情
        $http.get(SETTING.ApiUrl + "/CommunityProduct/Get?id=" + $stateParams.id, {
            'withCredentials': true
        }).success(function (data) {
            $scope.product = data.ProductModel;
            if(data!=null)
            {
                setTimeout(function(){
                    $scope.hasload=true
                },2000)
            }
        })
        //endregion
        //region 获取评论
        $scope.comcon = {
            Page: 0,
            PageCount: 2,
            ProductId: $stateParams.id
        }
        $scope.tipp = "查看更多评论";
        $scope.CommentList = [];//保存从服务器查来的任务，可累加
        var morecomment = function () {
            $timeout(function () {
                $scope.comcon.Page += 1;
                $http.get(SETTING.ApiUrl + "/ProductComment/Get", {
                    params: $scope.comcon,
                    'withCredentials': true
                }).success(function (data) {
                    if (data.Model != "") {
                        for (var i = 0; i < data.Model.length; i++) {
                            $scope.CommentList.push(data.Model[i]);
                        }
                    }
                    $scope.Count = data.TotalCount;
                });
            }, 1000)
        };
        morecomment();
        $scope.more = morecomment;

        //endregion
        //region 加载图文详情
       // $scope.hasload=false;
        var load_detail = function () {
            $timeout(function () {
                $http.get(SETTING.ApiUrl + "/ProductDetail/Get?id=" + $stateParams.id, {
                    'withCredentials': true
                }).success(function (data) {
                    $scope.productDetail = data;
                    if(data!=null)
                    {
                        $scope.hasload=false;
                    }
                });
               $scope.$broadcast("scroll.infiniteScrollComplete");
            }, 1000);
        };
        $scope.isDetail=false;
        $scope.getDetail=function(){
            load_detail();
            $scope.isDetail=true;
        }
        //endregion
        //region 加入购物车
        $scope.cartinfo = {
            id: null,
            name: null,
            count: null,
            mainimg:null,
            price:null,
            oldprice:null,
            //parameterValue:[]
        };
        $scope.changIng=false;
        $scope.AddCart=function(){
            $scope.cartinfo.id = $scope.product.Id;
            $scope.cartinfo.name = $scope.product.Name;
            $scope.cartinfo.mainimg=$scope.product.MainImg;
            $scope.cartinfo.price=$scope.product.Price;
            $scope.cartinfo.oldprice=$scope.product.OldPrice;
            //$scope.cartinfo.parameterValue=$scope.product.ParameterValue;
            $scope.cartinfo.count = 1;
            cartservice.add($scope.cartinfo);
            $scope.changIng=true;
        }
        //endregion
        //region  立即购买
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
        //$scope.productcount={
        //    id: $stateParams.id,
        //    name: $scope.product.Name,
        //    count: $scope.numbers,
        //    mainimg:$scope.product.MainImg,
        //    price:$scope.product.Price,
        //    oldprice:$scope.product.OldPrice,
        //    parameterValue:$scope.product.ParameterValue
        //};

        $scope.buy=function(){
            //$scope.cartinfo.id = $scope.product.Id;
            //$scope.cartinfo.name = $scope.product.Name;
            //$scope.cartinfo.mainimg=$scope.product.MainImg;
            //$scope.cartinfo.price=$scope.product.Price;
            //$scope.cartinfo.oldprice=$scope.product.OldPrice;
            //$scope.cartinfo.parameterValue=$scope.product.ParameterValue;
            //$scope.cartinfo.count = $scope.numbers;
            //$scope.price=$scope.product.Price*$scope.numbers
            $state.go("page.order",{productId: $scope.product.Id,count:$scope.numbers})
        }
        //endregion
        //$scope.AddGWCAction = function () {
        //    //显示图标
        //    var actionDOM = document.getElementById("gwcaction");
        //    actionDOM.style.visibility = "visible";
        //    //执行动画
        //    var abc = actionDOM.className;
        //    actionDOM.className = abc + "Gwcactive";
        //
        //    //执行完毕动画后，隐藏图标
        //    $timeout(show, 1000);
        //    function show() {
        //        actionDOM.className = abc;
        //        actionDOM.style.visibility = "hidden";
        //    }
        //}
    }])
app.controller('SearchProductCtr', ['$http', '$scope', '$stateParams','$timeout',
    function ($http, $scope, $stateParams,$timeout) {
    $scope.search = {
        Page:0,
        PageCount:10,
        Name: $stateParams.productName
    }
    //$http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
    //    params: $scope.search,
    //    'withCredentials': true
    //}).success(function (data) {
    //    $scope.productList = data.List
    //})
    $scope.productList=[];
    $scope.hasmore = true;
    $scope.loadProduct = function () {
        $timeout(function () {
            $scope.search.Page += 1;
            $http.get(SETTING.ApiUrl + "/CommunityProduct/Get", {
                params: $scope.search,
                'withCredentials': true  //跨域
            }).success(function (data) {
                if(data.List.length==0)
                {
                    $scope.hasmore = false;
                }
                for (var i = 0; i < data.List.length; i++) {
                    $scope.productList.push(data.List[i]);
                    if ($scope.productList.length == data.TotalCount) {
                        $scope.hasmore = false;
                    }
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                }
                $scope.$broadcast("scroll.infiniteScrollComplete");
            });
        }, 1000)
    };
}])