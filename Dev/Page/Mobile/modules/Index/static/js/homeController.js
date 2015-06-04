/**
 * Created by Administrator on 2015/5/29.
 */
app.controller('homeController',['$http','$scope',function($http,$scope){
    $(".slider").yxMobileSlider({width:640,height:230,during:3000});
    $http.get(SETTING.ApiUrl+'/Brand/GetAllBrand/',{'withCredentials':true}).success(function(data){
            $scope.BrandList=data;

    });

}]);



