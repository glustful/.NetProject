/**
 * Created by Administrator on 2015/6/18.
 */

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










        //编辑商品信息
         $scope.update=function(){
            var newproduct = {
            Id:$scope.product.Id,
                ClassifyId:$scope.product.ClassId,
                brandId:$scope.product.BrandId,
            Price:$scope.product.Price,
            Productname:$scope.product.Productname,
            RecCommission:$scope.product.RecCommission,
            Commission:$scope.product.Commission,
            ContactPhone:$scope.product.Phone,
            Dealcommission:$scope.product.Dealcommission,
            Status:$scope.product.Status,
            Recommend:$scope.product.Recommend,
            Stockrule:$scope.product.Stockrule,
            SubTitle:$scope.product.SubTitle,
                Productimg:$scope.product.Productimg
            };
            var newproductDetail = {
                 Productdetail:$scope.product.ProductDetailed,
                 Sericeinstruction:$scope.product.Sericeinstruction
            };
            var classifyJson = JSON.stringify({ product: newproduct, productDetail: newproductDetail });
            $http.post(SETTING.ApiUrl + '/Product/EditProduct',classifyJson,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    $state.go("page.Trading.product.product");
                }
                else{
                    $scope.alerts=[{type:'danger',msg:data.Msg}];
                }
            });
        };

    }]);

