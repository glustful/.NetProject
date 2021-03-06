/**
 * Created by lhl on 2015/5/12 等级配置
 */
angular.module("app").controller('configureIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name:'',
            page: 1,
            pageSize: 10,
            orderByAll:"OrderByName",//排序
            isDes:true//升序or降序,
        };
        var iniImg=function(){
            $scope.OrderByName="footable-sort-indicator";
            $scope.OrderByDescribe="footable-sort-indicator";
            $scope.OrderByAddtime="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderByName="fa-caret-down";//升降序图标
        $scope.getList  = function(orderByAll) {
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
            $http.get(SETTING.ApiUrl+'/Level/SearchLevel',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
                if (data.List==""){
                    $scope.errorTip = "不存在数据";
                }
                else{
                    $scope.errorTip = "";
                }
                console.log(data);
            });
        };
        $scope.getList();


        $scope.open = function(id){
            $scope.selectedId= id;
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
                $http.post(SETTING.ApiUrl+'/Level/DeleteLevel',$scope.selectedId,{ 'withCredentials':true}).success(function(data){
                    if(data.Status){
                        $scope.getList();//成功刷新列表
                    }
                    else{
                        $scope.errorTip=data.Msg;

                    }})
            })
        }

    }
]);

angular.module("app").controller('configureCreateController',['$http','$scope','$state','FileUploader',function($http,$scope,$state,FileUploader){
    $scope.LevelModel = {
        Id: 0,
        Name:'',
        Url:'',
        Describe:''
    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/Level/DoCreate',$scope.LevelModel,{
            'withCredentials':true
        }).success(function(data){
            console.log(data);
            if(data.Status){
                $state.go("page.CRM.configure.index");
            }else{
                    alert(data.Msg);
            }

        });
    }
    //图片上传
    var uploader = $scope.uploader = new FileUploader({
        url: SETTING.ApiUrl+'/Resource/Upload',
        'withCredentials':true
    });
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
        $scope.LevelModel.Url=response.Msg
    };
}]);

angular.module("app").controller('configureEditController',['$http','$scope','$stateParams','$state','FileUploader',function($http,$scope,$stateParams,$state,FileUploader){
    $http.get(SETTING.ApiUrl + '/Level/GetLevel/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.LevelModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Level/DoEdit',$scope.LevelModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.index");
            }else{
                alert(data.Msg);
            }
        });
    }
    //图片上传
    var uploader = $scope.uploader = new FileUploader({
        url: SETTING.ApiUrl+'/Resource/Upload',
        'withCredentials':true
    });
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);
        $scope.LevelModel.Url=response.Msg
    };
}]);







angular.module("app").controller('configureSetIndexController', [
    '$http','$scope','$stateParams','$modal',function($http,$scope,$stateParams,$modal) {
        $scope.searchCondition = {
            name:'',
            page: 1,
            pageSize: 10,
            orderByAll:"OrderByName",//排序
            isDes:true//升序or降序,
        };
        //初始化所有图标
        var iniImg=function(){
            $scope.OrderByName="footable-sort-indicator";
            $scope.OrderByDescribe="footable-sort-indicator";
            $scope.OrderByValue="footable-sort-indicator";
            $scope.OrderByAddtime="footable-sort-indicator";

        }
        iniImg();
        $scope.OrderByName="fa-caret-down";//升降序图标
        $scope.getList = function (orderByAll) {
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
            $http.get(SETTING.ApiUrl+'/LevelConfig/SearchLevelConfig',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                console.log(data);
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
                if(data.List==""){
                    $scope.errorTip="不存在数据";
                }
                else{
                    $scope.errorTip="";
                }
            });
        };
        $scope.getList();

        //删除

        $scope.open = function(id){
            $scope.selectedId= id;
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
                $http.post(SETTING.ApiUrl+'/LevelConfig/DeleteLevelConfig',$scope.selectedId,{ 'withCredentials':true}).success(function(data){
                    if(data.Status){
                        $scope.getList();//成功刷新列表
                    }
                    else{
                        $scope.errorTip=data.Msg;

                }})
            })
        }

    }
]);

angular.module("app").controller('configureSetCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.LevelConfig = {
        Id: 0,
        Name:'',
        Value:'',
        Describe:''
    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/LevelConfig/DoCreate',$scope.LevelConfig,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.indexset");
            }else{
                alert(data.Msg);
            }

        });
    }
}]);

angular.module("app").controller('configureSetEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/LevelConfig/GetLevelConfig/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.LevelConfig =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/LevelConfig/DoEdit',$scope.LevelConfig,{
        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.configure.indexset");
            }else{
                alert(data.Msg);
            }
        });
    }

}]);






