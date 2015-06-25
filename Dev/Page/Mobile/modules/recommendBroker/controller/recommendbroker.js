
/**
 * Created by lhl on 2015/5/28.
 */

app.controller('recommendBrokerController',['$scope','$http','AuthService',function($scope,$http,AuthService){

    $scope.currentuser= AuthService.CurrentUser();
    console.log($scope.currentuser);
    $http.get(SETTING.ApiUrl+'/RecommendAgent/GetRecommendAgentListByUserId',{params:{userId:$scope.currentuser.UserId},'withCredentials': true}).success(function(response) {
        if(response==""){
            $scope.Tips="您还未添加经纪人！"
        }
        else{
            $scope.List = response;

        }
        });


}])

//
//app.controller('recommendBrokerAddController',['$http','$scope','$state',function($http,$scope,$state){
//    $scope.YZM={
//        phone:'',
//        SmsType:'6'
//    }
//    $scope.Crete = function()
//    {
//        $http.post(SETTING.ApiUrl+'/SMS/SendSMS', $scope.YZM, {'withCredentials': true}).success(function(datas) {
//            if (data.Message=="1")
//            {
//               alert("邀请成功");
//            }
//     });
//    };
//
//}]);
//
