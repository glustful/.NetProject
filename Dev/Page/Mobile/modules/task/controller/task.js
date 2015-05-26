/**
 * Created by 黄秀宇 on 2015/5/26.
 */
app.controller('taskController',['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.searchCondition = {
        Taskname: '',

        Id:0,
        page: 1,
        pageSize: 10
    };
    //查询任务
    var getTaskList  = function() {
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.totalCount>0){
                $scope.list = data.list;
                $scope.searchCondition.page=data.condition.Page;
                $scope.searchCondition.pageSize=data.condition.PageCount;
                $scope.searchCondition.totalCount=data.totalCount;}
            else{

            }
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
    //删除任务
    $scope.delTask= function (id) {
        $scope.selectedId = id;
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
            $http.get(SETTING.ApiUrl +'/Task/DelTask/', {
                    params: {
                        id: $scope.selectedId
                    }, 'withCredentials':true
                }
            ).success(function (data) {
                    if (data.Status) {
                        getTaskList();
                        var modalInstanc = $modal.open({
                            templateUrl: 'myModalContent.html',
                            controller: 'ModalInstanceCtrl',
                            resolve: {
                                msg: function () {
                                    return data.Msg;
                                }
                            }
                        });

                    }
                    else{
                        var modalInstanc = $modal.open({
                            templateUrl: 'myModalContent.html',
                            controller: 'ModalInstanceCtrl',
                            resolve: {
                                msg: function () {
                                    return data.Msg;
                                }
                            }
                        });
                        //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
        });
    }

}
]);