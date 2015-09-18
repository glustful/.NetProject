/**
 * Created by gaofengming on 2015/9/15.
 */
app.controller('login',['$scope','$state','AuthService','$ionicLoading','$timeout',function($scope,$state,AuthService,$ionicLoading,$timeout){
    $scope.user={
        userName:'',
        password:''
    };
    $scope.login = function(){
        //��¼�ɹ�����ת��������
        AuthService.doLogin($scope.user.userName,$scope.user.password,function(data){
            $ionicLoading.show({
                template:"��¼�У����Ժ�..."
            });
            $timeout(function(){
                $ionicLoading.hide();
                $state.go('page.me');
            },2000);


        },
            //��¼ʧ��
            function(data){
            $ionicLoading.show({
                template:data.Msg,
                noBackdrop:true
            });
            $timeout(function(){
                $ionicLoading.hide();
            },2000);
        })
    }
}]);