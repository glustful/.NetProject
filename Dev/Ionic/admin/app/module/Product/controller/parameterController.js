/**
 * Created by Administrator on 2015/9/11.
 */
app.controller('getParameter',['$http','$scope','$state','$stateParams',function($http,$scope,$stateParams,$state){
   console.log($stateParams);
    $http.get(SETTING.ZergWcApiUrl+"/ProductParameter/Get?categoryId="+$stateParams.params.CategoryId,{
        'withCredentials':true
    }).success(function(data){
        $scope.list=data
    })
    //$http.get(SETTING.ZergWcApiUrl+"/CommunityProduct/Get?id="+$stateParams.params.productId,{
    //    'widthCreadentials':true
    //}).success(function(data){
    //    $scope.product=data.ProductModel;
    //})
    //$scope.hasValue = function(arry,value)
    //{
    //    var has = false;
    //    for(var i = 0;i<arry.length;i++){
    //        if(arry[i].Value ==value)
    //        {
    //            has = true;
    //            break;
    //        }
    //    }
    //    return has;
    //};
    $scope.model={
        productId:$stateParams.params.productId,
        valueIds:[]
    };

    $scope.save= function () {
        for (var i = 0; i < $scope.list.length; i++) {
            $scope.model.valueIds.push($scope.list[i].value);
        }
        $http.put(SETTING.ZergWcApiUrl+"/ProductParameter/Put",$scope.model,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("app.product.productList");
            }
        })
    }

}])