/**
 * Created by AddBean on 2015/5/10 0010.
 */
angular.module("app").controller('ProductController', [
    '$http','$scope',function($http,$scope) {
        $scope.img=SETTING.ImgUrl;
        $scope.product = {
            tag: '2213412341235',
            page: 1,
            pageSize: 10,
            totalPage:1
        };
        $scope.rowCollectionProduct=[];
        $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{'withCredentials':true}).success(function (data) {
            $scope.list = data.List;
            $scope.product.Page=data.Condition.Page;
            $scope.product.pageSize=data.Condition.pageSize;
            $scope.totalCount = data.TotalCount;
        });

        $scope.delProduct=function(productId){
            $http.get(SETTING.ApiUrl + '/Product/delProduct?productId='+productId,{'withCredentials':true}).success(function (data) {
                alert(data);
                $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{'withCredentials':true}).success(function (data) {
                    $scope.rowCollectionProduct = data;
                });
            });
        };
    }
]);