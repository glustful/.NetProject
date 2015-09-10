/**
 * Created by Yunjoy on 2015/8/4.
 */

/* Controllers */
// home controller
app.controller('contactController', ['$scope', '$http', '$state', function($scope, $http, $state) {
    $scope.GetFocusRes=function(){
        $http.get(SETTING.ZergWcApiUrl+"/AutoRes/GetFocusRes",{
            'withCredentials':true  //øÁ”Ú
        }).success(function(data){
            $scope.content=data.Content;
        });
    };
    $scope.GetFocusRes();

    $scope.focusEditStatus=false;
    $scope.EditFocusRes=function(){
        $http.put(SETTING.ZergWcApiUrl+"/AutoRes/PutFocusRes",{content:$scope.content},{
            'withCredentials':true  //øÁ”Ú
        }).success(function(data){
            $scope.reStatus=data.Status;
            $scope.reMsg=data.Msg;
        });
    };
}]);