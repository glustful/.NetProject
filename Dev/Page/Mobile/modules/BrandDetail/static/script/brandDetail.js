/**
 * Created by Administrator on 2015/7/9.
 */
app.controller('BrandDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl+'/Brand/GetBrandDetail?brandId='+$stateParams.brandId,{'withCredentials':true}).success(function(data) {
        $scope.brandDetail=data
    })
}])
app.filter('trustHtml', function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    }
})