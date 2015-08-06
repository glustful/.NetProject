

//region 带客待上访信息
angular.module("app").controller('WaitVistController', [
    '$http','$scope','AuthService',function($http,$scope) {
        $scope.searchCondition = {
            status:"等待上访",
            brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:'OrderById',
            isDes:true
        };
        $scope.UpOrDownImgClass='fa-caret-down';
        /////////////////////获取带客待上访记录/////////////////////
        var getDKpetition = function(orderByAll){
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
                $scope.DKBrokerlist = data.list1;
                if (data.list1 == ""){
                    $scope.errorTip = "当前没有待上仿记录"
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.pageSize=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getDKList = getDKpetition;
        getDKpetition();
        ////////////////////////////////////////////////////////////
    }
]);
//endregion


//region 带客待上访详细信息以及流程变更
angular.module("app").controller('DKDetialController',[
    '$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail/'+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.detail = data;
        });
        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.updateLead=function(enum1){
            $scope.PassAudit.Id= $scope.detail.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/BrokerLeadClient/UpdateLeadClient',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    $state.go('page.CRM.DKWaitPetition.index');
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion
