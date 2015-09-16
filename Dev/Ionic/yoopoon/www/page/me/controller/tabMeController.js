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


});app.controller('TabMeCtrl', function($scope, $ionicSlideBoxDelegate) {
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


  //我的订单
  $scope.tabIndex=1;
  $scope.getOrderList1=function(){
    $scope.tabIndex=1
  }
  $scope.getOrderList2=function(){
    $scope.tabIndex=2
  }
  $scope.getOrderList3=function(){
    $scope.tabIndex=3
  }
  $scope.getOrderList4=function(){
    $scope.tabIndex=4
  }

               });


});

/////////////////////////////头像修改////////////////////////////
function previewImage(file)
{
  var MAXWIDTH  = 128;
  var MAXHEIGHT = 128;
  var div = document.getElementById('preview');
  files = file.files[0];
  if (file.files && files)
  {
    div.innerHTML ='<img id=imghead>';
    var img = document.getElementById('imghead');
    img.onload = function(){
      img.width  =  128;
      img.height =  128;
      //隐藏默认头像
      var defaultHeadImg = document.getElementById("preview");
      defaultHeadImg.style.background = 'white';
    }
    var reader = new FileReader();
    reader.onload = function(evt){
      //base64编码
      img.src = evt.target.result;
      //扩展名
      var ext=file.value.substring(file.value.lastIndexOf(".")+1).toLowerCase();
      // gif在ie浏览器不显示
      if(ext!='png'&&ext!='jpg'&&ext!='jpeg'&&ext!='gif'){
        alert("只支持JPG,PNG,JPEG格式的图片");
        return;
      }
      //发送请求
      var xmlhttp=new XMLHttpRequest();
      xmlhttp.onreadystatechange = callback;
      var fd = new FormData();
      xmlhttp.open("POST",SETTING.ApiUrl+'/Resource/Upload');
      fd.append("fileToUpload",files);
      xmlhttp.withCredentials = true;
      xmlhttp.send(fd);
      var headtext = document.getElementById("Uptext");
      headtext.innerHTML = '正在上传..';
      headtext.style.color ='#40AD32'
      //回调函数
      function callback () {
        //将response提取出来分割出文件名
        httpimguri =  xmlhttp.response;
        var g1=httpimguri.split(':"');
        var g2= httpimguri.split(',')[1].split(':"')[1];
        //将分割好的文件名赋予给img全局变量
        httpimguri=g2.substring(0,g2.length-1);
        //图片上传成功字样样式
        headtext.innerHTML = '上传成功!';
        headtext.style.color ='red';
      }
    }
    reader.readAsDataURL(files);
  }
}

///////////////////////////头像修改//////////////////////////////////
