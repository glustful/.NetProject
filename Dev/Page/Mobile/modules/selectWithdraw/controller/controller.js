/**
 * Created by chen on 2015/7/13.
 */
app.controller('withdrawController',['$http','$scope','AuthService',function($http,$scope,AuthService) {
    $scope.Withdraw={
      UserId:""
    };


    $scope.currentUser=AuthService.CurrentUser();
    $scope.Withdraw.UserId = $scope.currentUser.UserId;

    var getwithdrawResult  = function() {
        $http.get(SETTING.ApiUrl+'/BrokeAccount/GetBrokeAccountByUserId/',{params:$scope.Withdraw,'withCredentials':true}).success(function(data){
            $scope.list=data.List;
        });
    };
    getwithdrawResult();
    ///
    $scope.Ids=[];
    $scope.toggle = function (Id) {
        $scope.Ids.push (Id);
    }

}])