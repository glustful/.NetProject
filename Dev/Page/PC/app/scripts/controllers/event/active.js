/**
 * Created by gaofengming on 2015/6/30.
 */

angular.module("app").controller('activeController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {

        $scope.tip = function () {
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要激活吗？";}
                }
            })};
         //  modalInstance.result.then(function(){})



    }])
