/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope',function($http,$scope){
    //ҳ����ת
    $scope.go=function(state){
        window.location.href=state;
    }

//���¹���ˢ��
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
            'withCredentials':true  //����
        }).success(function(data){
            $scope.list=data.List;
            $scope.sech.Page=data.Condition.Page;
            $scope.sech.PageCount=data.Condition.PageCount;
            $scope.totalCount = data.TotalCount;
        });
}]);
//start----------------------------商品分类 huangxiuyu2015.09.15-------------------------
app.controller('CategoryController',['$scope','$http',function($scope,$http){

    $scope.searchCondition={
        ifid:0
    }
    $scope.selectCategory=function(ifid){
        $scope.searchCondition.ifid=ifid;
        $http.get(SETTING.ApiUrl+'/Category/GetAllTree/',{params:$scope.searchCondition,'withCredentials': true}).
            success(function(data){
                $scope.list=data;
                console.log(data);
            })
    };
    $scope.selectCategory(1);


}]);
//end----------------------------商品分类 huangxiuyu2015.09.15-------------------------





