/**
 * Created by AddBean on 2015/5/10 0010.
 */
angular.module("app").controller('ProductController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.img=SETTING.ImgUrl;
        $scope.product = {
            //tag: '2213412341235',
            page: 1,
            pageSize: 10,
            totalPage:1
        };
        $scope.rowCollectionProduct=[];
        var getProductList=function(){$http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{params:$scope.product,'withCredentials':true}).success(function (data) {
            $scope.list = data.List;
            $scope.product.page=data.Condition.Page;
            $scope.product.pageSize=data.Condition.PageCount;
            $scope.totalCount = data.TotalCount;
        })};
        $scope.getList=getProductList;
        getProductList();

        $scope.del = function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要删除吗？";}
                }
            });
            modalInstance.result.then(function(){
                $http.get(SETTING.ApiUrl + '//Product/delProduct',{
                        params:{
                            productId:$scope.selectedId
                        },
                        'withCredentials':true
                    }
                ).success(function(data) {
                        if (data.Status) {
                            getProductList();
                        }
                    });
            })
        }
//        $scope.delProduct=function(productId){
//            $http.get(SETTING.ApiUrl + '/Product/delProduct?productId='+productId,{'withCredentials':true}).success(function (data) {
//                alert(data);
//                $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{'withCredentials':true}).success(function (data) {
//                    $scope.rowCollectionProduct = data;
//                });
//            });
//        };
    }
]);




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
//        + "-"
//        + date.getHours()
//        + ":"
//        + date.getMinutes()
//        + ":"
//        + date.getSeconds()
        ;

}


