
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
$scope.bankcard={

};

    $scope.Crete = function()
    {
        $http.post(SETTING.ApiUrl+'/BankCard/AddBankCard', $scope.bankcard, {'withCredentials': true}).success(function(datas) {
            alert(datas.toString());
        });
    };

}]);

