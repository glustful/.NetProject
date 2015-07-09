/**
 * Created by chen on 2015/7/9.
 */


angular.module("app").controller('playMoney',[
    '$http','$scope',function($http,$scope){
        $scope.searchCondition = {
            page:1,
            pageSize:10
        };
        var getTagList = function(){
            $http.get(SETTING.ApiUrl+'/BrokerWithdraw/GetBrokerWithdraw',{
                params:$scope.searchCondition
            }).success(function(data){
                $scope.BrokerWithdraw = data.List;
                if(data.List == ""){
                    $scope.errorTip="当前不存在提现信息";
                }
                if(data.List.state==0){
                    $scope.BrokerWithdraw.state="处理中";
                }
                if(data.List.state==1){
                    $scope.BrokerWithdraw.state="完成";
                }
                $scope.searchCondition.page=data.condition.Page;
                $scope.searchCondition.PageCount=data.condition.PageCount;
                $scope.totalCount=data.totalCont;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
])