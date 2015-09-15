app.controller('TabMeCtrl','$http', function($scope,$http, $ionicSlideBoxDelegate) {
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


//打开评论
  var comment=document.getElementById("userComment");
  $scope.open=function(){
    comment.style.display="";
  }

  //获取当前会员信息
  var getMemById = function(){
    $http.get(SETTING.ZergWcApiUrl+"/Member/Get?id="+$stateParams.id,{
      'withCredentials':true
    }).success(function(data){
      $scope.memModels=data;
    });
  }
  getMemById();


});

