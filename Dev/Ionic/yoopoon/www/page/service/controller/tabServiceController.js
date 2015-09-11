app.controller('TabServiceCtrl', function($scope, $ionicSlideBoxDelegate) {
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
    //    页面跳转
    $scope.go=function(state){
        window.location.href=state;
    }

//    搜索功能
    $scope.showSelect=false;
    $scope.isShow=false;
    $scope.showInput=function(){
        $scope.showSelect=true;
        $scope.isShow=true;
        alert($scope.isShow)



    }
               });