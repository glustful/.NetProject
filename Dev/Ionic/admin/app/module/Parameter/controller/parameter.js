/**
 * Created by Administrator on 2015/9/11.
 */

app.controller('ParameterController', ['$scope', '$http', '$state','$modal', function ($scope, $http, $state,$modal) {
    $scope.my_data = [];
    $scope.Parameter = {
        Id: '',
        Name:'',
        Sort:'',
        Category:''
    };
    $scope.searchCondition = {
        CategoryId:''
    };

    $scope.getList=function() {
        $http.get(SETTING.ZergWcApiUrl+ '/Parameter/Get/', {params:$scope.searchCondition,'withCredentials': true}).success(function (data) {
            $scope.list = data;
        })
    };


    $scope.getClassList=function() {
        $http.get(SETTING.ZergWcApiUrl+ '/Category/GetAllClassify/', {'withCredentials': true}).success(function (data) {
            $scope.my_data = data;
            console.log($scope.my_data);

        })
    };
    $scope.getClassList();

    //添加参数
    $scope.addParameter = function () {
        if( $scope.searchCondition.CategoryId==undefined ||  $scope.searchCondition.CategoryId=="")
        {
         alert("请选择相应的分类！");

        }else
        {
            $http.post(SETTING.ZergWcApiUrl + '/Parameter/Post',   $scope.Parameter, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    AddParameterWindowClose();

                }
            });
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


    //选中事件；
    $scope.selectEvent = function (branch) {
        $scope.output = '您选择了分类：" ' + branch.label + '", ID为："' + branch.Id+'"';;
        if (branch.children ==null) {
            $scope.Parameter.Id=branch.Id;//添加时候 这里传的是分类ID
            $scope.searchCondition.CategoryId = branch.Id;
            document.getElementById("btnok").disabled=true;
            $scope.getList();
        } else {
            $scope.output = '您选择了分类：" ' + branch.label + '", ID为："' + branch.Id+'"  '+ "[温馨提示：只有末端分支才能添加属性参数！]";
            $scope.searchCondition.CategoryId="";
            document.getElementById("btnok").disabled=false;
        }
    };




















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


