/**
 * Created by Yunjoy on 2015/8/7.
 */

app.controller('ModalInstanceCtrl', ['$scope', '$modalInstance', 'statusModal', '$state',
    function($scope, $modalInstance, statusModal,$state) {
    $scope.statusModal = statusModal;

    $scope.ok = function () {
        $modalInstance.close('ok');
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
;