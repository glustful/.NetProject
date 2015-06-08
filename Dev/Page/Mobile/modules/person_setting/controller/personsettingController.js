

/**
 * Created by gaofengming on 2015/5/28.
 */
var app = angular.module("zergApp");
app.controller('personsettingController',['$scope','$http','AuthService',function($scope,$http,AuthService){
    $scope.olduser={
        Brokername:'',
        Realname:'',
        Nickname:'',
        Sexy:'',
        Sfz:'',
        Email:'',
        Phone:''
    };
    $scope.currentuser= AuthService.CurrentUser();
    $http.get(SETTING.ApiUrl+'/BrokerInfo/GetBrokerByUserId?userId='+$scope.currentuser.UserId,{'withCredentials':true})
       .success(function(response) {
            console.log(response);
            $scope.olduser=response
        });
    $scope.save = function()
    {
        $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.olduser)
            .success(function(data) {
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
				console.log(img.src);
                xmlhttp.withCredentials = true;
				xmlhttp.send(fd);
				function callback () {
				//1：请求已经建立，但是还没有发送（还没有调用 send()）。
				//2：请求已发送，正在处理中（通常现在可以从响应中获取内容头）。
				//3：请求在处理中；通常响应中已有部分数据可用了，但是服务器还没有完成响应的生成。
				//4：响应已完成；您可以获取并使用服务器的响应了。
					if(xmlhttp.readyState==4)
					{
						
					}
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