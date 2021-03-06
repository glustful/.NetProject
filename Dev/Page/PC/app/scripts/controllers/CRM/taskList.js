/**
 * Created by 黄秀宇 on 2015/5/12.
 */
app.controller('taskIndexController',['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.searchCondition = {
        Taskname: '',
        Id:0,
        page: 1,
        pageSize: 10,
        orderByAll:"OrderByTaskname",//排序
        isDes:true//升序or降序,
    };
    //初始化所有图标
    var iniImg=function(){
        $scope.OrderByTaskname="footable-sort-indicator";
        $scope.OrderByName="footable-sort-indicator";
        $scope.OrderByEndtime="footable-sort-indicator";
        $scope.OrderByAdduser="footable-sort-indicator";
    }
    iniImg();
    $scope.OrderByTaskname="fa-caret-down";//升降序图标
//------------------------查询任务 start--------------------------
    var getTaskList  = function(orderByAll) {
        if(orderByAll!=undefined){
            $scope.searchCondition.orderByAll=orderByAll ;
            if($scope.searchCondition.isDes==true)//如果为降序，
            {
                $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                iniImg();//将所有的图标变成一个月
                eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                $scope.searchCondition.isDes=false;//则变成升序
            }
            else if($scope.searchCondition.isDes==false)
            {
                $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                iniImg();
                eval($scope.d);
                $scope.searchCondition.isDes=true;
            }
        }
        $http.get(SETTING.ApiUrl+'/Task/TaskList/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            if(data.totalCount>0){
                $scope.visibleif=true;
                $scope.tips="";
            $scope.list = data.list;
            $scope.searchCondition.page=data.condition.Page;
            $scope.searchCondition.pageSize=data.condition.PageCount;
            $scope.searchCondition.totalCount=data.totalCount;}
            else{
                $scope.visibleif=false;
                $scope.tips="没有数据";
            }
        });
    };
    $scope.getList = getTaskList;
    getTaskList();
//------------------------查询任务 end----------------------------

//---------------------根据任务id删除任务 start-------------------
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
//---------------------根据任务id删除任务 end---------------------

   }
]);
app.controller('taskListcontroller',['$http','$scope','$stateParams','$modal',function($http,$scope,$stateParams,$modal) {
    $scope.searchCondition1 = {
        brokerName:'',
        id:$stateParams.id,
        page: 1,
        pageSize: 10
    };
//-------------------------根据用户id查询任务列表 start----------------
    var getTaskList1  = function() {
        $http.get(SETTING.ApiUrl+'/Task/taskListByuser',{params:$scope.searchCondition1, 'withCredentials':true}).success(function(data){
            if(data.totalCount>0){
                $scope.visibleif=true;
                $scope.tips="";
            $scope.taskModel = data.list;
            $scope.searchCondition1.page=data.condition.Page;
            $scope.searchCondition1.pageSize=data.condition.PageCount;
            $scope.searchCondition1.totalCount=data.totalCount;}
            else{
                $scope.visibleif=false;
                $scope.tips="没有数据";
}

        });
    };
$scope.getList1=getTaskList1 ;
    getTaskList1();
//-------------------------根据用户id查询任务列表 end------------------

}
]);



