/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope','$stateParams','$timeout',function($http,$scope,$stateParams,$timeout){

    $scope.go=function(state){
        window.location.href=state;
    }

    $scope.AddGWCAction = function()
    {
        //显示图标
        var actionDOM = document.getElementById("gwcaction");
        actionDOM.style.visibility = "visible";
        //执行动画
        var abc = actionDOM.className;
        actionDOM.className = abc+"Gwcactive";
        //执行完毕动画后，隐藏图标
        $timeout(show,1000);
        function show()
        {
            actionDOM.style.visibility = "hidden";
        }

    }
    //region鍟嗗搧澶у浘鑾峰彇
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

    //region 鍟嗗搧鑾峰彇
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

    $scope.items = [];
    var base = 0;
    $scope.load_more = function(){
        $timeout(function(){
            for(var i=0;i<10;i++,base++)
                $scope.items.push(["item ",base].join(""));
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },500);
    };

    //region    婊氬姩鍒锋柊
 //   $scope.load_more = function(){
 //       $timeout(function(){
 //           $scope.searchCondition.Page+=1;
 //           $http.get('http://localhost:50597/api/CommunityProduct/Get', {
 //               params: $scope.searchCondition,
 //               'withCredentials': true
 //           }).success(function (data) {
 //               // $scope.product = data.List;
 //               //  items = data.List;
 //               if(data.List!="") {
 //                   for (var i = 0; i < data.List.length; i++) {
 //                       $scope.items.push(data.List[i]);
 //                   }
 //               }
 //           })
 //           $scope.$broadcast("scroll.infiniteScrollComplete");
 //       },1000);
 //};
 //   //endregion
 //
 //   //region 杞挱鍥剧墖
 //   $scope.channelName='banner';
 //   $http.get('http://localhost:50597/api/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
 //       $scope.content=data;
 //   });
//endregion




   }]);
//endregion




