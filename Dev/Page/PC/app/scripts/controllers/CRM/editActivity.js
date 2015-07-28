/**
 * Created by Yunjoy on 2015/7/28.
 */
app.controller('EditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.activity =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '',$scope.activity,{
        }).success(function(data){
            if(data.Status){
                $state.go("");
            }else{
                alert(data.Msg);
            }
        });
    }

}]);