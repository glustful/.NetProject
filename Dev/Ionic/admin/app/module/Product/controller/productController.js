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
        MainImg : '',
        IsRecommend :'',
        Sort :'' ,
        Stock : '',
        Subtitte :'',
        Contactphone :'',
        Detail :'',
        SericeInstruction:''
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

//            $scope.images.push("http://img.yoopoon.com/"  +e);
//            $scope.Productimg = "http://img.yoopoon.com/" +  strs[0];
//            $scope.Productimg1 = "http://img.yoopoon.com/" +  strs[1];
//            $scope.Productimg2 = "http://img.yoopoon.com/" +  strs[2];
//            $scope.Productimg3 = "http://img.yoopoon.com/" +  strs[3];
//            $scope.Productimg4 = "http://img.yoopoon.com/" +  strs[4];
        $scope.images.push(e);
        $scope.MainImg =$scope.images[0];
        $scope.Img=$scope.images[1];
        $scope.Img1 =$scope.images[2];
        $scope.Img2 =$scope.images[3];
        $scope.Img3 =$scope.images[4];
        $scope.Img4 =$scope.images[5];
//
//            $scope.imgUrl = "http://img.yoopoon.com/" + e.Msg;
    }

    function errorHandler(e) {
        // console.log(e);
    }

    function progressHandlingFunction(e) {
        if (e.lengthComputable) {
            $('progress').attr({value: e.loaded, max: e.total});
        }
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

