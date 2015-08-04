/**
 * Created by Administrator on 2015/6/2.
 */
app.controller('HousesPicController',['$http','$scope','$stateParams', function ($http,$scope,$stateParams) {
     $http.get(SETTING.ApiUrl+'/Product/GetProductById?productId='+$stateParams.productId,{'withCredentials':true}).success(
         function(data){
             $scope.ProductDetail=data;
             $scope.select=false;
             if($scope.ProductDetail.Productimg1==null)
             {
                 $scope.select=true;
             }
             for(var i= 0;i<=data.ParameterValue.length;i++)
             {
                 if(data.ParameterValue[i].ParameterString=="户型")
                 {
                     $scope.type= data.ParameterValue[i].Value;
                 }
             }
         }
     )

    }]);

app.filter('trustHtml', ['$sce',function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    }
}])