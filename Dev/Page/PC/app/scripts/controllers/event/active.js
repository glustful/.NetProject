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
                $http.post('http://120.55.151.12:8081/api/coupon/ActiveCoupon', {couponNum: $scope.ticketNum}, {'withCredentials': true}).success(function (data) {
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
