/**
 * Created by Administrator on 2015/5/26.
 */

angular.module('app').controller('LogoutControl',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.Logout = function(){
        AuthService.doLogout(function(){
            $state.go('page.settings');
        },function(){
            //todo:ʧ��ʱ����ʾ������Ϣ
        })
    }
}]);