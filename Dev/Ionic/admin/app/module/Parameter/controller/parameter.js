/**
 * Created by Administrator on 2015/9/11.
 */

app.controller('ParameterController', ['$scope', '$http', '$state','$modal', function ($scope, $http, $state,$modal) {
    var tree, treedata_avm;
    $scope.rowParameter = [];
    //选中事件；
    $scope.selectEvent = function (branch) {
        var _ref;
        $scope.output = '您选择了" ' + branch.label + '", ID为："' + branch.Id+'"';;
        if (branch.children.length == 0) {
            $http.get(SETTING.ApiUrl + '/Classify/GetParameterByClassify?classifyId=' + branch.Id,{'withCredentials':true}).success(function (data) {
                $scope.rowParameter = data;
            });
        } else {
            $scope.output = "温馨提示：只有末端分支才能添加属性参数！";
        }
    };
    $scope.classifyType = 0;
    $scope.classifyValue = "";
    $scope.my_data = [];
    $scope.my_tree = tree = {};

    //初始化树形图
    $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/',{'withCredentials':true}).success(function (data) {
        $scope.my_data = data;
        $scope.my_tree.select_branch($scope.my_tree.get);
    });

    //添加参数
    $scope.parameterName = "";
    $scope.addParameter = function () {
        var selectedBranch = tree.get_selected_branch();
        if (selectedBranch!=null) {
            var par = {
                ClassifyId: selectedBranch.Id,
                Name: $scope.parameterName,
                Sort: 1
                //Adduser: 'jiadou',
                //Upduser: 'jiadou'
            };
            var parJson = JSON.stringify(par);
            $http.post(SETTING.ApiUrl + '/Classify/AddParameter', parJson, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    var selectedBranch = tree.get_selected_branch();
                    $http.get(SETTING.ApiUrl + '/Classify/GetParameterByClassify?classifyId=' + selectedBranch.Id,
                        {'withCredentials':true}).success(function (data) {
                            $scope.rowParameter = data;
                        });
                }
                else{
                    $scope.alerts=[{type:'danger',msg:data.Msg}];
                }
                //$scope.output = "温馨提示：只有末端分支才能添加属性参数！";
                AddParameterWindowClose();
            });
            $scope.closeAlert = function(index) {
                $scope.alerts.splice(index, 1);
            };
        } else {
            $scope.output = "温馨提示：只有末端分支才能添加属性参数！";
            AddParameterWindowClose();
        }
    }

    //删除参数
    $scope.delParameter = function (parameterId) {
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
            $http.get(SETTING.ApiUrl + '/Classify/DelParameter?parameterId=' + parameterId, {
                'withCredentials': true
            }).success(function (data) {
                if (data.Status) {
                    var selectedBranch = tree.get_selected_branch();
                    $scope.selectEvent(selectedBranch);
//            $http.get(SETTING.ApiUrl + '/Classify/GetParameterByClassify?classifyId=' + selectedBranch.Id,{'withCredentials':true}).success(function (data) {
//                $scope.rowParameter = data;
//            });
                    //$scope.output = data;
                }
                else {
                    $scope.alerts = [ {type: 'danger', msg: data.Msg} ];
                }
            });
        });
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
//        $http.get(SETTING.ApiUrl + '/Classify/DelParameter?parameterId=' + parameterId,{'withCredentials':true}).success(function (data) {
//            var selectedBranch = tree.get_selected_branch();
//            $http.get(SETTING.ApiUrl + '/Classify/GetParameterByClassify?classifyId=' + selectedBranch.Id,{'withCredentials':true}).success(function (data) {
//                $scope.rowParameter = data;
//            });
//            $scope.output = data;
//        });
    }

    //获取所有参数值；
    $scope.rowParameterValue = [];
    $scope.selectParameterId = 0;
    $scope.getParameterValue = function (parameterId) {
        $scope.selectParameterId = parameterId;
        $http.get(SETTING.ApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + parameterId,{'withCredentials':true}).success(function (data) {
            $scope.rowParameterValue = data;
        });
    }

    //添加参数值
    $scope.ClassifyParameterValue = "";
    $scope.addParameterValue = function () {
        var parValue = {
            ParameterId: $scope.selectParameterId,
            Parametervalue: $scope.ClassifyParameterValue
            //Adduser: 'jiadou',
            // Upduser: 'jiadou'
        };
        var parValueJson = JSON.stringify(parValue);
        $http.post(SETTING.ApiUrl + '/Classify/AddParameterValue', parValueJson, {
            'withCredentials': true
        }).success(function (data) {
            if(data.Status)
            {
                AddValueWindowClose();
                $http.get(SETTING.ApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + $scope.selectParameterId,{'withCredentials':true}).success(function (data) {
                    $scope.rowParameterValue = data;
                });
//                $scope.output = data;
            }
            else{
                $scope.alerts = [ {type: 'danger', msg: data.Msg} ];
            }
        });
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }

    //删除参数值
    $scope.delParameterValue=function(parameterValueId){
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
            $http.get(SETTING.ApiUrl + '/Classify/DelParameterValue?parameterValueId=' + parameterValueId,
                {'withCredentials': true}).success(function (data) {
                    if(data.Status)
                    {
                        $scope.getParameterValue($scope.selectParameterId);
//                         $http.get(SETTING.ApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + $scope.selectParameterId, {'withCredentials': true}).success(function (data) {
//                            $scope.rowParameterValue = data;
//                        });
//                    $scope.output = data;
                    }

                });
        });
    };
}]);



app.controller('parameterIndex', ['$scope', '$http','$modal', function($scope, $http,$modal) {
    $scope.sech={
        Page:1,
        PageCount:10
    };
    var getparameterList=function()
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
    getparameterList();
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

app.controller('editparameter',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
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

app.controller('createparameter',['$http','$scope','$state','FileUploader',function($http,$scope,$state,FileUploader){
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
