'use strict';

/* Controllers */
// signin controller
app.controller('testController', ['$scope', '$http', '$state', function($scope, $http, $state) {
    $scope.gettest=function(){
        $http.get("http://192.168.1.199:9013/API/OtherCoupons/GetList",{
//            params:$scope.models,   //参数
            'withCredentials':true  //跨域
        }).success(function(data){
            console.log(data);
            $scope.list=data;
        });
    };
    $scope.gettest();
}])
;