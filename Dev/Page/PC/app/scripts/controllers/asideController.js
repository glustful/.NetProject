/**
 * Created by gaofengming on 2015/6/17.
 */
angular.module('app').controller('asideController',['$scope','AuthService',function($scope,AuthService){
    $scope.currentUser=AuthService.CurrentUser();
}])