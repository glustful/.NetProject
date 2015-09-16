app.controller('TabMeCtrl',['$http','$scope', function($http,$scope, $ionicSlideBoxDelegate) {
  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //
  //$scope.$on('$ionicView.enter', function(e) {
  //});
  $scope.model = {
    activeIndex:0
  };
  
  $scope.pageClick = function(index){
    //alert(index);
    //alert($scope.delegateHandler.currentIndex());
    $scope.model.activeIndex = 2;
  };

  $scope.slideHasChanged = function($index){
    //alert($index);
    //alert($scope.model.activeIndex);
  };
  $scope.delegateHandler = $ionicSlideBoxDelegate;

                }]);

//start--------------------------地址管理 huangxiuyu 2015.09.15--------------------------
app.controller('addressAdm',['$http','$scope',function($http,$scope, $ionicSlideBoxDelegate) {
    $scope.searchCondition={
        Adduser:3
    }
    $scope.getAddress=function(){
        $http.get(SETTING.ApiUrl+'/MemberAddress/Get/',{params:$scope.searchCondition,'withCredentials':true}).
            success(function(data){
                $scope.list=data.List;
                console.log(data);
            })};
    $scope.getAddress();

    //删除地址
    $scope.delCondition={
        id:0
    }
    $scope.delAddress=function(id){
        $scope.delCondition.id=1;
$http.delete(SETTING.ApiUrl+'/MemberAddress/Delete/',{params:$scope.delCondition,withCredentials:true}).
    success(function(data){
        alert(data.Msg);
    });
    }
}]);
//end--------------------------地址管理 huangxiuyu 2015.09.15--------------------------
