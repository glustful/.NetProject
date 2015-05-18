/**
 * Created by AddBean on 2015/5/10 0010.
 */
angular.module("app").controller('billController', [
    '$http', '$scope', function ($http, $scope) {
        //默认初始化推荐订单；
        $http.get(SETTING.TradingApiUrl + '/bill/GetAdminBill').success(function (data) {
            $scope.rowCollectionBasic = data;
        });

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