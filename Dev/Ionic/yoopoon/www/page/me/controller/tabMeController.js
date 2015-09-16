app.controller('TabMeCtrl', function($scope, $ionicSlideBoxDelegate) {
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