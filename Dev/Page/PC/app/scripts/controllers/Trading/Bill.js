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
            };
        $http.get(SETTING.ApiUrl+'/Order/GetByOrderId?orderId='+$scope.orderId,{'withCredentials':true}).success(function(data){
            $scope.OrderDetail=data;
        })
        $scope.Create = function(){
            $http.post(SETTING.ApiUrl + '/Bill/CreateBill',$scope.BillModel,{
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