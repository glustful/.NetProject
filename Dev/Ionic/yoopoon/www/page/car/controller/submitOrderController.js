/**
 * Created by Yunjoy on 2015/9/21.
 */
app.controller('submitOrderController', ['$http','$scope','repository', '$stateParams','AuthService', '$state','cartservice', 'orderService',
    function($http,$scope,repository, $stateParams, AuthService,$state,cart, orderService) {
        $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
        if( $scope.currentuser==undefined ||  $scope.currentuser=="")
        {
            $state.go("page.login");//调到登录页面
        }
        $scope.count=$stateParams.count;
        $scope.productId=$stateParams.productId;
        if ($stateParams.productId!=null && $stateParams.count!=null) {
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
            //$scope.model = data;
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
        $scope.mcon={
            UserId:$scope.currentuser.UserId
        };
        if($stateParams.memberaddreid)
        {
            $http.get(SETTING.ApiUrl+"/MemberAddress/Get?id="+$stateParams.memberaddreid,{
                'withCredentials':true
            }).success(function(data){
                console.log(data);
                $scope.userinfo = data;
            });
        }
        else{
        $http.get(SETTING.ApiUrl+"/MemberAddress/Get",{
            params:$scope.mcon,
        'withCredentials':true
         }).success(function(data){
            $scope.userinfo = data.List[0];
        });
        }
//        $scope.searchCondition={
//            Adduser:$scope.currentuser.UserId
//        };
//        $scope.getAddress=function(){
//            $http.get(SETTING.ApiUrl+'/MemberAddress/Get/',{params:$scope.searchCondition,'withCredentials':true}).
//                success(function(data){
//
//                    $scope.userinfo=data.List[0];
//                    console.log(data);
//                })};
//        $scope.getAddress();
        //todo:完成生成订单并付款的逻辑
        //		$scope.submit = function () {
        //			alert("111");
        //		};

    }
]);