/**
 *
 */

//带客列表
angular.module("app").controller('DKTalkingList', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"2",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        ////////////////////////带客洽谈列表////////////////////////////////////
        var  getTagList1 =  function(){
            $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
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
        $scope.getList = getTagList1;
        getTagList1();
        ///////////////////////////////////////////////////////////////////////
    }
]);

//详细信息
angular.module("app").controller('DKTaklDetial',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail/' + $stateParams.id,{
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

            $http.post(SETTING.ApiUrl + '/BrokerLeadClient/UpdateLeadClient',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);