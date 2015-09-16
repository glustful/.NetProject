app.controller('TabMeCtrl', function($scope, $ionicSlideBoxDelegate,$ionicModal,$stateParams) {
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
//    新增地址


//    $ionicModal.fromTemplateUrl("my-modal.html", {
//        scope: $scope,
//        animation: "slide-in-up"
//    }).then(function(modal) {
//        $scope.modal = modal;
//    });
//    $scope.openModal = function() {
//        $scope.modal.show();
//    };
//    $scope.closeModal = function() {
//        $scope.modal.hide();
//    };
//    //Cleanup the modal when we are done with it!
//    $scope.$on("$destroy", function() {
//        $scope.modal.remove();
//    });
//    // Execute action on hide modal
//    $scope.$on("modal.hidden", function() {
//        // Execute action
//    });
//    // Execute action on remove modal
//    $scope.$on("modal.removed", function() {
//        // Execute action
//    });
               });


app.controller('selectAddress', function($scope, $stateParams) {

    $scope.chats = [
        {
        id: 0,
        name: '北京市',
        lastText: 'You on your way?',
        face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
    },
        {
            id: 1,
            name: '天津市',
            lastText: 'You on your way?',
            face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
        }
    ];
    alert($scope.chats.name);

    $scope.goBack = function(){
        window.history.go(-1);
    };


});