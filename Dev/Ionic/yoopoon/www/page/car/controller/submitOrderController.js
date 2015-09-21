/**
 * Created by Yunjoy on 2015/9/21.
 */
app.controller('submitOrderController', ['repository', '$stateParams', 'cartservice', 'orderService', function (repository, $stateParams, cart, orderService) {
    if ($stateParams.productId && $stateParams.count) {
        repository.get('product', {id: $stateParams.productId}).then(function (data) {
            $scope.model = [{
                id: data.Id,
                price: data.Price,
                name: data.Name,
                newprice: data.OldPrice,
                mainimg: data.MainImg,
                count: $stateParams.count
            }]
        })
    } else {
        $scope.model = cart.GetAllcart();
    }

    //TODO:完成地址绑定及选择
    //todo:完成生成订单并付款的逻辑
}]);