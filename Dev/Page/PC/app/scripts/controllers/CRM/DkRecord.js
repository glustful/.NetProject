/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('DkRecordController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };
        $scope.searchCondition1 = {
            userId:''
        }

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/GetBLCList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.List;
                if(data.List == ""){
                    $scope.errorTip == "不存在数据"
                }
                $scope.searchCondition.page=data.Condition.Page;
                $scope.searchCondition.PageCount=data.Condition.PageCount;
                $scope.searchCondition.totalCount=data.totalCount;
            });
        };
        $scope.getList = getTagList;
        getTagList();

        //初始化区域列表
        $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=推荐订单',{'withCredentials':true}).success(function (data) {
            $scope.rowCollectionBasic = data;
        });
        var getRecClientByUser = function(){
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/SearchBrokerLeadClient',{
                    params:$scope.searchCondition1,
                    'withCredentials':true
                }).success(function(data){
                    $scope.record =data.list;
                    if (data.list == ""){
                        $scope.errorTip == "该经纪人没有带客记录信息"
                    }

                });
        };
    }
]);

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
        $scope.SecretaryList = data;
    });
    $scope.updateDKRecord = function(type){
        $scope.ARDetialModel.Status = type;
        $http.post(SETTING.ApiUrl +'/BrokerLeadClient/UpdateLeadClient',$scope.ARDetialModel,{ 'withCredentials':true}).success(function(data){
            if(data.Status){
                $state.go('page.CRM.DkRecord.index');
            }else{
                alert(data.Msg);
            }
        });
    };
}]);