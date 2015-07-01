/**
 * Created by Administrator on 2015/6/18.
 */
angular.module("app").controller('BrandEditController', [
    '$http','$state','$scope','$stateParams','FileUploader',function($http,$state,$scope,$stateParams,FileUploader) {
        $http.get(SETTING.ApiUrl+'/Brand/GetByBrandId?BrandId='+$stateParams.brandId,{
            'withCredentials':true
        }).success(function(data){
            $scope.list = data;

        });

        var uploader = $scope.uploader = new FileUploader({
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        });
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            $scope.list.Bimg=response.Msg;
        };


//-------------------------------------------------修改品牌信息----------------------------------------

        $scope.save = function(){

    document.getElementById("btnok").setAttribute("disabled", true);

    $http.post(SETTING.ApiUrl + '/Brand/UpProductBrand',$scope.list,{
        'withCredentials':true
    }).success(function(data){
        if(data.Status){
            document.getElementById("btnok").removeAttribute("disabled");
            $state.go("page.Trading.product.brand");

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



//----------------------------------------------图片上传-------------------------------------
$scope.image="";
$scope.SImg=SETTING.ImgUrl;
function completeHandler(e) {
    $scope.image=(e);
}

function errorHandler(e) {
    // console.log(e);
}

 }])

//------------------------------------------------end--------------------------------------------