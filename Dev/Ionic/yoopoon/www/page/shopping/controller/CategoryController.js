/**
 * Created by huangxiuyu on 2015/9/15.
 */
//start----------------------------商品分类 huangxiuyu2015.09.15-------------------------
app.controller('CategoryController',['$scope','$http','$state',function($scope,$http,$state){
    //$scope.searchCondition={
    //    ifid:0
    //}
    //$scope.selectCategory=function(ifid){
    //    //alert("fds");
    //    $scope.searchCondition.ifid=ifid;
    //    $http.get(SETTING.ApiUrl+'/Category/GetAllTree/',{params:$scope.searchCondition,'withCredentials':
    //        true}).
    //        success(function(data){
    //            $scope.list=data;
    //            console.log(data);
    //        })
    //};
    //$scope.selectCategory(1);
    //$scope.productName = '';
    //document.getElementById('search').onblur = function () {
    //    $state.go("page.search_product", {productName: $scope.productName});
    //};
    $scope.searchCondition={
        ifid:0
    };
    $scope.selectCategory=function(ifid){
        //alert("fds");
        $scope.searchCondition.ifid=ifid;
        $http.get(SETTING.ApiUrl+'/Category/GetAllTree/',{params:$scope.searchCondition,'withCredentials':
            true}).
            success(function(data){
                $scope.catelist=data;
               // console.log(data);
            })
    };
   // $scope.selectCategory(1);
//    $scope.productName = '';
//    document.getElementById('search').onblur = function () {
//        $state.go("page.search_product", {productName: $scope.productName});
//    };
    $scope.tabIndex= 1 ;
    $scope.category=function(tabIndex){

        $scope.tabIndex=tabIndex;
    }
    $scope.Img=SETTING.ImgUrl;
    //-----------查找商品-----------
    $scope.getProduct=function(){
        $http.get(SETTING.ApiUrl+'/Category/GetCateANDPro',{withCredentials:true}).
            success(function(data){
                $scope.catelist=data;
                $scope.tabIndex=data[0].Id;
            console.log(data);
        });
    }
    $scope.getProduct();
}]);
//end----------------------------商品分类 huangxiuyu2015.09.15-------------------------
