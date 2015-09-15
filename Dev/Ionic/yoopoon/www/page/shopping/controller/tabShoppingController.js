/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope',function($http,$scope){
    //页面跳转
    $scope.go=function(state){
        window.location.href=state;
    }

//向下滚动刷新
    $scope.items = [];
    var base = 0;
    $scope.load_more = function(){
        $timeout(function(){
            for(var i=0;i<10;i++,base++)
                $scope.items.push(["item ",base].join(""));
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },500);
    };
    $scope.sech={
        Page:1,
        PageCount:10,
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        CategoryId:1
    };
        $http.get(SETTING.ApiUrl+"/CommunityProduct/Get",{
            params: $scope.sech,
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.list=data.List;
            $scope.sech.Page=data.Condition.Page;
            $scope.sech.PageCount=data.Condition.PageCount;
            $scope.totalCount = data.TotalCount;
        });
}]);





