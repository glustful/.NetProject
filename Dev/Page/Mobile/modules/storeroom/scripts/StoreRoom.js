/**
 * Created by Administrator on 2015/5/29.
 */
app.controller('StormRoomController',['$http','$scope',function($http,$scope){
    $scope.searchCondition={
        AreaName:'',
        TypeId:'',
        PriceBegin:'',
        PriceEnd:''
    };
    $scope.getTypeCondition= function(value){
        $scope.searchCondition.TypeId = value;
        if($scope.type==false)
        {
            $scope.type = !$scope.type;
        }

        $scope.searchProduct();
    }
    $scope.getAreaCondition= function(value){
        $scope.searchCondition.AreaName = value;
        if($scope.province==false)
        {
            $scope.province = !$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city = !$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county = !$scope.county;
        }
        $scope.searchProduct();
    }
    $scope.getPriceCondition= function(priceBegin,priceEnd){
        $scope.searchCondition.PriceBegin =priceBegin;
        $scope.searchCondition.PriceEnd=priceEnd;
        if($scope.price==false)
        {
            $scope.price = !$scope.price;
        }
        $scope.searchProduct();
    }
     $scope.searchProduct=function(){
         $http.get(SETTING.ApiUrl+'/Product/GetSearchProduct',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
             $scope.List =data.List;
             $scope.Count=data.TotalCount;
         })
     };
    $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{'withCredentials':true}).success(function(data){
        $scope.List =data.List;
        $scope.Count=data.TotalCount;
    });
    $http.get(SETTING.ApiUrl + '/Condition/GetCondition',{'withCredentials':true}).success(function(data){
        $scope.Area =data.AreaList;
        $scope.Type=data.TypeList;
    });
    $scope.getCityList=function(id){
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCity =data.AreaList;
        });
        if($scope.city) {
            $scope.city = !$scope.city;
        }
//        if($scope.county==false)
//        {
//            $scope.county=!$scope.county;
//        }
        $scope.AreaCounty=null;
        //$scope.AreaCity=null;
    };
    $scope.getCountyList=function(id){
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCounty =data.AreaList;
        });
        if($scope.county) {
            $scope.county = !$scope.county;
        }
        //$scope.AreaCounty=null;
    };
    $scope.type = true;
    $scope.province=true;
    $scope.city=true;
    $scope.county=true;
    $scope.price=true;
    $scope.toggleProvince=function(){
        $scope.province=!$scope.province;
        if($scope.price==false)
        {
            $scope.price=!$scope.price;
        }
        if($scope.type==false)
        {
            $scope.type=!$scope.type;
        }
        if($scope.city==false)
        {
            $scope.city=!$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county=!$scope.county;
        }
    }
    $scope.toggleType = function() {
        $scope.type = !$scope.type;
        if($scope.price==false)
        {
            $scope.price=!$scope.price;
        }
        if($scope.province==false)
        {
            $scope.province=!$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city=!$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county=!$scope.county;
        }
    };
    $scope.togglePrice=function(){
        $scope.price=!$scope.price;
        if($scope.type==false)
        {
            $scope.type=!$scope.type;
        }
        if($scope.province==false)
        {
            $scope.province=!$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city=!$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county=!$scope.county;
        }
    };
}])