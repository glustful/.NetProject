var httpimguri="";
app.controller('TabMeCtrl', function($scope,$http,$state,AuthService,orderService,$ionicSlideBoxDelegate,$ionicModal,$stateParams) {

    $scope.model = {
        activeIndex: 0
    };

    $scope.pageClick = function (index) {
        //alert(index);
        //alert($scope.delegateHandler.currentIndex());
        $scope.model.activeIndex = 2;
    };

    $scope.slideHasChanged = function ($index) {
        //alert($index);
        //alert($scope.model.activeIndex);
    };
    $scope.delegateHandler = $ionicSlideBoxDelegate;

    $scope.go = function (state) {
        window.location.href = state;
    }



    //获取当前用户信息
    $scope.currentuser= AuthService.CurrentUser();
    //我的订单
    $scope.tabIndex = 1;
    $scope.getOrderList = function (tabIndex) {
        $scope.tabIndex = tabIndex;
    };
    function tab() {
        //待付款
        if ($stateParams.tabIndex == 1) {
            $scope.tabIndex = 1;
            $scope.serchCondition={
                Status:'1',
                CustomerName: $scope.currentuser.UserName
            }
            $scope.myOrder = orderService.getOrderList($scope.searchCondition)
        }
        //待发货
        if ($stateParams.tabIndex == 2) {
            $scope.tabIndex = 2;
            $scope.serchCondition={
                CustomerName: $scope.currentuser.UserName,
                Status:'2'
            }
            $scope.myOrder = orderService.getOrderList($scope.searchCondition);
        }
        //待收货
        if ($stateParams.tabIndex == 3) {
            $scope.tabIndex = 3;
            $scope.serchCondition={
                CustomerName: $scope.currentuser.UserName,
                Status:'3'
            }
            $scope.myOrder = orderService.getOrderList($scope.searchCondition);
        }
        //待评价
        if ($stateParams.tabIndex == 4) {
            $scope.tabIndex = 4;
        }
    }
    tab();
    //个人资料修改
    $scope.imgUrl = SETTING.ImgUrl;
    $scope.oldMem = {
        Realname: '',
        UserName:'',
        Gender: '1',
        IdentityNo: '4564',
        Icq: '454',
        Phone: '18388026186',
        Thumbnail: '',
        PostNo: '456',
        AccountNumber: '4444',
        Points: '5',
        Level: '4',
        AddTime: '2015-08-09',
        UpdUser: '1',
        UpdTime: '2015-08-09'
    };

    $http.get(SETTING.ApiUrl+'/Member/Get?userId='+$scope.currentuser.UserId,{'withCredentials':true})
        .success(function(response) {
            $scope.oldMem=response;
            console.log($scope.oldMem);
            //添加判断,如果用户没有头像,隐藏IMG标签
            if($scope.oldMem.Thumbnail.length<15){
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

    $scope.save = function () {

        if (httpimguri.length > 0) {
            $scope.oldMem.Thumbnail = httpimguri;
            //如果服务器返回了用户的头像地址,操作IMG标签的SRC为angularjs绑定
            var img = document.getElementById('imghead');
            img.src = "{{oldMem.Thumbnail}}";
            //有图片就显示
            img.style.display = 'block';
        } else {
            httpimguri = '';
        }
        $http.post(SETTING.ApiUrl + '/Member/Post', $scope.oldMem, {'withCredentials': true})
            .success(function (data) {
                if (data.Status) {
                    var img = document.getElementById('imghead');
                    img.src = $scope.oldMem.Thumbnail;
                    location.reload([true]);
                    $state.go("app.me");
                }
            });
    }

})
app.controller('selectAddress', function($scope,$routeParams) {

    $scope.model = {
        activeIndex: 0
    };

    $scope.pageClick = function (index) {
        $scope.model.activeIndex = 2;
    };

    $scope.slideHasChanged = function ($index) {

    };
    $scope.delegateHandler = $ionicSlideBoxDelegate;

//打开评论
    var comment = document.getElementById("userComment");
    $scope.open = function () {
        comment.style.display = "";
    }

})


/////////////////////////////头像修改////////////////////////////
function previewImage(file)
{
    var MAXWIDTH  = 80;
    var MAXHEIGHT = 80;
    var div = document.getElementById('preview');
    files = file.files[0];
    if (file.files && files)
    {
        div.innerHTML ='<img id=imghead>';
        var img = document.getElementById('imghead');
        img.onload = function(){
            img.width  =  80;
            img.height =  80;
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
            //var headtext = document.getElementById("Uptext");
            //headtext.innerHTML = '正在上传..';
            //headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                if (xmlhttp.readyState == 4) {
                    //将response提取出来分割出文件名
                    httpimguri =  xmlhttp.response;
                    var g1=httpimguri.split(':"');
                    var g2= httpimguri.split(',')[1].split(':"')[1];
                    //将分割好的文件名赋予给img全局变量
                    httpimguri=g2.substring(0,g2.length-1);
                    //图片上传成功字样样式
                    //headtext.innerHTML = '上传成功!';
                    //headtext.style.color ='red';
                }
            }
        }
        reader.readAsDataURL(files);
    }
}
///////////////////////////头像修改//////////////////////////////////
