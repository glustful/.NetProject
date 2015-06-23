/** create by 杨波 2015.6.5 创富英雄榜**/
app.controller('BrokerTopThreeController',['$scope','$http','AuthService','$state',function($scope,$http,AuthService,$state){
    var BrokerTopThree=function() {
        $http.get(SETTING.ApiUrl + '/BrokerInfo/OrderByBrokerTopThree', {'withCredentials': true}).success(function (data) {
//           $scope.ii=0;
//           for(var i=0;i<data.List.length-1;i++)
//            {
//                $scope.list.push(data.List[i]);
//                $scope.ii=i+1;
//            }
//           console.log($scope.li);
           $scope.list = data.List;
            console.log($scope.list);
        })

    };
    BrokerTopThree();

    //获取推荐楼房信息
    var getRecProduct=function() {
        $http.get(SETTING.ApiUrl + '/Brand/GetOneBrand', {params: $scope.searchCondition, 'withCredentials': true}).success(function (data) {
            $scope.List = data.List;
        });
    };
    getRecProduct();


    //经纪人专区图片轮播
    $scope.channelName='banner';
    $http.get(SETTING.ApiUrl+'/Channel/GetTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.content=data;
    })
    //经纪人专区活动图片轮播
    $scope.channelName='活动';
    $http.get(SETTING.ApiUrl+'/Channel/GetActiveTitleImg',{params:{ChannelName:$scope.channelName},'withCredentials':true}).success(function(data){
        $scope.Actcontent=data;
    });
}]);