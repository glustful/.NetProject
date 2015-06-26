/**
 * Created by gaofengming on 2015/6/25.
 */
app.controller('personal_userController',['$scope','$http','AuthService',function($scope,$http,AuthService){
    $scope.user={
        Brokername:'',
        Headphoto:''
    };
    $scope.currentuser=AuthService.CurrentUser();
    $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByUserId?userId='+$scope.currentuser.UserId,{'withCredentials':true}).success(function(data){
     $scope.user=data
    });

}])