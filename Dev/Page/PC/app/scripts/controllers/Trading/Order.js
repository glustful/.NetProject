/**
 * Created by AddBean on 2015/5/13 0013.
 */
angular.module("app").controller('orderController', [
    '$http', '$scope', function ($http, $scope) {
        //默认初始化推荐订单；
        $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=推荐订单',{'withCredentials':true}).success(function (data) {
            $scope.rowCollectionBasic = data;
        });
        var vm = $scope.vm = {};
        vm.optionsData = [
            {
                id: 0,
                title: "推荐订单"
            },
            {
                id: 1,
                title: "带客订单"
            },
            {
                id: 2,
                title: "成交订单"
            }
        ];

        //点击选择订单状态；
        $scope.selectChange = function () {
            //添加了ng-change事件来试试id值的输出
            if (vm.selectVal == 0) {
                $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=推荐订单',{'withCredentials':true}).success(function (data) {
                    $scope.rowCollectionBasic = data;
                });
            }else if(vm.selectVal == 1) {
                $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=带客订单',{'withCredentials':true}).success(function (data) {
                    $scope.rowCollectionBasic = data;
                });
            }
            else {
                $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=成交订单',{'withCredentials':true}).success(function (data) {
                    $scope.rowCollectionBasic = data;
                });
            }
        };

        //审查；
        $scope.Pass = function (OrderId, status) {
            $http.get(SETTING.ApiUrl + '/order/EditOrderStatus?orderId=' + OrderId + "&status=" + status,{'withCredentials':true}).success(function (data) {
                alert(data);
                if (vm.selectVal == 1) {
                    $http.get(SETTING.ApiUrl + '/order/getAllDealOrders',{'withCredentials':true}).success(function (data) {
                        $scope.rowCollectionBasic = data;
                    });
                } else{
                    $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders',{'withCredentials':true}).success(function (data) {
                        $scope.rowCollectionBasic = data;
                    });
                }
            });

        }

    }
]);