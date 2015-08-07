
/**
 * Created by yangdingpeng on 2015/5/15.
 */

//region 推荐待上访信息
angular.module("app").controller('PetitionListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"等待上访",
            Brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderById',
            isDes:true
        };
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderBySecretaryName="footable-sort-indicator";
            $scope.OrderByWaiter="footable-sort-indicator";
            $scope.OrderByUptime="footable-sort-indicator";
            $scope.OrderByProjectname="footable-sort-indicator";
        }

        iniImg();
        $scope.OrderById="fa-caret-down";
        var getTagList = function(orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll;
                if($scope.searchCondition.isDes==true){
                    $scope.searchCondition.isDes=false;
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();
                    eval($scope.d);
                }
                else if($scope.searchCondition.isDes==false){
                    $scope.searchCondition.isDes=true;
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                }
            }
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if (data.list1 == ""){
                    $scope.errorTip="当前没有上访客户"
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.pageSize=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);
//endregion


//region 推荐待上访详细信息以及流程变更
angular.module("app").controller('WPDetialController',[
    '$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
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
                    alert("操作成功");
                    $state.go('page.CRM.WaitPetition.index');
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion
