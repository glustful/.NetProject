/**
 * Created by Administrator on 2015/6/2.
 */
app.controller('HousesPicController',['$http','$scope','$stateParams', function ($http,$scope,$stateParams) {
     $http.get(SETTING.ApiUrl+'/Product/GetProductById?productId='+$stateParams.productId,{'withCredentials':true}).success(
         function(data){
             $scope.ProductDetail=data;
         }
     )
}]);