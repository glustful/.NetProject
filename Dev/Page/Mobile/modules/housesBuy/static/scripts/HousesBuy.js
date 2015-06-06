/**
 * Created by Administrator on 2015/6/3.
 */
app.controller('HousesBuyController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl+'/Product/GetProductsByBrand?BrandId='+$stateParams.BrandId).success(function (data) {
        $scope.ProductList=data.productList;
        $scope.tent=data.content;
    })
}]);



