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








    //删除参数值
    $scope.del = function (parameterId) {
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
            $http.get(SETTING.ZergWcApiUrl + '/Parameter/Delete', {
                params:{
                    id:parameterId
                },
                'withCredentials': true
            }).success(function (data) {
                if (data.Status) {
                    $scope.getList();
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

}]);


app.controller('parametervalueIndex',['$http','$scope','$state','$modal','$stateParams',function($http,$scope,$state,$modal,$stateParams){

    if($stateParams.parameterId==undefined ||$stateParams.parameterId=="" )
    {
       // alert($stateParams.name);
        $state.go("app.parameter.parameterList");
    }
    $scope.parName=$stateParams.name;
    $scope.getParametervalueList=function() {
        $http.get(SETTING.ZergWcApiUrl + "/ParameterValue/Get?ParameterId=" + $stateParams.parameterId, {
            'withCredentials': true  //跨域
        }).success(function (data) {
            $scope.parameterValueList = data;
        })
    };
    $scope.getParametervalueList();
    $scope.ParameterValue = {
        Id: $stateParams.parameterId,
        ParameterValue:'',
        Sort:''

    };

    $scope.addParameter = function () {

            $http.post(SETTING.ZergWcApiUrl + '/ParameterValue/Post',   $scope.ParameterValue, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    AddParameterWindowClose();
                    $scope.getParametervalueList();
                }
            });


    }

    $scope.del = function (parameterId) {
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
            $http.get(SETTING.ZergWcApiUrl + '/ParameterValue/Delete', {
                params:{
                    id:parameterId
                },
                'withCredentials': true
            }).success(function (data) {
                if (data.Status) {
                    $scope.getParametervalueList();
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

}])
