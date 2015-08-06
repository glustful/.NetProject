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
            totalPage:1,
            orderByAll:"OrderByAddtime",//排序
            isDes:true//升序or降序
        };
        var iniImg=function(){
            $scope.OrderByPrice="footable-sort-indicator";
            $scope.OrderByRecCommission="footable-sort-indicator";
            $scope.OrderByCommission="footable-sort-indicator";
            $scope.OrderByDealcommission="footable-sort-indicator";
            $scope.OrderByStockRule="footable-sort-indicator";
            $scope.OrderByAddtime="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderByAddtime="fa-caret-down";//升降序图标
        $scope.rowCollectionProduct=[];
        var getProductList=function(orderByAll){
            if(orderByAll!=undefined){
                $scope.product.orderByAll=orderByAll ;
                if($scope.product.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.product.isDes=false;//则变成升序
                }
                else if($scope.product.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.product.isDes=true;
                }
            }
            $http.get(SETTING.ApiUrl + '/Product/GetAllProduct',{params:$scope.product,'withCredentials':true}).success(function (data) {
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


