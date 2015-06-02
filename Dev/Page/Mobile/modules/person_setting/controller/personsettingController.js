

/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('personsettingController',function($scope,$http){
	
    $scope.user = {
        userType: 122,
        name: "ggg",
        phone: 2445254,
        page: 1,
        pageSize: 10
    }
    $scope.newuser = {
        Id:11,
        Brokername: "afaf",
        phone: 525424,
        Sfz:1234567891,
        page: 1,
        pageSize: 10
    }
    $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{params: $scope.user})
        .success(function(response) {$scope.users = response.List[0];

        });
    $scope.save = function()
    {
        $http.post(SETTING.ApiUrl+'/BrokerInfo/UpdateBroker', $scope.newuser)
            .success(function(data) {
            });
    }
})
/////////////////////////////上传头像////////////////////////////
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
              	//扩展名
              	var ext=file.value.substring(file.value.lastIndexOf(".")+1).toLowerCase();
              	// gif在IE浏览器暂时无法显示
			     if(ext!='png'&&ext!='jpg'&&ext!='jpeg'&&ext!='gif'){
			         alert("图片的格式必须为png或者jpg或者jpeg格式！"); 
			         return;
			     }
              	img.src = evt.target.result;
				var xmlhttp=new XMLHttpRequest();
				xmlhttp.onreadystatechange = callback;
				var fd = new FormData();  
				xmlhttp.open("POST",SETTING.ApiUrl+'/Resource/Upload');
				//var boundary ="---------------------------32404670520626"; 
				fd.append("fileToUpload", files);
				//xmlhttp.charset="utf-8";
				//xmlhttp.setRequestHeader("Content-Type","multipart/form-data");
				xmlhttp.send(fd);
				function callback () {
					//0未初始化，1正在加载，2已经加载，3交互中，4完成
					if(xmlhttp.readyState==4)
					{
						
					}
				}
              }
              reader.readAsDataURL(file.files[0]);
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
///////////////////////////结束//////////////////////////////////