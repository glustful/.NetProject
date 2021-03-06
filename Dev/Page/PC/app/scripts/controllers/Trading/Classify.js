app.controller('ProductTreeCtrl', ['$scope', '$http', '$state','$modal', function ($scope, $http, $state,$modal) {
    var tree, treedata_avm;
    //选中事件；
    $scope.selectEvent = function (branch) {
        var _ref;
        $scope.output = '您选择了" ' + branch.label + '", ID为："' + branch.Id+'"';;
        if ((_ref = branch.data) != null ? _ref.description : void 0) {
            return $scope.output += '(' + branch.data.description + ')';
        }
    };
    $scope.classifyType = 0;
    $scope.classifyValue = "";
    $scope.my_data = [];
    $scope.my_tree = tree = {};
    $scope.getClassList=function() {
        $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/', {'withCredentials': true}).success(function (data) {
            $scope.my_data = data;
            $scope.my_tree.select_branch($scope.my_tree.get);
        })
    };
   $scope.getClassList();
    $scope.addClass=function(){
        var selectedBranch;
        selectedBranch = tree.get_selected_branch();
        if(selectedBranch!=null){
            $scope.output = "您正在为 " + selectedBranch.label + " 添加子分类 " + $scope.classifyValue + " ";
            var cla = {
                ClassifyId: selectedBranch.Id,
                Name: $scope.classifyValue
                //Adduser: '1',
                //Upduser: '1'
            };
            var classifyJson = JSON.stringify(cla);
            $http.post(SETTING.ApiUrl + '/Classify/AddClassify', classifyJson, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    WindowClose();
                    $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/',{'withCredentials':true}).success(function (data) {
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
            $scope.output = "您正在为添加 " + $scope.classifyValue + " 分类!  ";
            var selectedFatherBranch;
            var cla;
            try{
                selectedFatherBranch= $scope.my_tree.get_parent_branch( $scope.my_tree.get_selected_branch());
                cla = {
                    ClassifyId:selectedFatherBranch.Id,
                    Name: $scope.classifyValue
                    // Adduser: '1',
                    //Upduser: '1'
                };
            }catch(e){
                cla = {
                    ClassifyId:0,
                    Name: $scope.classifyValue
                    //Adduser: '1',
                    //Upduser: '1'
                };
            }
            var classifyJson = JSON.stringify(cla);
            $http.post(SETTING.ApiUrl + '/Classify/AddClassify', classifyJson, {
                'withCredentials': true
            }).success(function (data) {
                if(data.Status)
                {
                    WindowClose();
                    $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/',{'withCredentials':true}).success(function (data) {
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
    //添加类别
//    $scope.addClassify = function () {
//            $scope.output = "您正在为添加 " + $scope.classifyValue + " 分类!  ";
//            var selectedFatherBranch;
//            var cla;
//            try{
//                selectedFatherBranch= $scope.my_tree.get_parent_branch( $scope.my_tree.get_selected_branch());
//                cla = {
//                    ClassifyId:selectedFatherBranch.Id,
//                    Name: $scope.classifyValue
//                   // Adduser: '1',
//                    //Upduser: '1'
//                };
//            }catch(e){
//                cla = {
//                    ClassifyId:0,
//                    Name: $scope.classifyValue
//                    //Adduser: '1',
//                    //Upduser: '1'
//                };
//            }
//            var classifyJson = JSON.stringify(cla);
//            $http.post(SETTING.ApiUrl + '/Classify/AddClassify', classifyJson, {
//                'withCredentials': true
//            }).success(function (data) {
//                WindowClose();
//                $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/',{'withCredentials':true}).success(function (data) {
//                    $scope.my_data = data;
//                    $scope.my_tree.expand_all();
//                });
//            });
//    };
    //添加子类别
//    $scope.addSubClassify = function () {
//        var selectedBranch;
//        selectedBranch = tree.get_selected_branch();
//        if(selectedBranch!=null){
//            $scope.output = "您正在为 " + selectedBranch.label + " 添加子分类 " + $scope.classifyValue + " ";
//            var cla = {
//                ClassifyId: selectedBranch.Id,
//                Name: $scope.classifyValue
//                //Adduser: '1',
//                //Upduser: '1'
//            };
//            var classifyJson = JSON.stringify(cla);
//            $http.post(SETTING.ApiUrl + '/Classify/AddClassify', classifyJson, {
//                'withCredentials': true
//            }).success(function (data) {
//                WindowClose();
//                $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/',{'withCredentials':true}).success(function (data) {
//                    $scope.my_data = data;
//                    $scope.my_tree.expand_all();
//                });
//            });
//        }else{
//            alert("请至少选择一个分类");
//        }
//
//    };
    //删除类别
    $scope.delClassify=function() {
        var selectedBranch;
        selectedBranch = tree.get_selected_branch();
        if (selectedBranch != null) {
            $scope.output = "您正在删除 " + selectedBranch.label + " 分类! ";
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
                $http.get(SETTING.ApiUrl + '/Classify/DelClassify?classifyId=' + selectedBranch.Id, {
                    'withCredentials': true
                }).success(function (data) {
                    if (data.Status) {
                        $scope.getClassList();
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
            alert("请至少选择一个分类");
        }
    }
//    $scope.delClassify = function () {
//        var selectedBranch;
//        selectedBranch = tree.get_selected_branch();
//        if(selectedBranch!=null){
//            $scope.output = "您正在删除 " + selectedBranch.label +" 分类! ";
//            $http.get(SETTING.ApiUrl + '/Classify/DelClassify?classifyId='+selectedBranch.Id, {
//                'withCredentials': true
//            }).success(function (data) {
//                $http.get(SETTING.ApiUrl + '/Classify/GetAllClassify/').success(function (data) {
//                    $scope.my_data = data;
//                });
//                $scope.my_tree.expand_all();
//                $scope.output=data;
//            });
//        }else{
//            alert("请至少选择一个分类");
//        }
//
//    };
}]);