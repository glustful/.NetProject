/**
 * Created by Administrator on 2015/6/18.
 */
var seascapeimgurl='';var bigimgurl='';var smallimgurl='';
angular.module("app").controller('EditProductController', [
    '$http', '$scope','$state','$stateParams','FileUploader', function ($http, $scope,$state,$stateParams,FileUploader) {
        //选择商品分类；
        var classifys = $scope.classifys = {};
        $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify',{'withCredentials':true}).success(function (data) {
            classifys.optionsData = data;
        });
        $scope.classifyMsg = "";
        $scope.classifyMsg1 = "";

      //  $scope.selectParameterValue=[];

        $scope.selectclassifyChange = function ( type,acreage,area) {
            $http.get(SETTING.ApiUrl + '/Classify/GetNextNodesById?nodeId=' + $scope.product.ClassId,{'withCredentials':true}).success(function (dataRes) {
                if (dataRes == "获取失败") {
                    $scope.classifyMsg1 = "选择完成！";
                    $http.get(SETTING.ApiUrl + '/Classify/GetParameterTreeData?classifyId=' + $scope.product.ClassId,{'withCredentials':true}).success(function (data) {
                        $scope.parameterValueList = data;

                       $scope.selectParameterValue=type;
                       // $scope.selectParameterValue.push(type);
                       // $scope.selectParameterValue.push(acreage);
                       // $scope.selectParameterValue.push(area);
                        //$scope.selectParameterValue[0]=type;
                        //$scope.selectParameterValue[1]=acreage;
                        //$scope.selectParameterValue[2]=area;


                    });
                } else {
                    $scope.classifyMsg1 = "";
                    $http.get(SETTING.ApiUrl + '/Classify/GetClassifyNameById?classifyId=' + $scope.product.ClassId,{'withCredentials':true}).success(function (data) {
                        classifys.optionsData = dataRes;
                        $scope.classifyMsg = $scope.classifyMsg + data + "->";
                    });
                }
            });
        };

        $scope.hasValue = function(arry,value)
        {
            var has = false;
            for(var i = 0;i<arry.length;i++){
                if(arry[i].Value ==value)
                {
                    has = true;
                    break;
                }
            }
            return has;
        };


        var uploader = $scope.uploader = new FileUploader({
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        })
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            $scope.product.Productimg=response.Msg;
        };

        // 获取品牌列表
        $http.get(SETTING.ApiUrl + '/Brand/GetBrandList',{'withCredentials':true}).success(function (data) {
            $scope.BrandList = data;
        });
        //获取商品信息
        $http.get(SETTING.ApiUrl+'/Product/GetProductById?productId='+$stateParams.productId).success(function(data)
        {
           $scope.product=data;
            $scope.selectclassifyChange(data.Type,data.acreage,data.area);
            // $scope.parameterValueList
              //type,acreage,area

                 //$scope.selectParameterValue[0]=data.Type;
                 //$scope.selectParameterValue[1]=data.acreage;
                 //$scope.selectParameterValue[2]=data.area;


            function SetChecked(type,acreage,area,data) {
                var a=document.getElementsByName(data[0].Name);
                for (var i = 0; i < a.length; i++) {
                    if (a[i].value=type) {
                        a[i].checked=checked;
                    }
                }
            }



        });
        $scope.selectParameterValue = [];
        //编辑商品信息
         $scope.update=function(){
             if(seascapeimgurl.length>0)
             {
                 $scope.product.Productimg1=seascapeimgurl;
             }
             if(bigimgurl.length>0)
             {
                 $scope.product.ProductDetailImg=bigimgurl;
             }
             if(smallimgurl.length>0)
             {
                 $scope.product.Productimg=smallimgurl;
             }
            var newproduct = {
            Id:$scope.product.Id,
            ClassifyId:$scope.product.ClassId,
            ProductBrandId:$scope.product.BrandId,
            Price:$scope.product.Price,
            Productname:$scope.product.Productname,
            RecCommission:$scope.product.RecCommission,
            Commission:$scope.product.Commission,
            ContactPhone:$scope.product.Phone,
            Dealcommission:$scope.product.Dealcommission,
            Status:$scope.product.Status==0?true:false,
            Recommend:$scope.product.Recommend== 0 ? true : false,
            Stockrule:$scope.product.Stockrule,
            SubTitle:$scope.product.SubTitle,
            Productimg:$scope.product.Productimg,
            //Productdetail:$scope.product.ProductDetailed,
            Sericeinstruction:$scope.product.Sericeinstruction
            };
            var newproductDetail = {
                Productdetail:$scope.product.ProductDetailed,
                Sericeinstruction:$scope.product.Sericeinstruction,
                Productimg1:$scope.product.Productimg1,
                ProductImg:$scope.product.ProductDetailImg
            };
            var classifyJson = JSON.stringify({ product: newproduct, productDetail: newproductDetail });
             $http.post(SETTING.ApiUrl + '/Product/EditProduct',classifyJson,{
                 'withCredentials':true
             }).success(function(data){
                 if(data.Status){
                     valueClick();
                     for (var i = 0; i < $scope.selectParameterValue.length; i++) {
                         $http.get(SETTING.ApiUrl + '/Classify/UpdateProductParameterVaule?parameterValueId=' + $scope.selectParameterValue[i] + "&productId=" + $scope.product.Id, {'withCredentials': true}).success(function (data) {

                         }).success(function(){
                             if(data.Status)
                             {
                                 $state.go("page.Trading.product.product");
                             }
                         });
                     }
                 }
                 else{
                     $scope.alerts=[{type:'danger',msg:data.Msg}];
                 }
             });
         };
        function valueClick() {
            $scope.selectParameterValue = [];
            for (var i = 0; i < $scope.parameterValueList.length; i++) {
                $scope.selectParameterValue.push(getRadioBoxValue($scope.parameterValueList[i].Name));
            }
            //alert($scope.selectParameterValue);
        }

        function getRadioBoxValue(radioName) {
            var obj = document.getElementsByName(radioName);  //这个是以标签的name来取控件
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].checked) {
                    return   obj[i].value;
                }
            }
            return "undefined";
        }
    }]);
/**
 * 房源库图片修改 start
 */
/**
 * 上传小户型图
 * @param file
 */
function updatesmallImage(file)
{
    var div = document.getElementById('small');
    files = file.files[0];
    if (file.files && files)
    {
        div.innerHTML ='<img style="height: 200px;width: 200px" id=smallimg>';
        var img = document.getElementById('smallimg');
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
            var headtext = document.getElementById("smalltext");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                smallimgurl=  xmlhttp.response;
                var g1=smallimgurl.split(':"');
                var g2= smallimgurl.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                smallimgurl=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
/**
 * 上传大户型图
 * @param file
 */
function updatebigImg(file)
{
    var div = document.getElementById('big');
    files = file.files[0];
    if (file.files && files)
    {
        div.innerHTML ='<img style="height: 200px;width: 200px" id=bigimg>';
        var img = document.getElementById('bigimg');
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
            var headtext = document.getElementById("bigtext");
            headtext.innerHTML = '正在上传..';
            headtext.style.color ='#40AD32'
            //回调函数
            function callback () {
                //将response提取出来分割出文件名
                bigimgurl=  xmlhttp.response;
                var g1=bigimgurl.split(':"');
                var g2= bigimgurl.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                bigimgurl=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}
/**
 * 上传海景图
 * @param file
 */
function seascapeimg(file)
{
    var div = document.getElementById('Seascape');
    files = file.files[0];
    if (file.files && files)
    {
        div.innerHTML ='<img style="height: 200px;width: 200px" id=Seascapeimg>';
        var img = document.getElementById('Seascapeimg');
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
                seascapeimgurl =  xmlhttp.response;
                var g1=seascapeimgurl.split(':"');
                var g2= seascapeimgurl.split(',')[1].split(':"')[1];
                //将分割好的文件名赋予给img全局变量
                seascapeimgurl=g2.substring(0,g2.length-1);
                //图片上传成功字样样式
                headtext.innerHTML = '上传成功!';
                headtext.style.color ='red';
            }
        }
        reader.readAsDataURL(files);
    }
}

