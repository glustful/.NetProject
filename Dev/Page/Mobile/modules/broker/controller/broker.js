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
    //获取推荐楼盘信息 create by chenda
    var getAllProdct=function(){
        $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{'withCredentials':true}).success(function(product){
            $scope.List = product.List;
            console.log($scope.List)
        })
    };
    //判断是否登录
   if(AuthService.IsAuthenticated) {
       getAllProdct();
   }else{
       $state.go("user.login");
   }
}]);