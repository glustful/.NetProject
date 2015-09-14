app.controller('TabCarCtrl', function($scope, $ionicSlideBoxDelegate) {
  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //
  $scope.chats = [{
    id: 0,
    name: 'Ben Sparrow',
    lastText: 'You on your way?',
    face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
  }, {
    id: 1,
    name: 'Max Lynx',
    lastText: 'Hey, it\'s me',
    face: 'https://avatars3.githubusercontent.com/u/11214?v=3&s=460'
  }, {
    id: 2,
    name: 'Adam Bradleyson',
    lastText: 'I should buy a boat',
    face: 'https://pbs.twimg.com/profile_images/479090794058379264/84TKj_qa.jpeg'
  }, {
    id: 3,
    name: 'Perry Governor',
    lastText: 'Look at my mukluks!',
    face: 'https://pbs.twimg.com/profile_images/598205061232103424/3j5HUXMY.png'
  }, {
    id: 4,
    name: 'Mike Harrington',
    lastText: 'This is wicked good ice cream.',
    face: 'https://pbs.twimg.com/profile_images/578237281384841216/R3ae1n61.png'
  }];

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

    ////全选按钮功能
    $scope.start=false;
    $scope.allButton=false;
    $scope.all=function(){
//alert($scope.start)

        if($scope.allButton==false){
            $scope.start=false;
//            alert("nihao")
        }if($scope.allButton==true){
            $scope.start=true;
//            alert($scope.start)
        }
    }
///数量增加减
    $scope.number=0;
    $scope.adding=function(){
       
        if($scope.number>=0){
            $scope.number=$scope.number+1;
//            alert($scope.number)
        }
        else{
            $scope.number=0;
        }
    }
    $scope.decrease=function(){
        if($scope.number>=0){
            $scope.number=$scope.number-1;
//            alert($scope.number)
        }
        else{
            $scope.number=0;
        }
}
//编辑
    $scope.flag={showDelete:false,showReorder:false};
    $scope.items=["Chinese","English","German","Italian","Janapese","Sweden","Koeran","Russian","French"];
    $scope.delete_item=function(item){
        var idx = $scope.items.indexOf(item);
        $scope.items.splice(idx,1);
    };
    $scope.move_item = function(item, fromIndex, toIndex) {
        $scope.items.splice(fromIndex, 1);
        $scope.items.splice(toIndex, 0, item);
    };
               });

app.controller('CarDetailCtrl', function($scope, $stateParams) {
  
  $scope.chat = {
    id: 0,
    name: 'Ben Sparrow',
    lastText: 'You on your way?',
    face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
  };
  

  $scope.goBack = function(){
    
    window.history.go(-1);
  };

  
});