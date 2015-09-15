/**
 * Created by Administrator on 2015/9/7.
 */

app.controller('TabShoppingCtrl',['$http','$scope','$timeout',function($http,$scope,$timeout){
    //ҳ����ת
    $scope.go=function(state){
        window.location.href=state;
    }

//���¹���ˢ��

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




    $scope.items = [];
    $scope.searchCondition = {
        IsDescending:true,
        OrderBy:'OrderByAddtime',
        Page:1,
        PageCount:6
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


//    滚动刷新
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
                    if($scope.items.length==data.TotalCount){
                        $scope.$broadcast("scroll.infiniteScrollComplete");
                    }
                }
            })
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },1000);
    };


    $scope.channelName='banner';
    $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    });

}]);





