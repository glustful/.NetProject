/**
 * Created by AddBean on 2015/5/10 0010.
 */
app.controller('BrandController', ['$scope', '$http', '$state', function ($scope, $http, $state) {
    //初始化界面；
    $scope.rowCollectionBasic = [];
    $scope.rowCollectionParameter = [];
    $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0).success(function (data) {
        $scope.rowCollectionBasic = data;
    });

    //下一页；
    $scope.getAllBrand = function (pageIndex) {
        $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0).success(function (data) {
            $scope.rowCollectionBasic = data;
        });
    };

    //删除该项目；
    $scope.delBrand = function (brandId) {
        $http.get(SETTING.ApiUrl + '/Brand/DelBrandById?brandId=' + brandId).success(function (data) {
            $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0).success(function (data) {
                $scope.rowCollectionBasic = data;
            });
            return $scope.output = data;
        });
    };

    //添加项目
    $scope.brandName = "";
    $scope.iconPath = "";
    $scope.addBrand = function () {
        var brand = {
            Bname: $scope.brandName,
            Bimg: $scope.imgUrl
        };
        var Json = JSON.stringify(brand);
        $http.post(SETTING.ApiUrl + '/Brand/AddProductBrand', Json, {
            'withCredentials': true
        }).success(function (data) {
            WindowClose();
            $http.get(SETTING.ApiUrl + '/Brand/GetAllBrand?pageindex=' + 0).success(function (data) {
                $scope.rowCollectionBasic = data;
            });
            $scope.output = data;
        });
    };

    //根据项目查找项目参数值；
    $scope.selectBrandId=0;
    $scope.getBrandParameter = function (seletId) {
        $scope.selectBrandId=seletId;
       $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + seletId).success(function (data) {
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
            Parametervaule:  $scope.brandParameterValue,
            Adduser: 2,
            Upduser: 'jiadou'
        };
        var Json = JSON.stringify(brand);
        $http.post(SETTING.ApiUrl + '/Brand/AddProductBrandParameter', Json, {
            'withCredentials': true
        }).success(function (data) {
            AddParameterWindowClose();
            $http.get(SETTING.TradingApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' + $scope.selectBrandId).success(function (data) {
                $scope.rowCollectionParameter = data;
            });
            $scope.output = data;
        });
    };

    //删除项目参数值
    $scope.delBrandParameter=function (brandParameterId) {
        $http.get(SETTING.ApiUrl + '/Brand/DelBrandParameter?brandParameterId=' + brandParameterId).success(function (data) {
            $http.get(SETTING.ApiUrl + '/Brand/GetBrandParameterByBrand?ProductBrandId=' +  $scope.selectBrandId).success(function (data) {
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
