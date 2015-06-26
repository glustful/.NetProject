/**
 * Created by yangdingpeng on 2015/5/15.
 */

//上访列表
angular.module("app").controller('PetitionListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"等待上访",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if (data.list1 == ""){
                    $scope.errorTip="当前没有上访客户"
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getList = getTagList;
        getTagList();
        /////////////////////获取带客待上访记录/////////////////////
        //var getDKpetition = function(){
        //    $http.get(SETTING.ApiUrl+'BrokerLeadClient/GetLeadClientInfoByBrokerName',{
        //        params:$scope.searchCondition,
        //        'withCredentials':true
        //    }).success(function(data){
        //        $scope.DKBrokerlist = data.list1;
        //        if (data.list1 == ""){
        //            $scope.errorTip = "当前没有待上仿记录"
        //        }
        //        $scope.searchCondition.page=data.condition1.Page;
        //        $scope.searchCondition.PageCount=data.condition1.PageCount;
        //        $scope.searchCondition.totalCount=data.totalCont1;
        //    });
        //};
        //$scope.getDKList = getDKpetition;
        //getDKpetition();
        ////////////////////////////////////////////////////////////
    }
]);


//详细信息
angular.module("app").controller('WPDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
        });

        //////////////////////获取待上访带客详细信息////////////////////////////////
        $http.get(SETTING.ApiUrl + 'BrokerLeadClient/GetBlDetail' + $stateParams.id,{
            'withCredentials':true
        }).success(function(data){
            $scope.detail = data;
        });
        ///////////////////////////////////////////////////////////////////////////



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
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);