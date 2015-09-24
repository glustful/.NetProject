/**
 * Created by Yunjoy on 2015/9/21.
 */
app.controller('submitOrderController', ['$http','$scope','repository', '$stateParams', 'cartservice', 'orderService',
    function($http,$scope,repository, $stateParams, cart, orderService) {
        if ($stateParams.productId && $stateParams.count) {
            $http.get(SETTING.ApiUrl+"/CommunityProduct/Get?id="+$stateParams.productId,{
                'withCredentails':true
            }).success(function(data){
                    $scope.model = [{
                        id: data.ProductModel.Id,
                        price: data.ProductModel.Price,
                        name: data.ProductModel.Name,
                        oldprice: data.ProductModel.OldPrice,
                        mainimg: data.ProductModel.MainImg,
                        count: $stateParams.count
                    }]
                $scope.CountPrice=data.ProductModel.Price*$stateParams.count
            })
        } else {
            $scope.model = data;
        }

        //TODO:完成地址绑定及选择
        //获取收货人信息
       // var url = 'http://localhost:50597/API/MemberAddress/Get?memberId=2';
        //$scope.userinfo = {
        //    name:null,
        //    phone:null,
        //    address:null
        //};
        //$http.get(url,{'withCredentials':true})
        //    .success(function(data) {
        //        $scope.userinfo = data;
        //    });
        $http.get(SETTING.ApiUrl+"/MemberAddress/Get?memberId="+5,{
        'withCredentials':true
         }).success(function(data){
            $scope.userinfo = data;
        })
        //todo:完成生成订单并付款的逻辑
        //		$scope.submit = function () {
        //			alert("111");
        //		};

    }
]);