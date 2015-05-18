/**
 * Created by Yunjoy on 2015/5/9.
 */
//app.controller('ModalInstanceCtrl', ['$scope', '$modalInstance','msg',function($scope, $modalInstance,msg) {
//    $scope.msg = msg;
//    $scope.ok = function () {
//        $modalInstance.close();
//        $scope.id = id;
//        $http.get(SETTING.ApiUrl + '/Tag/Delete',{
//                params:{
//                    tagId:$scope.id
//                }
//            }
//        ).success(function(data) {
//            if (data.Status) {
//                refresh();
//                $modalInstance.close();
//            }
//        });
//    };
//    $scope.cancel = function () {
//        $modalInstance.dismiss('cancel');
//    };
//}]);
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
            $http.get(SETTING.ApiUrl+'/Tag/Index',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.TotalCount;
            });
        };
        $scope.getList = getTagList;
        getTagList();

        $scope.open = function (id) {
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
                        }
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
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id).success(function(data){
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
                $scope.Message=data.Msg;
            }
        });
    }
}]);

angular.module("app").controller('TagEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Tag/Detailed/' + $stateParams.id).success(function(data){
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
                $scope.Message=data.Msg;
            }
        });
    }
}]);