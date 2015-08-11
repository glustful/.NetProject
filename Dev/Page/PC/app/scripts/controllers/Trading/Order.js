/**
 * Created by AddBean on 2015/5/13 0013.
 */
//region 获取所有订单
angular.module("app").controller('orderController', [
    '$http', '$scope', function ($http, $scope) {
        //默认初始化推荐订单；
        $scope.searchCondition={
            page:1,
            pageSize:10,
            type:'推荐订单',
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序
        }
        var iniImg=function(){
            $scope.OrderByPrice="footable-sort-indicator";
            $scope.OrderByCommission="footable-sort-indicator";
            $scope.OrderByDealcommission="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderById="fa-caret-down";//升降序图标
        $scope.UpOrDownImgClass="fa-caret-down";
        var getOrderList=function(orderByAll){
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
                if($scope.searchCondition.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.searchCondition.isDes=false;//则变成升序
                }
                else if($scope.searchCondition.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.searchCondition.isDes=true;
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
//endregion
//region 获取洽谈后的订单
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
        var iniImg=function(){
            $scope.OrderByPrice="footable-sort-indicator";
            $scope.OrderByRecCommission="footable-sort-indicator";
            $scope.OrderByCommission="footable-sort-indicator";
            $scope.OrderByDealcommission="footable-sort-indicator";
            $scope.OrderByAddtime="footable-sort-indicator";
        }
        iniImg();


        var getOrderList=function(orderByAll){
            if(orderByAll!=undefined){
                $scope.order.orderByAll=orderByAll ;
                if($scope.order.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.order.isDes=false;//则变成升序
                }
                else if($scope.order.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.order.isDes=true;
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
//endregion
