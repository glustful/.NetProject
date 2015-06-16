/**
 * Created by AddBean on 2015/5/10 0010.
 */

angular.module("app").controller('BrandListController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.img=SETTING.ImgUrl;
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };

        //--------------------------------------------获取项目列表----------------------------------------------//
        $scope.getList  = function() {
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
            Content:""

        };

        $scope.Save = function(){
            $scope.BrandModel.Bimg=$scope.image;
            $http.post(SETTING.ApiUrl + '/Brand/AddProductBrand',$scope.BrandModel,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){

                    $state.go('page.Trading.product.brand');
                    console.log(data);
                }else{
                    console.log("error");
                }
            });
        }


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



app.controller('BrandController', ['$scope', '$http', '$state', function ($scope, $http, $state) {
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
            AddParameterWindowClose();
            $http.get(SETTING.TradingApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + $scope.selectBrandId,{'withCredentials':true}).success(function (data) {
                $scope.rowCollectionParameter = data;
            });
            $scope.output = data;
        });
    };

    //删除项目参数值
    $scope.delBrandParameter=function (brandParameterId) {
        $http.get(SETTING.ApiUrl + '/Brand/DelBrandParameter?brandParameterId=' + brandParameterId,{'withCredentials':true}).success(function (data) {
            $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' +  $scope.selectBrandId,{'withCredentials':true}).success(function (data) {
                $scope.rowCollectionParameter = data;
            });
            return $scope.output = data;
        });
    };


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
app.filter('dateFilter',function(){
    return function(date){
        return FormatDate(date);
    }
})
function FormatDate(JSONDateString) {
    jsondate = JSONDateString.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    return date.getFullYear()
        + "-"
        + month
        + "-"
        + currentDate
        + "-"
        + date.getHours()
        + ":"
        + date.getMinutes()
        + ":"
        + date.getSeconds()
        ;

}