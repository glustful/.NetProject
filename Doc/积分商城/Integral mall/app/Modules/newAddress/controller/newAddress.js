/**
 * Created by YUNJOY on 2015/7/29.
 */
app.controller('newAddressController',function($scope ){
    $scope.province=false;
    $scope.down=true;
    $scope.open=function(){
        $scope.province=!$scope.province;
        $scope.down=!$scope.down;
    }
    $scope.selectProvince=function(){
        $scope.province=!$scope.province;
        $scope.down=!$scope.down;
//        $scope.selectedProvinces=selectedProvince;
    }
})