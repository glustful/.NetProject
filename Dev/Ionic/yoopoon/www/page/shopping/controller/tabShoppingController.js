/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope','$ionicLoading',function($http,$scope,$ionicLoading){
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

//ÃÃ²ÃÃ‚Â¹Ã¶Â¶Â¯Ã‹Â¢ÃÃ‚
    $scope.items = [];
    var base = 0;
    $scope.load_more = function(){
        $timeout(function(){
            for(var i=0;i<10;i++,base++)
                $scope.items.push(["item ",base].join(""));
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },500);
    };
    $scope.sech={
        Page:1,
        PageCount:10,
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        CategoryId:1
    };
        $http.get(SETTING.ApiUrl+"/CommunityProduct/Get",{
            params: $scope.sech,
            'withCredentials':true  //¿çÓò
        }).success(function(data){
            $scope.list=data.List;
            $scope.sech.Page=data.Condition.Page;
            $scope.sech.PageCount=data.Condition.PageCount;
            $scope.totalCount = data.TotalCount;
        });
}]);





