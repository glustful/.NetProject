﻿angular.module("app").controller('ContentIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            title: '',
            page: 1,
            pageSize: 10
        };

        var getContentList = function() {
            $http.get(SETTING.ApiUrl+'/Content/Index',
                {params:$scope.searchCondition,'withCredentials':true
                }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.title=data.Condition.Title;
                $scope.searchCondition.page=data.Condition.Page;
                $scope.searchCondition.pageSize=data.Condition.PageCount;
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
                   },
                   'withCredentials':true
               }).success(function(data){
                   if(data.Status){
                       getContentList();
                   }
                   else{
                           $scope.alerts=[{type:'danger',msg:data.Msg}];
                       }
               })
            });
        };
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };
    }
]);

angular.module("app").controller('ContentDetailController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $http.get(SETTING.ApiUrl + '/Content/Detailed/' + $stateParams.id,{ 'withCredentials':true}).success(function(data){
       $scope.ContentModel =data;
    });
}]);

angular.module("app").controller('ContentCreateController',['$http','$scope','$state','FileUploader',function($http,$scope,$state, FileUploader){
    $scope.ContentModel = {
        Id: 0,
        Title: '',
        Status: '0',
        Content:'',
        TitleImg:'',
        ChannelId: 0,
        AddUser:0,
        AdSubTitle:''

    };

    $http.get(SETTING.ApiUrl + '/Channel/Index',{'withCredentials':true}).success(function(data){
        $scope.ChannelList = data.List;
    });

    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Content/Create',$scope.ContentModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.content.index");
            }
            else{
                $scope.alerts=[{type:'danger',msg:data.Msg}];
            }
        });
    }
    $scope.closeAlert = function(index) {
        $scope.alerts.splice(index, 1);
    };
    var uploader = $scope.uploader = new FileUploader({
        url: SETTING.ApiUrl+'/Resource/Upload',
        'withCredentials':true
    })
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
        $scope.ContentModel.TitleImg=response.Msg;
    };
}]);

angular.module("app").controller('ContentEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Content/Detailed/' + $stateParams.id,{'withCredentials':true}).success(function(data){
        $scope.ContentModel =data;
        $scope.titleImg = SETTING.ImgUrl+ data.TitleImg;
    });

    $http.get(SETTING.ApiUrl + '/Channel/Index',{'withCredentials':true}).success(function(data){
        $scope.ChannelList = data.List;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Content/Edit',$scope.ContentModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CMS.content.index");
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