/**
 * Created by Administrator on 2015/6/3.
 */
app.controller('HousesController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl+'/Product/GetProductsByBrand?BrandId='+$stateParams.BrandId,{'withCredentials':true}).success(function (data) {
        $scope.ProductList=data.productList;
        $scope.tent=data.content;
    })
    //
    //
    //$http.get(SETTING.ApiUrl+'/Brand/GetByBrandId?BrandId='+$stateParams.BrandId,{'withCredentials':true}).success(function (data1) {
    //    $scope.brand=data1;
    //
    //})

    $http.get(SETTING.ApiUrl+'/Product/GetProductById?ProductId='+$stateParams.ProductId,{'withCredentials':true}).success(function (data) {
        $scope.List=data;
    })
}]);



