
/**
 * Created by lhl on 2015/5/28.
 */
app.controller('myPurseController',function($scope,$http){

    $scope.user = {
        userId:2
    }
    $http.get(SETTING.ApiUrl+'/BankCard/SearchBankCardsByUserID',{params: $scope.user}).success(function(response) {
        $scope.List = response;
    });


})


app.controller('bankAddController',['$http','$scope','$state',function($http,$scope,$state){

    $http.get(SETTING.ApiUrl+'/Bank/SearchAllBank').success(function(response) {
        $scope.BankList = response.List;
    });

     $scope.bankcard={
         Num:"",
         Type:"储蓄卡",
         Address:""

    };
    $scope.Crete = function()
    {
        $http.post(SETTING.ApiUrl+'/BankCard/AddBankCard', $scope.bankcard, {'withCredentials': true}).success(function(datas) {
            alert(datas.toString());
        });
    };

}]);
