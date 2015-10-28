/**
 * Created by Administrator on 2015/10/24.
 */
app.controller("ServiceListCtr",['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.condition = {
        Page: 1,
        PageCount: 10
    }
    var getServiceList = function () {
        $http.get(SETTING.ZergWcApiUrl + "/Service/GetList", {
            params: $scope.sech,
            'withCredentials': true  //跨域
        }).success(function (data) {
                $scope.list = data.List;
                $scope.condition.Page = data.Condition.Page;
                $scope.condition.PageCount = data.Condition.PageCount;
                $scope.totalCount = data.TotalCount;
            }
        )
    }
    getServiceList();
    $scope.getList=getServiceList;
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
            $http.delete(SETTING.ZergWcApiUrl + '/Service/Delete',{
                    params:{
                        id:$scope.selectedId
                    },
                    'withCredentials':true
                }
            ).success(function(data) {
                    if (data.Status) {
                        getServiceList();
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
}])
app.controller("CreateServiceCtr",['$http','$scope','$state',function($http,$scope,$state){
    $scope.model={
        Name:'',
        Class:'',
        Link:''
    }
    $scope.save=function() {
        $http.post(SETTING.ZergWcApiUrl + "/Service/Post", $scope.model, {
            'withCredentials': true
        }).success(function(data){
            if(data.Status)
            {
                $state.go("app.service.serviceList")
            }
        })
    }
}])
app.controller("EditServiceCtr",['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ZergWcApiUrl+"/Service/GetService?Id="+$stateParams.Id,{
        'withCredentials':true
    }).success(function(data){
        $scope.model=data
    })
    $scope.update=function(){
        $http.put(SETTING.ZergWcApiUrl+"/Service/Put",$scope.model,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status)
            {
                $state.go("app.service.serviceList")
            }
        })
    }
}])