/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('WaitListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"审核中",
            Brokername:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{params:$scope.searchCondition}).success(function(data){
                $scope.Brokerlist = data.list1;
                console.log(data);
                $scope.searchCondition.page=data.condition1.page;
                $scope.searchCondition.totalCount=data.condition1.totalCount;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);

//详细信息
angular.module("app").controller('ARDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id).success(function (data) {
            $scope.ARDetialModel = data;
            console.log(data);
        });
        //获取带客人员列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/WaiterList').success(function (data) {
            $scope.WaiterList = data;
            console.log(data);
        });
        //获取驻场秘书列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/SecretaryList').success(function (data) {
            $scope.SecretaryList = data;
            console.log(data);
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        $scope.passAudit1=function(){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status="等待上访";

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