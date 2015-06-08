

/**
 * Created by gaofengming on 2015/5/28.
 */
var httpimguri;
app.controller('personsettingController',function($scope,$http){
	
    $scope.user = {
        userType:0,
        name: "昵称1",
        phone: 111111,
        page: 1,
        pageSize: 10,
        Headphoto:''
    }
    $scope.newuser = {
        Id:11,
        Brokername: "afaf",
        phone: 525424,
        Sfz:1234567891,
        page: 1,
        pageSize: 10,
        Headphoto:''
    }
    $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{params: $scope.user})
        .success(function(response) {$scope.users = response.List[0];

        });
    $scope.save = function()
    {
    	if(httpimguri.length>15)
    	{
    		$scope.newuser.Headphoto = SETTING.ImgUrl+httpimguri;
    	}
        $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.newuser)
            .success(function(data) {
            });
    }
})
 /////////////////////////////头像修改////////////////////////////
        function previewImage(file)
        {
          var MAXWIDTH  = 128; 
          var MAXHEIGHT = 128;
          var div =document.getElementById('preview');
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
				function callback () {
				httpimguri =  xmlhttp.response;
				var g1=httpimguri.split(':"');
				var g2= httpimguri.split(',')[1].split(':"')[1];
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