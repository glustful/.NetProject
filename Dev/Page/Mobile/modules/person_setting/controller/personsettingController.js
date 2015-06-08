

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
            }
        });
    $scope.save = function()
    {
    	
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
    //$scope.user = {
    //    userType: 122,
    //    name: "ggg",
    //    phone: 2445254,
    //    page: 1,
    //    pageSize: 10
    //}
    //$scope.newuser = {
    //    Id:11,
    //    Brokername: "afaf",
    //    phone: 525424,
    //    Sfz:1234567891,
    //    page: 1,
    //    pageSize: 10
    //}
    //$http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{params: $scope.user})
    //    .success(function(response) {$scope.users = response.List[0];
    //    });
    //$scope.save = function()
    //{
    //    $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.newuser)
    //        .success(function(data) {
    //        });
    //}
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
                var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
                img.width  =  128;
                img.height =  128;
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
				//回调函数
				function callback () {
				//将response提取出来分割出文件名
				httpimguri =  xmlhttp.response;
				var g1=httpimguri.split(':"');
				var g2= httpimguri.split(',')[1].split(':"')[1];
				//将分割好的文件名赋予给img全局变量
				httpimguri=g2.substring(0,g2.length-1);
				}
              }
              reader.readAsDataURL(files);
          }
        }
        function clacImgZoomParam( maxWidth, maxHeight, width, height ){
            var param = {top:0, left:0, width:width, height:height};
            if( width>maxWidth || height>maxHeight )
            {
                rateWidth = width / maxWidth;
                rateHeight = height / maxHeight;
                
                if( rateWidth > rateHeight )
                {
                    param.width =  maxWidth;
                    param.height = Math.round(height / rateWidth);
                }else
                {
                    param.width = Math.round(width / rateHeight);
                    param.height = maxHeight;
                }
            }
            
            param.left = Math.round((maxWidth - param.width) / 2);
            param.top = Math.round((maxHeight - param.height) / 2);
            return param;
        }
///////////////////////////头像修改//////////////////////////////////