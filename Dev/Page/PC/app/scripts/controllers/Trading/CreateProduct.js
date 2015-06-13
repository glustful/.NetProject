/**
 * Created by AddBean on 2015/5/13 0013.
 */
angular.module("app").controller('CreatProductController', [
    '$http', '$scope', '$state','FileUploader', function ($http, $scope, $state,FileUploader) {
        //选择商品分类；
        var classifys = $scope.classifys = {};
        $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify',{'withCredentials':true}).success(function (data) {
            classifys.optionsData = data;
        });
        $scope.classifyMsg = "";
        $scope.classifyMsg1 = "";
        $scope.selectclassifyChange = function () {
            $http.get(SETTING.ApiUrl + '/Classify/GetNextNodesById?nodeId=' + classifys.selectClassifyId,{'withCredentials':true}).success(function (dataRes) {
                if (dataRes == "获取失败") {
                    $scope.classifyMsg1 = "选择完成！";
                    $http.get(SETTING.ApiUrl + '/Classify/GetParameterTreeData?classifyId=' + classifys.selectClassifyId,{'withCredentials':true}).success(function (data) {
                        $scope.parameterValueList = data;
                    });
                } else {
                    $scope.classifyMsg1 = "";
                    $http.get(SETTING.ApiUrl + '/Classify/GetClassifyNameById?classifyId=' + classifys.selectClassifyId,{'withCredentials':true}).success(function (data) {
                        classifys.optionsData = dataRes;
                        $scope.classifyMsg = $scope.classifyMsg + data + "->";
                    });
                }
            });
        };

        //重置分类选择；
        $scope.resetClassifySelect = function () {
            $scope.classifyMsg = "";
            $scope.classifyMsg1 = "";
            $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify',{'withCredentials':true}).success(function (data) {
                classifys.optionsData = data;
            });
        };

        //选择分类值
        $scope.parameterValueList = [];
//        $http.get(SETTING.ApiUrl + '/Classify/GetParameterTreeData?classifyId=' + 57).success(function (data) {
//            $scope.parameterValueList = data;
//        });

        //选择品牌项目
        var brands = $scope.brands = {};
        $http.get(SETTING.ApiUrl + '/Brand/GetBrandList',{'withCredentials':true}).success(function (data) {
            brands.optionsData = data;
        });
        $scope.selectBrandChange = function () {
            brands.selectBrandVal;
        };

        //添加商品
        $scope.ProductName = "";
        $scope.Productdetail = "";
        $scope.Sericeinstruction = "";
        $scope.selectParameterValue = [];
        $scope.addProduct = function () {
            var product = {
                ClassifyId: classifys.selectClassifyId,// 平台商品分类ID
                ProductBrandId: brands.selectBrandVal,// 品牌ID
                Commission: $scope.commission,//佣金
                Dealcommission: $scope.dealCommission,//成交佣金；
                Price: $scope.price,//价格；
                BussnessId: 1,// 商家ID
                Status: $scope.Status == 0 ? true : false,// 商品上架状态
                Recommend: $scope.Recommend == 0 ? true : false,// 商家推荐标识
                Sort: 1,// 分类排序
                Stockrule: $scope.Stockrule// 库存计数（拍下、付款）
            };
            var productDetail = {
                Productname: $scope.ProductName,// 商品名称
                Productdetail: $scope.productContent,// 商品明细(5000字富文本编辑)
                Productimg: $scope.Productimg,// 商品图片（主图）
                Productimg1: $scope.Productimg1,// 商品附图1
                Productimg2: $scope.Productimg2,// 商品附图2
                Productimg3: $scope.Productimg3,// 商品附图3
                Productimg4: $scope.Productimg4,// 商品附图4
                Sericeinstruction: "售后说明",// 售后说明
                Adduser: "UserId",
                Upduser: "UserId"
            };
            if (
                //product.ClassifyId == undefined ||
                //product.Commission == undefined ||
                //product.Dealcommission == undefined ||
                //product.Price == undefined ||
               // product.ProductBrandId == undefined ||
                //product.Sort == undefined ||
                //product.Recommend == undefined ||
                //productDetail.Productdetail == undefined ||
                //productDetail.Productimg == "" ||
                //productDetail.Productimg1 == "" ||
                //productDetail.Productimg2 == "" ||
                //productDetail.Productimg3 == "" ||
                //productDetail.Productimg4 == "" ||
                productDetail.Productname == "") {
                alert("添加失败，请认真检查是否有漏填项！");
            } else {
                var classifyJson = JSON.stringify({ product: product, productDetail: productDetail });
                $http.post(SETTING.ApiUrl + '/Product/AddProduct', classifyJson, {
                    'withCredentials': true
                }).success(function (productId) {
                    if (productId > 0) {
                        //遍历添加参数
                        valueClick();
                        for (var i = 0; i < $scope.selectParameterValue.length; i++) {
                            $http.get(SETTING.ApiUrl + '/Classify/AddProductParameterVaule?parameterValueId=' + $scope.selectParameterValue[i] + "&productId=" + productId,{'withCredentials':true}).success(function (data) {

                            });
                        }
                        alert("添加成功");
                        $state.go('page.Trading.product.product');
                    } else {
                        alert("添加失败");
                    }
                    ;

                });
            }


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

        //上传部分
        $scope.Productimg = "";
        $scope.Productimg1 = "";
        $scope.Productimg2 = "";
        $scope.Productimg3 = "";
        $scope.Productimg4 = "";
        $(':file').change(function () {
            var file = this.files[0];
            name = file.name;
            size = file.size;
            type = file.type;
            //your validation
            var formData = new FormData($('form')[0]);
            $.ajax({
                url: SETTING.ApiUrl + '/Resource/Upload',  //server script to process data
                type: 'POST',
                xhr: function () {  // custom xhr
                    myXhr = $.ajaxSettings.xhr();
                    if (myXhr.upload) { // check if upload property exists
                        myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // for handling the progress of the upload
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
        $scope.images = [];
        function completeHandler(e) {

            $scope.images.push("http://img.yoopoon.com/"  +e);
//            $scope.Productimg = "http://img.yoopoon.com/" +  strs[0];
//            $scope.Productimg1 = "http://img.yoopoon.com/" +  strs[1];
//            $scope.Productimg2 = "http://img.yoopoon.com/" +  strs[2];
//            $scope.Productimg3 = "http://img.yoopoon.com/" +  strs[3];
//            $scope.Productimg4 = "http://img.yoopoon.com/" +  strs[4];
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
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        })
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            completeHandler(response.Msg);
        };
    }
]);
