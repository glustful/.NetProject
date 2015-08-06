
/**
 * Created by yangdingpeng on 2015/5/12.
 */
//region 推荐待审核信息
angular.module("app").controller('WaitListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"审核中",
            Brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderById',
            isDes:true
};

        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderByPhone="footable-sort-indicator";
            $scope.OrderByBrokername="footable-sort-indicator";
            $scope.OrderByAddtime="footable-sort-indicator";
            $scope.OrderByProjectname="footable-sort-indicator";
            $scope.OrderByBrokerlevel="footable-sort-indicator";
        }
        iniImg();
var getTagList = function(orderByAll) {
    $scope.OrderById="fa-caret-down";
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
            $scope.errorTip="当前没有需要审核的推荐信息";
        }
        $scope.searchCondition.page=data.condition1.Page;
        $scope.searchCondition.PageCount=data.condition1.PageCount;
        $scope.searchCondition.totalCount=data.totalCont1;

    });
};
    $scope.getList = getTagList;
    getTagList();
}]);
//endregion

//region 推荐待审核详细信息以及流程变更操作
angular.module("app").controller('ARDetialController',[
    '$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
        });
        //获取带客人员列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/WaiterList',{
            'withCredentials':true
        }).success(function (data) {
            $scope.WaiterList = data;
        });
        //获取驻场秘书列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/SecretaryList',{
            'withCredentials':true
        }).success(function (data) {
            $scope.SecretaryList = data;
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
                    $state.go('page.CRM.WaitCheck.index');
                    console.log(data.Msg);
                }else{
                    alert(data.Msg);
                    $state.go('page.CRM.WaitCheck.index');
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion
