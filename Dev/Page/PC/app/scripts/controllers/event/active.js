/**
 * Created by gaofengming on 2015/6/30.
 */

angular.module("app").controller('activeController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.ticketNum='';
        $scope.tip = function () {
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要激活吗？";
                    }
                }
            })
            modalInstance.result.then(function () {
                $http.post(SETTING.eventApiUrl+'/coupon/ActiveCoupon', {couponNum: $scope.ticketNum}, {'withCredentials': true}).success(function (data) {
                    if (data.Status) {
                        var modalInstance = $modal.open({
                            templateUrl: 'myModalContent.html',
                            controller: 'ModalInstanceCtrl',
                            resolve: {
                                msg: function () {
                                    return data.Msg;
                                }
                            }
                        });
                    }


                })
            })
        };


    }]);
angular.module("app").controller('CouponController', [
    '$http','$scope','$state',function($http,$scope,$state) {
       $scope.ti=function(){
  //  alert("fsd");
        $http.get(SETTING.eventApiUrl+'/Coupon/GetUserAllCoupon?username='+$scope.username,{
           'withCredentials':true
        }).success(function(data){
            if(data){
                $scope.Coupon=data.list;
                $scope.alerts=[{type:'danger',msg:data.Msg}];
            }

        });




       }

    }]);

