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
        // 判断resultStatus 为“9000”则代表支付成功，具体状态码代表含义可参考接口文档
//              if (TextUtils.equals(resultStatus, "9000")) {
//                  
//                  Toast.makeText(cordova.getActivity(), "支付成功",
//                          Toast.LENGTH_SHORT).show();
//              } else {
//                  // 判断resultStatus 为非“9000”则代表可能支付失败
//                  // “8000” 代表支付结果因为支付渠道原因或者系统原因还在等待支付结果确认，最终交易是否成功以服务端异步通知为准（小概率状态）
//                  if (TextUtils.equals(resultStatus, "8000")) {
//                      Toast.makeText(cordova.getActivity(), "支付结果确认中",
//                              Toast.LENGTH_SHORT).show();
//
//                  } else {
//                      Toast.makeText(cordova.getActivity(), "支付失败",
//                              Toast.LENGTH_SHORT).show();
//
//                  }
//              }
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
//  下拉框效果
	$scope.selectBox=false;
	$scope.showSelectBox=function(){
		$scope.selectBox=true;
	}
//	关闭下拉框
	$scope.closeSelectBox=function(){
		$scope.selectBox=false;
	}

    //region地址获取
    $scope.Condition = {
        Page: 1,
        father: true,
        Parent_Id: ''
    };
    $scope.pare = [];

    var getAddress = function () {
        $http.get(SETTING.ApiUrl + '/CommunityArea/Get', {
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data3) {
            if (data3.List != "") {
                $scope.addrss = data3.List;
                $scope.selected = data3.List[0].Id;//如果想要第一个值
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

        Parent_Id: ''
    };

    $scope.area="云南省"
    $scope.click = function (area,id) {
        $scope.area=area;
        //$scope.selectBox=false;
        $scope.SCondition.Parent_Id =id
        $http.get(SETTING.ApiUrl + '/CommunityArea/Get', {
            params: $scope.SCondition,
            'withCredentials': true
        }).success(function (data) {
            $scope.zilei = data.List;
        })

    }
    $scope.huan=function(area){
        $scope.area=area;
        $scope.selectBox=false;
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
            var img=$scope.list.MainImg;
            var imgfb=document.createElement("img");
            document.getElementById("father").appendChild(imgfb);
            imgfb.className="add";
            imgfb.setAttribute("src","http://img.iyookee.cn/"+img);
        });

    };
    getProductList();
    $scope.getList = getProductList;

//    增加到购物车动画
//    var imgfb=document.createElement("img");
////    var imgfb=img.cloneNode(true);
//    document.getElementById("father").appendChild(imgfb);
//    imgfb.className="f-show add";
//    imgfb.setAttribute("ng-src","http://img.iyookee.cn/"+'$scope.list.MainImg');
    $scope.AddCart1 = function (list) {
        $scope.cartinfo.id = $scope.list.Id;
        $scope.cartinfo.name = $scope.list.Name;
        $scope.cartinfo.mainimg = $scope.list.MainImg;
        $scope.cartinfo.price = $scope.list.Price;
        $scope.cartinfo.oldprice = $scope.list.OldPrice;
        $scope.cartinfo.count = 1;
        cartservice.add($scope.cartinfo);
        var $bigImg=$("#bigImg").children();
        var bigImg=$bigImg[0];
        var cloneImg=bigImg.cloneNode(true);
        document.body.appendChild(cloneImg);
   		var top=$bigImg.offset().top;
   		var left=$bigImg.offset().left;
   		cloneImg.style.position="fixed";
   		cloneImg.style.top=top+"px";
   		cloneImg.style.left=left+"px";
   		cloneImg.className="gwcFrist";
   		setTimeout(function(){
   			$(cloneImg).remove();
   		},1000)
    };


//endregion

    //region 商品获取
    $scope.items = [];
    $scope.searchCondition = {
        IsDescending: true,
        OrderBy: 'OrderByAddtime',
        Page: 1,
        PageCount: 4
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
    $scope.loadmore = true;
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
                    if ($scope.items.length == data.TotalCount) {
                        $scope.loadmore = false;
                    }
                }
                $scope.$broadcast("scroll.infiniteScrollComplete");
            });
        }, 1000)
    };


    //endregion

    //region 图片轮播
    $scope.channelName = 'banner';
    $http.get(SETTING.ApiUrl+'/Channel/GetTitleImg', {
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
        count: null,
        price: null,
        oldprice: null,
        parameterValue:[]
    };
    // 添加商品
//  $(window).scroll(function(){
//  	var st=$(this).scrollTop();
//  })
    $scope.AddCart = function (data,$event) {
        $scope.cartinfo.id = data.row.Id;
        $scope.cartinfo.name = data.row.Name;
        $scope.cartinfo.mainimg = data.row.MainImg;
        $scope.cartinfo.price = data.row.Price;
        $scope.cartinfo.oldprice = data.row.OldPrice;
        $scope.cartinfo.count = 1;
        cartservice.add($scope.cartinfo);
        
        var currentObj=$event.target;
		var imgF=currentObj.parentNode.parentNode.parentNode;
		var img=imgF.getElementsByTagName("img");
		var currentImg=img[0];
		var cloneImg=currentImg.cloneNode(false);
		var $cloneImg=$(cloneImg);
		document.body.appendChild(cloneImg);
		var top = $(currentImg).offset().top;
		var left = $(currentImg).offset().left;
	    $cloneImg.css({"position":"fixed","top":top+"px","left":left+"px"});
		cloneImg.className="gwcAnimation";
		setTimeout(function(){
			$cloneImg.remove();
		},1000);
    }

    //endregion

    //region   立即购买
    $scope.buysome=function(id){

        $http.get(SETTING.ApiUrl + '/CommunityProduct/Get?id='+id, {
            'withCredentials': true
        }).success(function (data) {
                $scope.item = data.ProductModel;
            $state.go("page.order",{productcount:$scope.item,pricecount:$scope.item.Price})
        });

    }
    //endregion

    //region  下拉刷新
    $scope.doRefresh = function() {
        window.location.reload();
    }
    //endregion

    $scope.searchname = '';
    document.getElementById('search').onblur = function () {
        $state.go("page.search_product", {productName: $scope.searchname});
    };
$scope.ServiceCon={
    Page:1,
    PageCount:10
}
    $http.get(SETTING.ApiUrl+"/Service/GetList",$scope.ServiceCon,{
        'withCredentials':true
    }).success(function(data)
    {
        $scope.ServiceList=data.List
    })
}]);







