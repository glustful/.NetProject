/**
 * Created by yangdingpeng on 2015/5/15.
 */

//推荐列表//带客列表
angular.module("app").controller('TalkingListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈中",
            Brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderByUptime',
            isDes:true
        };
        $scope.UpOrDownImgClass='fa-caret-down';
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
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip="当前没有洽谈中的业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
        ////////////////////////带客洽谈列表////////////////////////////////////
        var  getTagList1 =  function(){
            $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetLeadCientInfoByBrokerName',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist1 = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip = "当前没有洽谈中的业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList1 = getTagList1;
        getTagList1();
        ///////////////////////////////////////////////////////////////////////
    }
]);

//详细信息
angular.module("app").controller('TaklDetialController',[
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
                    alert(data.Msg);
                    $state.go('page.CRM.talking.index');
                }
            });
        };
    }
]);