/**
 * Created by huangxiuyu on 2015/10/24.
 */
app.controller('selectaddress',['$http','$scope','$stateParams','AuthService','$state',function($http,$scope,$stateParams,AuthService,$state){
    $scope.currentuser= AuthService.CurrentUser();
    $scope.searchCondition={
        Adduser:$scope.currentuser.UserId
    };
    if( $scope.currentuser==undefined ||  $scope.currentuser=="")
    {
        $state.go("page.login");//调到登录页面
    }
    $scope.getAddress=function(){
        $http.get(SETTING.ApiUrl+'/MemberAddress/Get/',{params:$scope.searchCondition,'withCredentials':true}).
            success(function(data){

                $scope.list=data.List;
               // console.log(data);
            })};
    $scope.getAddress();
    $scope.productId=$stateParams.productId;
    $scope.count=$stateParams.count;
    $scope.gotoOrder =function(id)
    {
       // alert(id);
        $state.go("page.order",{productId:$scope.productId,count:$scope.count,memberaddreid:id});

    }

}]);

