/**
 * Created by Yunjoy on 2015/5/9.
 */
angular.module("app").controller('TagIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            LikeTag: '',
            page: 1,         //????
            pageSize: 10,   //???????
            totalPage:1
                 //??????
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/Tag/Index',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.TotalCount;
            });
        };
        $scope.getList = getTagList;
        getTagList();

        $scope.del = function (id) {
             $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要删除吗？";}
                }
            });
            modalInstance.result.then(function(){
                $http.get(SETTING.ApiUrl + '/Tag/Delete',{
                        params:{
                            tagId:$scope.selectedId
                        },
                        'withCredentials':true
                    }
                ).success(function(data) {
                        if (data.Status) {
                            getTagList();
                        }
                    });
            })
        }
    }
]);

angular.module("app").controller('TagDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id,{'withCredentials':true}).success(function(data){
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.TagModel =data;
    });
}]);

angular.module("app").controller('TagCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.TagModel = {
        Id: 0,
        Tag:''
    };

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Tag/Create',$scope.TagModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.tag.index");
            }
            else{
                //$scope.Message=data.Msg;
                $scope.alerts=[{type:'danger',msg:data.Msg}];
            }
        });
    }
    $scope.closeAlert = function(index) {
        $scope.alerts.splice(index, 1);
    };
}]);

angular.module("app").controller('TagEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id,{'withCredentials':true}).success(function(data){
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.TagModel =data.Tag;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Tag/Edit',$scope.TagModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.tag.index");
            }
            else{
                $scope.alerts=[{type:'danger',msg:data.Msg}];
            }
        });
    }
    $scope.closeAlert = function(index) {
        $scope.alerts.splice(index, 1);
    };
}]);