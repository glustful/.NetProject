/**
 * Created by Yunjoy on 2015/8/6.
 * 公众号基本设置
 */

app.controller('baseSettingController', ['$scope', '$http','openModal','$state',
    function($scope, $http,openModal,$state) {
    $scope.sech={
        page:0,
        pageCount:0,
        pageSize:0,
        type:""
    };

    $scope.GetSettingList=function(){
        $http.get(SETTING.ZergWcApiUrl+"/Portal/GetSettingList",{
            params:$scope.sech,   //参数
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.list=data.List;
        });
    };
    $scope.GetSettingList();

    $scope.portal={
        Name:"",
        ToUserNamme:"",
        Token:"",
        AppId:"",
        AppSecret:"",
        MchId:"",
        MKey:"",
        EncodingAESKey:""
    };
    $scope.CreatePortal=function(){
        $http.post(SETTING.ZergWcApiUrl+"/Portal/PostCreatePortal",$scope.portal,{
            'withCredentials':true  //跨域
        }).success(function(data){
            if(data.Status){
                //刷新公众号选择列表
                $scope.RefreshPortalList();

                var okf=function(){
                    $state.go("deploy.baseSetting");
                };

                openModal.openModal(1,"提示","添加成功","","关闭","确定",okf);
            }else{

                openModal.openModal(1,"提示","添加失败",data.Msg,"关闭","确定");
            }
        });
    };

        //-------------- change status ---------------//

        $scope.changePortal=function(id){
            $http.put(SETTING.ZergWcApiUrl+"/Portal/PutPortalStatus",{id:id},{
                'withCredentials':true  //跨域
            }).success(function(data){
                $scope.reStatus=data.Status;
                $scope.reMsg=data.Msg;

                //此处有BUG，暂时无法将showSpline控制到页面显示，但不影响程序操作
               data.Object == 0? $scope.showSpline=true: $scope.showSpline=false;
                console.log($scope.showSpline);
            });
        };


        //-------------- delete ----------------//
    $scope.deletePortal=function(id){
        $http.delete(SETTING.ZergWcApiUrl+"/Portal/DeletePortal",{
            params:{id:id},
            'withCredentials':true  //跨域
        }).success(function(data){
            if(data.Status){
                //刷新公众号选择列表
                $scope.RefreshPortalList();

                var okf=function(){
                    $state.go("deploy.baseSetting");
                };

                openModal.openModal(1,"提示","已经删除ID为:"+id+"的公众号","","关闭",
                    "确定",okf);
                $scope.GetSettingList();
            }else{


                openModal.openModal(1,"提示","删除失败",data.Msg,"关闭","确定");
            }
        });
    };
}]);


//----------------------- create ----------------------//
app.controller('editSettingController', ['$scope', '$http', '$state','$stateParams','openModal',
    function($scope, $http,$state,$stateParams,openModal) {
    $scope.portal={
        Name:"",
        ToUserNamme:"",
        Token:"",
        AppId:"",
        AppSecret:"",
        MchId:"",
        MKey:"",
        EncodingAESKey:""
    };

    $scope.GetSettingDetial=function(){
        $http.get(SETTING.ZergWcApiUrl+"/Portal/GetSettingDetial?id="+$stateParams.id,{
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.portal={
                Id:data.Id,
                Name:data.Name,
                AppId:data.AppId,
                AppSecret:data.AppSecret,
                MchId:data.MchId,
                MKey:data.MKey,
                Token:data.Token,
                EncodingAESKey:data.EncodingAESKey,
                ToUserNamme:data.ToUserNamme
            };
        });
    };
    $scope.GetSettingDetial();

    $scope.EditPortal=function(){
        $http.put(SETTING.ZergWcApiUrl+"/Portal/PutEditPortal",$scope.portal,{
            'withCredentials':true  //跨域
        }).success(function(data){
            $scope.reStatus=data.Status;
            $scope.reMsg=data.Msg;

            if(data.Status){
                //刷新公众号选择列表
                $scope.RefreshPortalList();

                var okf=function(){
                    $state.go("deploy.baseSetting");
                };

                openModal.openModal(1,"提示","修改成功","","关闭","确定",okf);
            }else{

                openModal.openModal(1,"提示","修改失败",data.Msg,"关闭","确定");
            }
        });
    };
}]);


