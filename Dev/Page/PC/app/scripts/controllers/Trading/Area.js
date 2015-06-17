/**
 * Created by Administrator on 2015/6/17.
 */
app.controller('AreaTreeCtrl', ['$scope', '$http', '$state','$modal', function ($scope, $http, $state,$modal) {
    var tree, treedata_avm;
    //选中事件；
    $scope.selectEvent = function (branch) {
        var _ref;
        $scope.output = '您选择了" ' + branch.label + '", ID为："' + branch.Id+'"';;
        if ((_ref = branch.data) != null ? _ref.description : void 0) {
            return $scope.output += '(' + branch.data.description + ')';
        }
    };
    $scope.my_data = [];
    $scope.my_tree = tree = {};
    $scope.getAreaList=function() {
        $http.get(SETTING.ApiUrl + '/Area/GetAllTree/', {'withCredentials': true}).success(function (data) {
            $scope.my_data = data;
            $scope.my_tree.select_branch($scope.my_tree.get);
        })
    };
    $scope.getAreaList();
    $scope.addArea=function(){
        var selectedBranch;
        selectedBranch = tree.get_selected_branch();
        if(selectedBranch!=null){
            $scope.output = "您正在为 " + selectedBranch.label + " 添加子地区" + $scope.areaName + " ";
            var area = {
                Id: selectedBranch.Id,
                AreaName: $scope.areaName
                //Adduser: '1',
                //Upduser: '1'
            };
            var areaJson = JSON.stringify(area);
            $http.post(SETTING.ApiUrl + '/Area/AddArea', areaJson, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    WindowClose();
                    $http.get(SETTING.ApiUrl + '/Area/GetAllTree/',{'withCredentials':true}).success(function (data) {
                        $scope.my_data = data;
                        $scope.my_tree.expand_all();
                    });
                }
                else{
                    $scope.alerts=[{type:'danger',msg:data.Msg}];
                }
            });
            $scope.closeAlert = function(index) {
                $scope.alerts.splice(index, 1);
            };
        }else{
            $scope.output = "您正在添加 " + $scope.areaName + "地区! ";
            var selectedFatherBranch;
            var area;
            try{
                selectedFatherBranch= $scope.my_tree.get_parent_branch( $scope.my_tree.get_selected_branch());
                area = {
                    Id:selectedFatherBranch.Id,
                    AreaName: $scope.areaName
                };
            }catch(e){
                area = {
                    Id:0,
                    AreaName: $scope.areaName
                };
            }
            var areaJson = JSON.stringify(area);
            $http.post(SETTING.ApiUrl + '/Area/AddArea', areaJson, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    WindowClose();
                    $http.get(SETTING.ApiUrl + '/Area/GetAllTree/',{'withCredentials':true}).success(function (data) {
                        $scope.my_data = data;
                        $scope.my_tree.expand_all();
                    });
                }
                else{
                    $scope.alerts=[{type:'danger',msg:data.Msg}];
                }
            });
            $scope.closeAlert = function(index) {
                $scope.alerts.splice(index, 1);
            };
        }
    }
    $scope.delArea=function() {
        var selectedBranch;
        selectedBranch = tree.get_selected_branch();
        if (selectedBranch != null) {
            $scope.output = "您正在删除 " + selectedBranch.label + " 地区! ";
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
                $http.get(SETTING.ApiUrl + '/Area/Delete?id=' + selectedBranch.Id, {
                    'withCredentials': true
                }).success(function (data) {
                    if (data.Status) {
                        $scope.getAreaList();
                    }
                    else {
                        $scope.alerts = [ {type: 'danger', msg: data.Msg} ];
                    }
                });
            });
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }else{
            alert("请至少选择一个地区");
        }
    }
}]);