/**
 * Created by AddBean on 2015/5/10 0010.
 */
angular.module("app").controller('ProductController', [
    '$http','$scope',function($http,$scope) {
        $scope.product = {
            tag: '2213412341235',
            page: 1,
            pageSize: 10,
            totalPage:1
        };
        $scope.rowCollectionProduct=[];
        $http.get(SETTING.TradingApiUrl + '/Product/GetAllProduct').success(function (data) {
            $scope.rowCollectionProduct = data;
        });

        $scope.delProduct=function(productId){
            $http.get(SETTING.TradingApiUrl + '/Product/delProduct?productId='+productId).success(function (data) {
                alert(data);
                $http.get(SETTING.TradingApiUrl + '/Product/GetAllProduct').success(function (data) {
                    $scope.rowCollectionProduct = data;
                });
            });
        };
    }
]);