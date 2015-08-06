/**
 * Created by AddBean on 2015/5/13 0013.
 */
angular.module("app").controller('orderController', [
    '$http', '$scope', function ($http, $scope) {
        //默认初始化推荐订单；

        $scope.searchCondition={
            page:1,
            pageSize:10,
            type:'推荐订单',
            orderByAll:"OrderByAddtime",//排序
            isDes:true//升序or降序
        }
        $scope.UpOrDownImgClass="fa-caret-down";
        var getOrderList=function(orderByAll){
            if(orderByAll!=undefined) {
                $scope.searchCondition.orderByAll=orderByAll ;
                if ($scope.searchCondition.isDes == true )//如果为降序，
                {
                    $scope.UpOrDownImgClass = "fa-caret-up";//改变成升序图标
                    $scope.searchCondition.isDes = false;//则变成升序
                }
                else if ($scope.searchCondition.isDes == false) {
                    $scope.UpOrDownImgClass = "fa-caret-down";
                    $scope.searchCondition.isDes = true;
                }
            }

            $http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders',{params:$scope.searchCondition,'withCredentials':true}).success(function (data) {
                $scope.rowCollectionBasic = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        }
        $scope.getList=getOrderList;
        getOrderList()


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
                $scope.searchCondition.type="推荐订单",
                    getOrderList()
                //$http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=推荐订单',{'withCredentials':true}).success(function (data) {
                //    $scope.rowCollectionBasic = data.List;
                //});
            }else if(vm.selectVal == 1) {
                //$http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=带客订单',{'withCredentials':true}).success(function (data) {
                //    $scope.rowCollectionBasic = data.List;
                //});
                $scope.searchCondition.type="带客订单",
                    getOrderList()
            }
            else {
                //$http.get(SETTING.ApiUrl + '/order/getAllRecommonOrders?type=成交订单',{'withCredentials':true}).success(function (data) {
                //    $scope.rowCollectionBasic = data.List;
                //});
                $scope.searchCondition.type="成交订单",
                    getOrderList()
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
angular.module("app").controller('NegotiateOrderController', [
    '$http', '$scope', function ($http, $scope) {
        //获取洽谈后的订单
        $scope.order={
            status:1,
            page: 1,
            pageSize: 10,
            orderByAll:"OrderByAddTime",//排序
            isDes:true//升序or降序
        };
        $scope.UpOrDownImgClass="fa-caret-down";

        var getOrderList=function(orderByAll){
            if(orderByAll!=undefined) {
                $scope.order.orderByAll=orderByAll ;
                if ($scope.order.isDes == true )//如果为降序，
                {
                    $scope.UpOrDownImgClass = "fa-caret-up";//改变成升序图标
                    $scope.order.isDes = false;//则变成升序
                }
                else if ($scope.order.isDes == false) {
                    $scope.UpOrDownImgClass = "fa-caret-down";
                    $scope.order.isDes = true;
                }
            }
            $http.get(SETTING.ApiUrl+'/order/GetNegotiateOrders',{
                params:$scope.order,'withCredentials':true
            }).success(function(data){
                $scope.List=data.OrderList;
                $scope.order.page=data.Condition.Page;
                $scope.order.pageSize=data.Condition.PageCount;
                $scope.totalCount=data.TotalCount
            })
        }
        getOrderList();
        $scope.getList=getOrderList;
    }]);

