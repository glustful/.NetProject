/**
 * Created by Yunjoy on 2015/8/5.
 */

app.controller('keyResController', ['$scope', '$http', '$state', 'openModal',
    function($scope, $http,$state,openModal) {
        $scope.sech={
            page:0,
            pageCount:0,
            pageSize:0,
            type:"",
            PortalId:$scope.activedPortal.Id
        };

        $scope.GetKeyResList=function(){
            $http.get(SETTING.ZergWcApiUrl+"/AutoRes/GetKeyRes",{
                params:$scope.sech,   //参数
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.list=data.List;
            });
        };
        $scope.GetKeyResList();

        $scope.deleteKeyRes=function(id){
            $http.delete(SETTING.ZergWcApiUrl+"/AutoRes/DeleteKeyRes",{
                params:{id:id},
                'withCredentials':true  //跨域
            }).success(function(data){
                if(data.Status){
                    //刷新公众号选择列表
                    $scope.GetKeyResList();

                    var okf=function(){
                        $state.go("app.autoRes.keyRes");
                    };

                    openModal.openModal(1,"提示","已经删除ID为:"+id+"的公众号","","关闭",
                        "确定",okf);
                    $scope.GetKeyResList();
                }else{


                    openModal.openModal(1,"提示","删除失败",data.Msg,"关闭","确定");
                }
            });
        };
    }]);

//----------------------- createKeyRes ----------------------//
app.controller('createKeyResController', ['$scope', '$http','$state','openModal',
    function($scope, $http,$state,openModal) {
        $scope.keyResModel={
            key:"",
            content:"",
            PortalId:$scope.activedPortal.Id
        };

        $scope.createKeyRes=function(){
            $http.post(SETTING.ZergWcApiUrl+"/AutoRes/PostKeyRes",$scope.keyResModel,{
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.reStatus=data.Status;
                $scope.reMsg=data.Msg;

                if(data.Status){
                    //刷新公众号选择列表
                    $scope.RefreshPortalList();

                    var okf=function(){
                        $state.go("app.autoRes.keyRes");
                    };

                    openModal.openModal(1,"提示","添加成功","","关闭","确定",okf);
                }else{

                    openModal.openModal(1,"提示","添加失败",data.Msg,"关闭","确定");
                }
            });
        };
    }]);

//----------------------- editKeyRes ----------------------//
app.controller('editKeyResController', ['$scope', '$http','$state','openModal','$stateParams',
    function($scope, $http,$state,openModal,$stateParams) {
        $scope.sechKeyResDital=function() {

            $http.get(SETTING.ZergWcApiUrl + "/AutoRes/GetKeyResDeital?id=" + $stateParams.id, {
                'withCredentials': true  //跨域
            }).success(function (data) {
                $scope.keyResModel=data;
                console.log($scope.keyResModel);
            });
        };
        $scope.sechKeyResDital();

        $scope.editKetRes=function(){

            $http.put(SETTING.ZergWcApiUrl+"/AutoRes/PutKeyRes",$scope.keyResModel,{
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.reStatus=data.Status;
                $scope.reMsg=data.Msg;

                if(data.Status){
                    //刷新公众号选择列表
                    $scope.RefreshPortalList();

                    var okf=function(){
                        $state.go("app.autoRes.keyRes");
                    };

                    openModal.openModal(1,"提示","修改成功","","关闭","确定",okf);
                }else{

                    openModal.openModal(1,"提示","修改失败",data.Msg,"关闭","确定");
                }
            });
        };
    }]);