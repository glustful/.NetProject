/**
 * Created by Yunjoy on 2015/5/6.
 */
angular.module('app').controller('LoginControl',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.Login = function(){
        AuthService.doLogin($scope.user.name,$scope.user.password,function(){
            $state.go('page.Trading.product.product');
        },function(){
            //todo:ʧ��ʱ����ʾ������Ϣ
        })
    }
}]);