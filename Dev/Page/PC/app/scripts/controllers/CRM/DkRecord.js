/**
 * Created by yangdingpeng on 2015/5/12.
 */

//获取带客列表
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
        $scope.UpOrDownImgClass="fa-caret-down";//升降序图标
        var getTagList = function(orderByAll) {

            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll;
                if($scope.searchCondition.isDes==true){
                    $scope.searchCondition.isDes=false;
                    $scope.UpOrDownImgClass='fa-caret-up'
                }
                else if($scope.searchCondition.isDes==false){
                    $scope.searchCondition.isDes=true;
                    $scope.UpOrDownImgClass='fa-caret-down'
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

        ////初始化区域列表
        //$http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=推荐订单',{'withCredentials':true}).success(function (data) {
        //    $scope.rowCollectionBasic = data;
        //});
        ////根据经纪人名字搜索该经纪人带客记录信息
        //var getRecClientByUser = function(){
        //    $http.get(SETTING.ApiUrl+'/BrokerLeadClient/SearchBrokerLeadClient',{
        //            params:$scope.searchCondition1,
        //            'withCredentials':true
        //        }).success(function(data){
        //            $scope.Brokerlist =data.list;
        //            if (data.list == ""){
        //                $scope.errorTip == "该经纪人没有带客记录信息"
        //            }
        //        });
        //};
        //$scope.getRecClientByUser= getRecClientByUser;
        //getRecClientByUser();
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
        $scope.SecretaryList = data
    });
    $scope.updateDKRecord = function(type){
        $scope.ARDetialModel.Status = type;

        //if ($scope.SecretaryId == 0 || $scope.SecretaryId=="" ||  $scope.SecretaryId==undefined)
        //{
        //    alert("驻场秘书不能为空");
        //    return;
        //}
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