/**
 * Created by yangdingpeng on 2015/5/15.
 */

//推荐列表
angular.module("app").controller('SuccessListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈成功",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{params:$scope.searchCondition}).success(function(data){
                $scope.Brokerlist = data.list1;
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
                console.log($scope.Brokerlist);
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);

//详细信息
angular.module("app").controller('SuccessDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id).success(function (data) {
            $scope.ARDetialModel = data;
            console.log(data);
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
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);

//详细信息
angular.module("app").controller('BRECPayController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
//        $http.get(SETTING.ApiUrl + '/AdminPay/GetAuditDetail/' + $stateParams.id).success(function (data) {
//            $scope.ARDetialModel = data;
//            console.log(data);
//        });
        $scope.PayInfo = {
            Id:$stateParams.id,
            Name:"",
            Statusname:"洽谈成功",
            Describe:"",
            Amount:""
        };

        //变更用户状态
        $scope.SetPay=function(){
            $http.post(SETTING.ApiUrl + '/AdminPay/SetPay',$scope.PayInfo,{
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

//详细信息
angular.module("app").controller('BLPayController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
//        $http.get(SETTING.ApiUrl + '/AdminPay/GetAuditDetail/' + $stateParams.id).success(function (data) {
//            $scope.ARDetialModel = data;
//            console.log(data);
//        });
        $scope.PayInfo = {
            Id:$stateParams.id,
            Name:"",
            Statusname:"上访成功",
            Describe:"",
            Amount:""
        };

        //变更用户状态
        $scope.SetPay=function(){
            $http.post(SETTING.ApiUrl + '/AdminPay/SetPay',$scope.PayInfo,{
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