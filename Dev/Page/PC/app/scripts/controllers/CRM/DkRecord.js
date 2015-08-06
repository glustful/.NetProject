
/**
 * Created by yangdingpeng on 2015/5/12.
 */
//region 获取带客记录信息
angular.module("app").controller('DkRecordController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"预约中",
            Brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderById',
            isDes:true
        };
        $scope.searchCondition1 = {
            userId:''
        }
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderByPhone="footable-sort-indicator";
            $scope.OrderByBrokername="footable-sort-indicator";
            $scope.OrderByProjectname="footable-sort-indicator";
            $scope.OrderByAppointmenttime="footable-sort-indicator";
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
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1==""){

                    $scope.errorTip ="当前不存在预约记录"
                }
                console.log( $scope.Brokerlist);
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

//region 获取带客列表详细信息以及带客流程变更操作
angular.module("app").controller('DKRDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
//个人信息
    $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail?id=' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.ARDetialModel =data;
    });
    //获取驻场秘书列表
    $http.get(SETTING.ApiUrl + '/AdminRecom/SecretaryList',{
        'withCredentials':true
    }).success(function (data) {
        $scope.SecretaryList = data
    });
    $scope.updateDKRecord = function(type){
        $scope.ARDetialModel.Status = type;

        $http.post(SETTING.ApiUrl +'/BrokerLeadClient/UpdateLeadClient',$scope.ARDetialModel,{ 'withCredentials':true}).success(function(data){
            if(data.Status){
                alert(data.Msg);
                $state.go('page.CRM.DkRecord.index');
            }else{
                alert(data.Msg);
                $state.go('page.CRM.DkRecord.index');
            }
        });
    };
}]);
//endregion
