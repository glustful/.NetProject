/**
 * Created by AddBean on 2015/5/10 0010.
 */
app.controller('ParameterController', ['$scope', '$http', '$state', function ($scope, $http, $state) {
    var tree, treedata_avm;
    $scope.rowParameter = [];
    //选中事件；
    $scope.selectEvent = function (branch) {
        var _ref;
        $scope.output = '您选择了" ' + branch.label + '", ID为："' + branch.Id+'"';;
        if (branch.children.length == 0) {
            $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterByClassify?classifyId=' + branch.Id).success(function (data) {
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
    $http.get(SETTING.TradingApiUrl + '/Classify/GetAllClassify/').success(function (data) {
        $scope.my_data = data;
        $scope.my_tree.select_branch($scope.my_tree.get);
    });

    //添加参数
    $scope.parameterName = "";
    $scope.addParameter = function () {
        var selectedBranch = tree.get_selected_branch();
        if (selectedBranch.children.length == 0) {
            var par = {
                ClassifyId: selectedBranch.Id,
                Name: $scope.parameterName,
                Sort: 1,
                Adduser: 'jiadou',
                Upduser: 'jiadou'
            };
            var parJson = JSON.stringify(par);
            $http.post(SETTING.TradingApiUrl + '/Classify/AddParameter', parJson, {
                'withCredentials': true
            }).success(function (data) {
                $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterByClassify?classifyId=' + selectedBranch.Id).success(function (data) {
                    $scope.rowParameter = data;
                });
                $scope.output = "温馨提示：只有末端分支才能添加属性参数！";
                AddParameterWindowClose();
            });
        } else {
            $scope.output = "温馨提示：只有末端分支才能添加属性参数！";
            AddParameterWindowClose();
        }
    }

    //删除参数
    $scope.delParameter = function (parameterId) {
        $http.get(SETTING.TradingApiUrl + '/Classify/DelParameter?parameterId=' + parameterId).success(function (data) {
            var selectedBranch = tree.get_selected_branch();
            $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterByClassify?classifyId=' + selectedBranch.Id).success(function (data) {
                $scope.rowParameter = data;
            });
            $scope.output = data;
        });
    }

    //获取所有参数值；
    $scope.rowParameterValue = [];
    $scope.selectParameterId = 0;
    $scope.getParameterValue = function (parameterId) {
        $scope.selectParameterId = parameterId;
        $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + parameterId).success(function (data) {
            $scope.rowParameterValue = data;
        });
    }

    //添加参数值
    $scope.ClassifyParameterValue = "";
    $scope.addParameterValue = function () {
        var parValue = {
            ParameterId: $scope.selectParameterId,
            Parametervalue: $scope.ClassifyParameterValue,
            Adduser: 'jiadou',
            Upduser: 'jiadou'
        };
        var parValueJson = JSON.stringify(parValue);
        $http.post(SETTING.TradingApiUrl + '/Classify/AddParameterValue', parValueJson, {
            'withCredentials': true
        }).success(function (data) {
            AddValueWindowClose();
            $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + $scope.selectParameterId).success(function (data) {
                $scope.rowParameterValue = data;
            });
            $scope.output = data;
        });
    }

    //删除参数值
    $scope.delParameterValue=function(parameterValueId){
        $http.get(SETTING.TradingApiUrl + '/Classify/DelParameterValue?parameterValueId=' + parameterValueId).success(function (data) {
            $http.get(SETTING.TradingApiUrl + '/Classify/GetParameterValueByParameter?parameterId=' + $scope.selectParameterId).success(function (data) {
                $scope.rowParameterValue = data;
            });
            $scope.output = data;
        });
    };
}]);