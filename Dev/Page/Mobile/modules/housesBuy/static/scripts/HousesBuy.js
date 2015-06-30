/**
 * Created by Administrator on 2015/6/3.
 */
app.controller('HousesBuyController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl+'/Product/GetProductsByBrand?BrandId='+$stateParams.BrandId,{'withCredentials':true}).success(function (data) {
        $scope.ProductList=data.productList;

    })


    $http.get(SETTING.ApiUrl+'/Brand/GetByBrandId?BrandId='+$stateParams.BrandId,{'withCredentials':true}).success(function (data1) {
        $scope.brand=data1;


    })



}]);

//app.filter('asdasd', function ($sce) {
//
//    return function (input) {
//
//        return $sce.trustAsHtml(input);
//
//    }
//
//});