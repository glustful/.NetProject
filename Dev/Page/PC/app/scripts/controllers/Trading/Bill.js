/**
 * Created by AddBean on 2015/5/10 0010.
 */
angular.module("app").controller('billController', [
    '$http', '$scope', function ($http, $scope) {
        $scope.CFBill = {
            page: 1,
            pageSize: 10
        }
        var getBillList = function () {
            $http.get(SETTING.ApiUrl + '/bill/GetAdminBill',{params:$scope.CFBill}).success(function (data) {
                $scope.rowCollectionBasic = data.AdminBill;
                $scope.CFBill.page = data.Condition.Page;
                $scope.CFBill.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.BillCount;
            });
        }
            $scope.getList = getBillList;
            getBillList();

//        var vm = $scope.vm = {};
//        vm.optionsData = [
//            {
//                id: 0,
//                title: "创富宝"
//            },
//            {
//                id: 1,
//                title: "经纪人"
//            },            {
//                id: 1,
//                title: "地产商"
//            }
//        ];
//        //点击选择订单状态；
//        $scope.selectChange = function () {
//            //添加了ng-change事件来试试id值的输出
//            if (vm.selectVal == 0) {//查询创富宝
//                $http.get(SETTING.TradingApiUrl + '/bill/GetAdminBill').success(function (data) {
//                    $scope.rowCollectionBasic = data;
//                });
//            } else if(vm.selectVal == 1) {//查询经纪人
//                $http.get(SETTING.TradingApiUrl + '/bill/GetAgentBill').success(function (data) {
//                    $scope.rowCollectionBasic = data;
//                });
//            }else if(vm.selectVal == 2) {//查询地产商；
//                $http.get(SETTING.TradingApiUrl + '/bill/GetLandAgentBill').success(function (data) {
//                    $scope.rowCollectionBasic = data;
//                });
//            }
//        };
    }
]);
angular.module("app").controller('createBillController', [
    '$http', '$scope','$state','$stateParams', function ($http, $scope,$state,$stateParams) {
            $scope.orderId=$stateParams.orderId;
            $scope.BillModel={
                orderId:$stateParams.orderId,
                beneficiarynumber:'',
                Actualamount:'',
                remark:''
            }
        $scope.Create = function(){
            $http.post(SETTING.ApiUrl + '/Bill/CreateBillsByOrder',$scope.BillModel,{
                'withCredentials':true
            }).success(function(data){
                   if(data.Status)
                   {
                       $http.get(SETTING.ApiUrl+'/Order/EditStatus?orderId='+$scope.orderId,{'withCredentials':true}).success(function(data){
                       if(data.Status)
                       {
                          $state.go('page.Trading.order.Negotiateorder');
                       }
                       });
                   }
                else{
                       $scope.alerts=[{type:'danger',msg:data.Msg}];
                   }
            });
        }
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };
    }])
app.filter('dateFilter',function(){
    return function(date){
        return FormatDate(date);
    }
})
function FormatDate(JSONDateString) {
    jsondate = JSONDateString.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    return date.getFullYear()
        + "-"
        + month
        + "-"
        + currentDate
        + "-"
        + date.getHours()
        + ":"
        + date.getMinutes()
        + ":"
        + date.getSeconds()
        ;

}