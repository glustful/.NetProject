/**
 * Created by Administrator on 2015/9/10.
 */
angular.module("app").controller('ModalInstanceCtrl', ['$scope', '$modalInstance','msg',function($scope, $modalInstance,msg) {
    $scope.msg = msg;
    $scope.ok = function () {
        $modalInstance.close();
    }
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}]);
