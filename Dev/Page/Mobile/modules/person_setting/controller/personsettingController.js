

/**
 * Created by gaofengming on 2015/5/28.
 */
var httpimguri='';
var app = angular.module("zergApp");
app.controller('personsettingController',['$scope','$http','AuthService',function($scope,$http,AuthService){
    $scope.olduser={
        Brokername:'',
        Realname:'',
        Nickname:'',
        Sexy:'',
        Sfz:'',
        Email:'',
        Phone:'',
        Headphoto:''
    };
    $scope.currentuser= AuthService.CurrentUser();
    $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByUserId?userId='+$scope.currentuser.UserId,{'withCredentials':true})
       .success(function(response) {
            $scope.olduser=response;
            //添加判断,如果用户没有头像,隐藏IMG标签
            if($scope.olduser.Headphoto.length<15){
            	//操作IMG标签的SRC为空
            	var img = document.getElementById('imghead');
            	//没图片隐藏
            	img.style.display = 'none';
            	img.src = "";
            }else{
            	//隐藏默认头像
            	var defaultHeadImg = document.getElementById("preview");
            	defaultHeadImg.style.background = 'white';
            }
        });
    $scope.save = function()
    {
    	if(document.getElementById("Uptext").innerText=='正在上传..'){
    		alert("头像正在上传,请稍等!");
    		return;
    	}
        if(httpimguri.length>0)
        {
            $scope.olduser.Headphoto = SETTING.ImgUrl+httpimguri;
            //如果服务器返回了用户的头像地址,操作IMG标签的SRC为angularjs绑定
            var img = document.getElementById('imghead');
            img.src = "{{olduser.Headphoto}}";
            //有图片就显示
            img.style.display = 'block';
            
        }else{
        	httpimguri='';
        }

        $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.olduser)
            .success(function(data) {
            	var img = document.getElementById('imghead');
            	img.src = $scope.olduser.Headphoto;
            });
    }
}])
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