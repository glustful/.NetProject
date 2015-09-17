/**
 * Created by Yunjoy on 2015/9/16.
 */
app.service("orderService",function(repository){
    this.getOrderList = function(param){
        return repository.get("order",param);
    };

    this.submitOrder= function(param){
        return repository.post("order",param);
    };

    this.submitOrderFromCart =function(){
       //TODO:实现这里
    };

    this.updateOrder = function(id,status){
        var param = {Id:id,Status:status};
        return repository.put("order",param);
    }
});