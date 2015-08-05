/**
 *
 */

//获取洽谈中业务
angular.module("app").controller('DKTalkingList', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈中",
            Brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderByUptime',
            isDes:true
        };

        ////////////////////////���Ǣ̸�б�////////////////////////////////////
        $scope.UpOrDownImgClass='fa-caret-down';
        var  getTagList1 =  function(orderByAll){
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
            $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip = "当前不存在洽谈中的业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList1;
        getTagList1();
        ///////////////////////////////////////////////////////////////////////
    }
]);

//获取洽谈中业务详细
angular.module("app").controller('DKTaklDetial',[
    '$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
        $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail/'+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };
        $scope.UpdateLeadClient=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/BrokerLeadClient/UpdateLeadClient',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    $state.go('page.CRM.DKtalking.index');
                }
            });
        };
    }
]);