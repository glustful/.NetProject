/////////////////////////////上传头像////////////////////////////
        function previewImage(file)
        {
          var MAXWIDTH  = 128; 
          var MAXHEIGHT = 128;
          var div = document.getElementById('preview');
          if (file.files && file.files[0])
          {
              div.innerHTML ='<img id=imghead>';
              var img = document.getElementById('imghead');
              img.onload = function(){
                var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
                img.width  =  128;
                img.height =  128;
				//img.style.marginLeft = rect.left+'px';
                //img.style.marginTop = rect.top+'px';
              }
              var reader = new FileReader();
              reader.onload = function(evt){img.src = evt.target.result;}
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