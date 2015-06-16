/**
 * Created by AddBean on 2015/5/10 0010.
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
                $scope.output = data;
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
        + "-"
        + date.getHours()
        + ":"
        + date.getMinutes()
        + ":"
        + date.getSeconds()
        ;

}