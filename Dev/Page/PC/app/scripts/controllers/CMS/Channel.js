/**
 * Created by �Ϋh�F on 2015/5/9.
 */
angular.module("app").controller('ChannelIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            pageSize: 10,
            totalPage:1
        };

        var getChannelList = function() {
            $http.get(SETTING.ApiUrl+'/Channel/Index',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.TotalCount;
            });
        };
        $scope.getList = getChannelList;
        getChannelList();

        $scope.open=function(id){
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要删除吗？";}
                }
            });
            modalInstance.result.then(function(){
                $http.get(SETTING.ApiUrl + '/Channel/Delete',{
                        params:{
                            id:$scope.selectedId
                        }
                    }
                ).success(function(data) {
                        if (data.Status) {
                            getChannelList();
                        }
                        else{
                            $scope.Message=data.Msg;
                        }
                    });
            });
        }
    }
]);

angular.module("app").controller('ChannelDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Channel/Detailed/' + $stateParams.id).success(function(data){
        $scope.ChannelModel =data;
    });
}]);

angular.module("app").controller('ChannelCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.ChannelModel = {
        Id: 0,
        Name: '',
        Status: '',
        ParentId: 0
    };

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Channel/Create',$scope.ChannelModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.Channel.index");
            }
        });
    }
}]);

angular.module("app").controller('ChannelEditController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Channel/Detailed/' + $stateParams.id).success(function(data){
        $scope.ChannelModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Channel/Edit',$scope.ChannelModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.Channel.index");
            }
        });
    }
}]);