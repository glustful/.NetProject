/**
 * Created by Yunjoy on 2015/8/3.
 */

/* Controllers */
// home controller
app.controller('homeController', ['$scope', '$http',
    function($scope, $http) {
    $scope.GetFocusRes=function(){
        $http.get(SETTING.ZergWcApiUrl+"/AutoRes/GetFocusRes",{
            params:{portalId:$scope.activePortalId},  //参数
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.content=data.Content;
        });
    };
    $scope.GetFocusRes();

    $scope.focusEditStatus=false;

        $scope.infoModel={
            Content:$scope.content,
            PortalId:$scope.activePortalId
        };

    $scope.EditFocusRes=function(){
        $scope.infoModel.Content=$scope.content;
        $http.put(SETTING.ZergWcApiUrl+"/AutoRes/PutFocusRes",$scope.infoModel,{
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.reStatus=data.Status;
            $scope.reMsg=data.Msg;
        });
    };
}]);