/**
 * Created by Yunjoy on 2015/8/17.
 */
app.controller('deployController', ['$scope', '$http','PortalService','openModal','$state',
    function($scope, $http,PortalService,openModal,$state) {
        $scope.GetSettingList=function(){
            $http.get(SETTING.ZergWcApiUrl+"/Configuration/GetDeploy",{
                params:$scope.sech,   //参数
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.list=data;
                $scope.showSpline=data.FirstCheck;
            });
        };
        $scope.GetSettingList();

        $scope.addDeploy=function(){
            $http.post(SETTING.ZergWcApiUrl+"/Configuration/PostDeploy",{
                'withCredentials':true  //跨域
            }).success(function(data){
                if(data.Status){
                    $scope.okf=function(){
                        $state.go("deploy.deploy");
                    };
                    openModal.openModal(1,"提示","添加成功","","关闭","确定",$scope.okf);
                }else{

                    openModal.openModal(1,"提示","添加失败",data.Msg,"关闭","确定");
                }
            });
        };

        $scope.changePortal=function(){
            $http.put(SETTING.ZergWcApiUrl+"/Configuration/PutFirstCheck",{
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.reStatus=data.Status;
                $scope.reMsg=data.Msg;
                $scope.showSpline=data.Object;
            });
        };
    }
]);