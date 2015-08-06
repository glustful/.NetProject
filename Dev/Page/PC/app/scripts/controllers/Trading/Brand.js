/**
 * Created by AddBean on 2015/5/10 0010.
 */

angular.module("app").controller('BrandListController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.img=SETTING.ImgUrl;
        $scope.searchCondition = {
            page: 1,
            pageSize: 10,
            className:'',
            orderByAll:"OrderByAddtime",//排序
            isDes:true//升序or降序
           // className:'房地产'
        };
        $scope.UpOrDownImgClass="fa-caret-down";
        //--------------------------------------------获取项目列表----------------------------------------------//
        $scope.getList  = function(orderByAll) {
           //$scope.searchCondition.orderByAll=orderByAll ;
           // alert($scope.searchCondition.isDes);
            if(orderByAll!=undefined) {
                $scope.searchCondition.orderByAll=orderByAll ;
                if ($scope.searchCondition.isDes == true)//如果为降序，
                {
                    $scope.UpOrDownImgClass = "fa-caret-up";//改变成升序图标
                    $scope.searchCondition.isDes = false;//则变成升序
                }
                else if ($scope.searchCondition.isDes == false) {
                    $scope.UpOrDownImgClass = "fa-caret-down";
                    $scope.searchCondition.isDes = true;
                }
            }
            $http.get(SETTING.ApiUrl+'/Brand/GetAllBrand',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;

            });
        };
        $scope.getList();

        //---------------------------------------------删除单个项目----------------------------------------------//
        $scope.open=function(brandId){
            $scope.selectedId=brandId;
            var modalInstance=$modal.open({
                templateUrl:'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve:{
                    msg:function(){
                        return "你确定要删除吗？";
                    }
                }
            });

            modalInstance.result.then(function () {
                $http.get(SETTING.ApiUrl+'/Brand/DelBrandById',{
                    params:{
                        brandId:$scope.selectedId
                    },
                    'withCredentials':true
                }).success(function(data){
                    if(data.Status){
                        $scope.getList();//成功刷新列表
                    }
                    else{
                        $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                })
            });
        };
        $scope.closeAlert = function(Brand) {
            $scope.alerts.splice(Brand, 1);
        };
    }

]);

//----------------------------------------新增品牌----------------------------------------------//
angular.module("app").controller('CreatBrandController', [
    '$http', '$scope', '$state','FileUploader', function ($http, $scope, $state,FileUploader) {

        //--------添加项目 start---------//
        $scope.BrandModel={
            Bname:"",
            Bimg:"",
            SubTitle:"",
            Content:"",
            ClassId:""

        };
        //------------------------------------------------获取品牌类别----------------------------------------------//
        //-------by   yangyue   2015/7/7------获取类别信息--------
        $http.get(SETTING.ApiUrl + '/Classify/GetClassList',{'withCredentials':true}).success(function (data) {
            $scope.ClassList=data;
        });
        //-----------------end----------
        $scope.Save = function(){
            document.getElementById("btnok").setAttribute("disabled", true);
            $scope.BrandModel.Bimg=$scope.image;
            $http.post(SETTING.ApiUrl + '/Brand/AddProductBrand',$scope.BrandModel,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    document.getElementById("btnok").removeAttribute("disabled");
                    $state.go('page.Trading.product.brand');

                }else{
                    document.getElementById("btnok").removeAttribute("disabled");
                    $scope.alerts=[{type:'danger',msg:data.Msg}];


                }
            });
        }
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
            $scope.BrandModel.Bname=''
        };


        //---------------------------------------------图片上传 start------------------------------------//
        $scope.image="";
        $scope.SImg=SETTING.ImgUrl;
        function completeHandler(e) {
            $scope.image=(e);
        }

        function errorHandler(e) {
            // console.log(e);
        }

        var uploader = $scope.uploader = new FileUploader({
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        });
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            completeHandler(response.Msg);
        };
        //-------------------------------------------图片上传 end------------------------------------------//



    }




]);



app.controller('BrandController', ['$scope', '$http', '$state','$modal','FileUploader', function ($scope, $http, $state,$modal,FileUploader) {
//    //初始化界面；
//    $scope.rowCollectionBasic = [];
//    $scope.rowCollectionParameter = [];
//    $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0,{'withCredentials':true}).success(function (data) {
//        $scope.rowCollectionBasic = data;
//    });
//
//    //下一页；
//    $scope.getAllBrand = function (pageIndex) {
//        $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0,{'withCredentials':true}).success(function (data) {
//            $scope.rowCollectionBasic = data;
//        });
//    };

    //添加项目
//    $scope.brandName = "";
//    $scope.iconPath = "";
//    $scope.addBrand = function () {
//        var brand = {
//            Bname: $scope.brandName,
//            Bimg: $scope.imgUrl
//        };
//        var Json = JSON.stringify(brand);
//        $http.post(SETTING.ApiUrl + '/Brand/AddProductBrand', Json, {
//            'withCredentials': true
//        }).success(function (data) {
//            if(data.Status)
//            {
//                $state.go('page.Trading.product.brand')
//            }
//            else{
//                alert(["添加失败！"])
//
//            }
//            $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0,{'withCredentials':true}).success(function (data) {
//                $scope.rowCollectionBasic = data;
//            });
//            $scope.output = data;



//        });
//    };


    //根据项目查找项目参数值；
    $scope.selectBrandId=0;
    $scope.getBrandParameter = function (seletId) {
        $scope.selectBrandId=seletId;
       $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + seletId,{'withCredentials':true}).success(function (data) {
            $scope.rowCollectionParameter = data;

        });
    };

    //添加项目参数值；
    $scope.brandParameterName="";
    $scope.brandParameterValue="";
    $scope.addBrandParameter = function () {
        var brand = {
            ProductBrandId:  $scope.selectBrandId,
            Parametername: $scope.brandParameterName,
            Parametervaule:  $scope.brandParameterValue

        };
        var Json = JSON.stringify(brand);
        $http.post(SETTING.ApiUrl + '/Brand/AddProductBrandParameter', Json, {
            'withCredentials': true
        }).success(function (data) {
            AddParameterWindowClose();//------by  yangyue   2015/7/8---关闭当前弹出层----end----
            $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + $scope.selectBrandId,{'withCredentials':true}).success(function (data) {
                $scope.rowCollectionParameter = data;

            });
            return $scope.output = data;
        });
    };
    $scope.closeAlert = function(Brand) {
        $scope.alerts.splice(Brand, 1);
    };
    var uploader = $scope.uploader = new FileUploader({
        url: SETTING.ApiUrl+'/Resource/Upload',
        'withCredentials':true
    });
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
        //completeHandler(response.Msg);
        $scope.brandParameterValue=response.Msg
    };

    //------------by-----yangyue------2015/7/7--star---删除项目参数值
    $scope.delBrandParameter=function(brandParameterId) {
        $scope.selectedId = brandParameterId;
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            resolve: {
                msg: function () {
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(function () {
            $http.get(SETTING.ApiUrl + '/Brand/DelBrandParameter?brandParameterId=' + brandParameterId, {'withCredentials': true}).success(function (data) {
                $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + $scope.selectBrandId, {'withCredentials': true}).success(function (data) {
                    $scope.rowCollectionParameter = data;
                });

                $scope.alerts = [{type: 'danger', msg: data.Msg}];

            });
        });
    }

//-----------------------------------end-----------------------------------
    //上传部分
    $(':file').change(function(){
        var file = this.files[0];
        name = file.name;
        size = file.size;
        type = file.type;
        //your validation
        var formData = new FormData($('form')[0]);
        $.ajax({
            url: SETTING.ApiUrl+ '/Resource/Upload',  //server script to process data
            type: 'POST',
            xhr: function() {  // custom xhr
                myXhr = $.ajaxSettings.xhr();
                if(myXhr.upload){ // check if upload property exists
                    myXhr.upload.addEventListener('progress',progressHandlingFunction, false); // for handling the progress of the upload
                }
                return myXhr;
            },
            //Ajax事件
            //beforeSend: beforeSendHandler,
            success: completeHandler,
            error: errorHandler,
            // Form数据
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    });



    function completeHandler(e){
        $scope.imgUrl="http://img.yoopoon.com/"+e.Msg;
        document.getElementById("Brandimg").src=$scope.imgUrl;
        //alert( $scope.imgUrl);
    }
    function errorHandler(e){
       // console.log(e);
    }
    function progressHandlingFunction(e){
        if(e.lengthComputable){
            $('progress').attr({value:e.loaded,max:e.total});
        }
    }
}]);