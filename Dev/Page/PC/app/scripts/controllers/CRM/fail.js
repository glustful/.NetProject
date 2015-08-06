
/**
 * Created by yangdingpeng on 2015/5/15.
 */

//region 洽谈失败信息
angular.module("app").controller('FailListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈失败",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip="当前不存在洽谈失败业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);
//endregion

//region 推荐失败详细信息
angular.module("app").controller('FailDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
            console.log(data);
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/AdminRecom/PassAudit',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    console.log(data.Msg);
                }else{
                    alert(data.Msg);
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion


//region 带客洽谈失败相关信息
angular.module("app").controller('DKFailListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈失败",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip="当前不存在洽谈失败业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);
//endregion


//region  获取带客失败详细信息以及业务变更
angular.module("app").controller('DKFailDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
            console.log(data);
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/BrokerLeadClient/UpdateLeadClient',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    console.log(data.Msg);
                }else{
                    alert(data.Msg);
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion
