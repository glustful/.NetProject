/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope','$stateParams','$timeout',function($http,$scope,$stateParams,$timeout){

    $scope.go=function(state){
        window.location.href=state;
    }

    //region商品大图获取
    $scope.Condition = {
        IsDescending:true,
        OrderBy:'OrderById',
        IsRecommend:'1'
        //ProductId:''
    };
    var getProductList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.Condition,
            'withCredentials': true
        }).success(function (data1) {
            $scope.list = data1.List[0];
        })
    }
    getProductList();
    $scope.getList=getProductList;
//endregion

    //region 商品获取
    $scope.items = [];
    $scope.searchCondition = {
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        Page:1,
        PageCount:10
        //ProductId:''
    };
    var getList=function() {
        $http.get('http://localhost:50597/api/CommunityProduct/Get', {
            params: $scope.searchCondition,
            'withCredentials': true
        }).success(function (data) {
           // $scope.product = data.List;
          //  items = data.List;
            if(data.List!="") {
                $scope.items = data.List;
            }
        })
    }
    getList();
//endregion

    //region    滚动刷新
    $scope.load_more = function(){
        $timeout(function(){
            $scope.searchCondition.Page+=1;
            $http.get('http://localhost:50597/api/CommunityProduct/Get', {
                params: $scope.searchCondition,
                'withCredentials': true
            }).success(function (data) {
                // $scope.product = data.List;
                //  items = data.List;
                if(data.List!="") {
                    for (var i = 0; i < data.List.length; i++) {
                        $scope.items.push(data.List[i]);
                    }
                }
            })
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },1000);
 };
    //endregion

    //region 轮播图片
    $scope.channelName='banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });
//endregion




   }]);
//endregion




