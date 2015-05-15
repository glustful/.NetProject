﻿angular.module("app").controller('ContentIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            title: '',
            page: 1,
            pageSize: 10
        };

        var getContentList = function() {
            $http.get(SETTING.ApiUrl+'/Content/Index',{params:$scope.searchCondition}).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.title=data.Condtion.Title;
                $scope.searchCondition.page=data.Condition.Page;
                $scope.searchCondition.pageSize=data.Conditon.PageCount;
                $scope.searchCondition.totalPage=Math.ceil(data.TotalCount/data.Condition.PageCount);
                $scope.totalCount = data.TotalCount;
            });
        };
        $scope.getList = getContentList;
        getContentList();

        $scope.open=function(id){
            $scope.selectedId=id;
            var modalInstance=$modal.open({
                templateUrl:'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve:{
                    msg:function(){
                        return "你确定要删除吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
               $http.get(SETTING.ApiUrl+'/Content/Delete',{
                   params:{
                       id:$scope.selectedId
                   }
               }).success(function(data){
                   if(data.Status){
                       getContentList();
                   }
               })
            });
        }
    }
]);

angular.module("app").controller('ContentDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Content/Detailed/' + $stateParams.id).success(function(data){
       $scope.ContentModel =data;
    });
}]);

angular.module("app").controller('ContentCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.ContentModel = {
        Id: 0,
        Title: '',
        Status: '0',
        Content:'',
        ChannelId: 0,
        AddUser:0
    };

    $http.get(SETTING.ApiUrl + '/Channel/Index').success(function(data){
        $scope.ChannelList = data.List;
    });

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Content/Create',$scope.ContentModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.content.index");
            }
        });
    }
}]);

angular.module("app").controller('ContentEditController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Content/Detailed/' + $stateParams.id).success(function(data){
        $scope.ContentModel =data;
    });

    $http.get(SETTING.ApiUrl + '/Channel/Index').success(function(data){
        $scope.ChannelList = data.List;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Content/Edit',$scope.ContentModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.content.index");
            }
        });
    }
}]);