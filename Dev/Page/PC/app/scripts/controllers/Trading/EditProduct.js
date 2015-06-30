/**
 * Created by Administrator on 2015/6/18.
 */
angular.module("app").controller('EditProductController', [
    '$http', '$scope','$stateParams', function ($http, $scope,$stateParams) {
        //选择商品分类；
        var classifys = $scope.classifys = {};
        $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify',{'withCredentials':true}).success(function (data) {
            classifys.optionsData = data;
        });
        $scope.classifyMsg = "";
        $scope.classifyMsg1 = "";
        $scope.selectclassifyChange = function () {
            $http.get(SETTING.ApiUrl + '/Classify/GetNextNodesById?nodeId=' + $scope.product.ClassId,{'withCredentials':true}).success(function (dataRes) {
                if (dataRes == "获取失败") {
                    $scope.classifyMsg1 = "选择完成！";
                    $http.get(SETTING.ApiUrl + '/Classify/GetParameterTreeData?classifyId=' + $scope.product.ClassId,{'withCredentials':true}).success(function (data) {
                        $scope.parameterValueList = data;
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

        //获取品牌列表
        $http.get(SETTING.ApiUrl + '/Brand/GetBrandList',{'withCredentials':true}).success(function (data) {
            $scope.BrandList = data;
        });
        //获取商品信息
        $http.get(SETTING.ApiUrl+'/Product/GetProductById?productId='+$stateParams.productId).success(function(data)
        {
           $scope.product=data
        });

    }]);