/**
 * Created by Administrator on 2015/9/10.
 */

app.controller('productCtr', ['$scope', '$http','$modal', function($scope, $http,$modal) {
    $scope.sech={
        Page:1,
        PageCount:10
    };
        var getProductList=function()
        {
            $http.get(SETTING.ZergWcApiUrl+"/CommunityProduct/Get",{
                params: $scope.sech,
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.list=data.List;
                $scope.sech.Page=data.Condition.Page;
                $scope.sech.PageCount=data.Condition.PageCount;
                $scope.totalCount = data.TotalCount;
            });
        }
    getProductList();
    $scope.getList=getProductList;
    $scope.del=function(id){
        $scope.selectedId = id;
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller:'ModalInstanceCtrl',
            resolve: {
                msg:function(){return "你确定要删除吗？";}
            }
        });
        modalInstance.result.then(function(){
            $http.delete(SETTING.ZergWcApiUrl + '/CommunityProduct/Delete',{
                    params:{
                        id:$scope.selectedId
                    },
                    'withCredentials':true
                }
            ).success(function(data) {
                    if (data.Status) {
                        getProductList();
                    }
                    else{
                        //$scope.Message=data.Msg;
                        $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
        });
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };
    }
}]);
app.controller('editProductCtr',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
    $http.get(SETTING.ZergWcApiUrl+"/Category/Get",{
        'withCredentials':true
    }).success(function (data) {
        $scope.CategoryList=data;
    })
    $http.get(SETTING.ZergWcApiUrl+"/CommunityProduct/Get?id="+$stateParams.id,{
            'withCredentials':true  //跨域
        }).success(function(data){
             $scope.product=data.ProductModel;
        })
    $scope.update= function () {
        if(mainImg.length>0)
        {
            $scope.product.MainImg=mainImg
        }
        if(Img.length>0)
        {
            $scope.product.Img=Img;
        }
        if(Img1.length>0)
        {
            $scope.product.Img1=Img1;
        }
        if(Img2.length>0)
        {
            $scope.product.Img2=Img2;
        }
        if(Img3.length>0)
        {
            $scope.product.Img3=Img3;
        }
        if(Img4.length>0)
        {
            $scope.product.Img4=Img4;
        }
        $http.put(SETTING.ZergWcApiUrl+'/CommunityProduct/Put',$scope.product,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status)
            {
                $state.go("app.product.productList");
            }
        })
    }
}])
app.controller('createProductCtr',['$http','$scope','$state','FileUploader',function($http,$scope,$state,FileUploader){
    $http.get(SETTING.ZergWcApiUrl+"/Category/Get",{
        'withCredentials':true
    }).success(function (data) {
        $scope.CategoryList=data;
    })

      $scope.product={
            CategoryId:'',
            Price :'',
            Name :'',
            Status : '',
            MainImg :'',
            IsRecommend :'',
            Sort :'' ,
            Stock : '',
            Subtitte :'',
            Contactphone :'',
            Detail :'',
            SericeInstruction:'',
            Type:'',
            NewProce:'',
            Img:'',
            Img1:'',
            Img2:'',
            Img3:'',
            Img4:'',
            Ad1:'',
            Ad2:'',
            Ad3:''
        }
    $scope.save=function(){
        $http.post(SETTING.ZergWcApiUrl+"/CommunityProduct/Post",$scope.product,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status)
            {
                $state.go("app.product.productList")
            }
        })
    }
    //上传部分

    $scope.images = [];
    function completeHandler(e) {
        $scope.images.push(e);
        $scope.product.MainImg =$scope.images[0];
        $scope.product.Img=$scope.images[1];
        $scope.product.Img1 =$scope.images[2];
        $scope.product.Img2 =$scope.images[3];
        $scope.product.Img3 =$scope.images[4];
        $scope.product.Img4 =$scope.images[5];

    }

    function errorHandler(e) {
        // console.log(e);
    }

    var uploader = $scope.uploader = new FileUploader({
        url: SETTING.ZergWcApiUrl+'/Resource/Upload',
        'withCredentials':true
    })
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
        completeHandler(response.Msg);
    };
    $scope.deleteImg=function(item){
        item.remove();
        $scope.images.pop();
    }

}])

var mainImg='';var Img='';var Img1='';var Img2='';var Img3='';var Img4=''
//region 上传商品主图
function updatemainImage(file) {
    var div = document.getElementById('main');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=mainimg>';
        var img = document.getElementById('mainimg');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("maintext");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                mainImg=  xmlhttp.response;
                var g1=mainImg.split(':"');
                var g2= mainImg.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量

                mainImg=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
//endregion
//region 上传幅图
function updatevcImg(file) {
    var div = document.getElementById('vc');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=vcimg>';
        var img = document.getElementById('vcimg');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("vctext");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                Img=  xmlhttp.response;
                var g1=Img.split(':"');
                var g2= Img.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                Img=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
//endregion
function updatevcImg1(file) {
    var div = document.getElementById('vc1');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=vcimg1>';
        var img = document.getElementById('vcimg1');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("vc1text");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                Img1=  xmlhttp.response;
                var g3=Img1.split(':"');
                var g4= Img1.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                Img1=g4.substring(0,g4.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}


function updatevcImg2(file) {
    var div = document.getElementById('vc2');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=vcimg2>';
        var img = document.getElementById('vcimg2');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("vc1text2");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                Img2=  xmlhttp.response;
                var g1=Img2.split(':"');
                var g2= Img2.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                Img2=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
function updatevcImg3(file) {
    var div = document.getElementById('vc3');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=vcimg3>';
        var img = document.getElementById('vcimg3');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("vc1text3");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                Img3=  xmlhttp.response;
                var g1=Img3.split(':"');
                var g2= Img3.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                Img3=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
function updatevcImg4(file) {
    var div = document.getElementById('vc4');
    files = file.files[0];
    if (file.files && files) {
        div.innerHTML = '<img style="height: 150px;width: 150px" id=vcimg4>';
        var img = document.getElementById('vcimg4');
        var reader = new FileReader();
        reader.onload = function (evt) {
            //base64编码
            img.src = evt.target.result;
            //扩展名
            var ext = file.value.substring(file.value.lastIndexOf(".") + 1).toLowerCase();
            // gif在ie浏览器不显示
            if (ext != 'png' && ext != 'jpg' && ext != 'jpeg' && ext != 'gif') {
                alert("只支持JPG,PNG,JPEG格式的图片");
                return;
            }
            //发送请求
            var xmlhttp=new XMLHttpRequest();
            xmlhttp.onreadystatechange = callback;
            var fd = new FormData();
            xmlhttp.open("POST",SETTING.ZergWcApiUrl+'/Resource/Upload');
            fd.append("fileToUpload",files);
            xmlhttp.withCredentials = true;
            xmlhttp.send(fd);
            var headtext = document.getElementById("vc1text4");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                Img4=  xmlhttp.response;
                var g1=Img4.split(':"');
                var g2= Img4.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                Img4=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
