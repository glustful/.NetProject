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


//������
  var comment=document.getElementById("userComment");
  $scope.open=function(){
    comment.style.display="";
  }


  //�ҵĶ���
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

/////////////////////////////ͷ���޸�////////////////////////////
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
      //����Ĭ��ͷ��
      var defaultHeadImg = document.getElementById("preview");
      defaultHeadImg.style.background = 'white';
    }
    var reader = new FileReader();
    reader.onload = function(evt){
      //base64����
      img.src = evt.target.result;
      //��չ��
      var ext=file.value.substring(file.value.lastIndexOf(".")+1).toLowerCase();
      // gif��ie���������ʾ
      if(ext!='png'&&ext!='jpg'&&ext!='jpeg'&&ext!='gif'){
        alert("ֻ֧��JPG,PNG,JPEG��ʽ��ͼƬ");
        return;
      }
      //��������
      var xmlhttp=new XMLHttpRequest();
      xmlhttp.onreadystatechange = callback;
      var fd = new FormData();
      xmlhttp.open("POST",SETTING.ApiUrl+'/Resource/Upload');
      fd.append("fileToUpload",files);
      xmlhttp.withCredentials = true;
      xmlhttp.send(fd);
      var headtext = document.getElementById("Uptext");
      headtext.innerHTML = '�����ϴ�..';
      headtext.style.color ='#40AD32'
      //�ص�����
      function callback () {
        //��response��ȡ�����ָ���ļ���
        httpimguri =  xmlhttp.response;
        var g1=httpimguri.split(':"');
        var g2= httpimguri.split(',')[1].split(':"')[1];
        //���ָ�õ��ļ��������imgȫ�ֱ���
        httpimguri=g2.substring(0,g2.length-1);
        //ͼƬ�ϴ��ɹ�������ʽ
        headtext.innerHTML = '�ϴ��ɹ�!';
        headtext.style.color ='red';
      }
    }
    reader.readAsDataURL(files);
  }
}

///////////////////////////ͷ���޸�//////////////////////////////////