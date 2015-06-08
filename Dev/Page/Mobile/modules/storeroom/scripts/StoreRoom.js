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
    $scope.type = true;
    $scope.province=true;
    $scope.city=true;
    $scope.county=true;
    $scope.price=true;
    $scope.selectArea='区域';
    $scope.selectType='类型';
    $scope.selectPrice='价格';
    //条件重置
    $scope.Reset=function(){
        $scope.searchCondition.AreaName='';
        $scope.searchCondition.TypeId='';
        $scope.searchCondition.PriceBegin='';
        $scope.searchCondition.PriceEnd='';
        $scope.searchProduct();
        $scope.selectArea='区域';
        $scope.selectType='类型';
        $scope.selectPrice='价格';
    }
    //获取户型条件
    $scope.getTypeCondition= function(value,typeName){
        $scope.searchCondition.TypeId = value;
        $scope.selectType=typeName;
        if($scope.type==false)
        {
            $scope.type = !$scope.type;
        }
        $scope.searchProduct();
    }
    //获取地区条件
    $scope.getAreaCondition= function(value){
        $scope.searchCondition.AreaName = value;
        $scope.selectArea=value;
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
    //获取价格条件
    $scope.getPriceCondition= function(priceBegin,priceEnd){
        $scope.searchCondition.PriceBegin =priceBegin;
        $scope.searchCondition.PriceEnd=priceEnd;
        if(priceBegin==0)
        {
            $scope.selectPrice='4000以下';
        }
        else if(priceBegin==10000)
        {
            $scope.selectPrice='10000以上'
        }
        else{
            $scope.selectPrice=priceBegin+'-'+priceEnd;
        }

        if($scope.price==false)
        {
            $scope.price = !$scope.price;
        }
        $scope.searchProduct();
    }
    //根据条件获取product
     $scope.searchProduct=function(){
         $http.get(SETTING.ApiUrl+'/Product/GetSearchProduct',{
             params:$scope.searchCondition,
             'withCredentials':true
         })
             .success(function(data){
             $scope.List =data.List;
             $scope.Count=data.TotalCount;
         })
     };
    $scope.searchProduct();
    //获取所有商品
//    $http.get(SETTING.ApiUrl + '/Product/GetSearchProduct',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
//        $scope.List =data.List;
//        $scope.Count=data.TotalCount;
//    });
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition',{'withCredentials':true}).success(function(data){
            $scope.Area =data.AreaList;
            $scope.Type=data.TypeList;
        });
    //获取省对应的市
    $scope.getCityList=function(id,row){
        $scope.selectedRow = row;
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCity =data.AreaList;
        });
        if($scope.city) {
            $scope.city = !$scope.city;
        }
        $scope.AreaCity=null;
        $scope.AreaCounty=null;
    };
    //获取市对应的区县
    $scope.getCountyList=function(id,row){
        $scope.selectedRow1 = row;
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCounty =data.AreaList;
        });
        if($scope.county) {
            $scope.county = !$scope.county;
        }
    };
//展开地区
    $scope.toggleProvince=function(){
        $scope.province=!$scope.province;
        $scope.city=!$scope.city;
        $scope.county=!$scope.county;
        if($scope.price==false)
        {
            $scope.price=!$scope.price;
        }
        if($scope.type==false)
        {
            $scope.type=!$scope.type;
        }
    }
    //展开户型
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
    //展开价格
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