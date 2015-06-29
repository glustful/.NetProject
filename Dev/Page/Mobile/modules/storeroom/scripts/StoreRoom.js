/**
 * Created by Administrator on 2015/5/29.
 */
app.controller('StormRoomController',['$http','$scope','$timeout',function($http,$scope,$timeout){
//    ----------------------------------------------轮播------------------------------------------------

    $scope.channelName='banner';
    $http.get(SETTING.ApiUrl+'/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });
/*----------------------------------------------动态加载-------------------------------------------*/
    $scope.searchCondition={
        AreaName:'',
        TypeId:'',
        PriceBegin:'',
        PriceEnd:'',
        Page:0,
        PageCount:10,
        OrderBy:'OrderByAddtime',
        IsDescending:true
    };
    $scope.tipp="正在加载......";
    var loading = false
        ,pages=2;                      //判断是否正在读取内容的变量
    $scope.List = [];//保存从服务器查来的任务，可累加
    var pushContent= function() {                    //核心是这个函数，向$scope.posts
        //添加内容
//        $scope.List=[];
//        $scope.searchCondition.Page=0;
       // pages=2;
        if (!loading && $scope.searchCondition.Page < pages) {                         //如果页面没有正在读取
            loading = true;                     //告知正在读取
            $http.get(SETTING.ApiUrl+'/Product/GetSearchProduct',{params:$scope.searchCondition,'withCredentials':true}).success(function(data) {
                    pages =Math.ceil(data.TotalCount /$scope.searchCondition.PageCount);
                    for (var i = 0; i <= data.List.length - 1; i++) {
                        $scope.List.push(data.List[i]);
                    }
                    loading = false;            //告知读取结束
                    $scope.tipp="加载更多......";
                    if ($scope.List.length == data.TotalCount) {//如果所有数据已查出来
                        $scope.tipp = "已经是最后一页了";
                    }
                    $scope.Count=data.TotalCount;
            });
            $scope.searchCondition.Page++;                             //翻页
        }
//        else {
//            $scope.tipp = "已经是最后一页了";
//        }
    };
    pushContent();
    //$scope.more=pushContent;
    function pushContentMore(){

       if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
       {
          pushContent();//if判断有没有滑动到底部，到了加载
       }
        $timeout(pushContentMore, 2000);//定时器，每隔一秒循环调用自身函数
    }
    pushContentMore();//触发里面的定时器
    /*----------------------------------------------动态加载-------------------------------------------*/

    $scope.type = true;
    $scope.province=true;
    $scope.city=true;
    $scope.county=true;
    $scope.price=true;
    $scope.selectCity=false;
    $scope.selectCounty=false;
    $scope.selectArea='区域';
    $scope.selectType='类型';
    $scope.selectPrice='价格';
    //条件重置
    $scope.Reset=function(){
        $scope.searchCondition.AreaName='';
        $scope.searchCondition.TypeId='';
        $scope.searchCondition.PriceBegin='';
        $scope.searchCondition.PriceEnd='';
        $scope.List=[];
        $scope.searchCondition.Page=0;
        $scope.selectArea='区域';
        $scope.selectType='类型';
        $scope.selectPrice='价格';
        $scope.tipp="正在加载......";
        pages=2;
        pushContent();
    }
    //点击户型条件
    $scope.getTypeCondition= function(value,typeName){
        $scope.searchCondition.TypeId = value;
        $scope.selectType=typeName;
        if($scope.type==false)
        {
            $scope.type = !$scope.type;
        }
        //$scope.searchProduct();
       $scope.List=[];
       $scope.searchCondition.Page=0;
        $scope.tipp="正在加载......";
        pages=2;
        pushContent();
    }
    //点击地区条件
    $scope.getAreaCondition= function(value){
        $scope.searchCondition.AreaName = value;
        $scope.selectArea=value;
        $scope.province = !$scope.province;
        $scope.city = !$scope.city;
        $scope.county = !$scope.county;
        //$scope.searchProduct();
        $scope.tipp="正在加载......";
        $scope.List=[];
        $scope.searchCondition.Page=0;
        pages=2;
        pushContent();
    }
    //点击价格条件
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
        //$scope.searchProduct();
        $scope.tipp="正在加载......";
        $scope.List=[];
        $scope.searchCondition.Page=0;
        pages=2;
        pushContent();
    }
     //获取省份和户型
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
        $scope.selectCity=true;
        $scope.selectCounty=false;
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
        $scope.selectCounty=true;
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