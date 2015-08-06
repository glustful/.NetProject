/**
 * Created by lhl on 2015/5/16 驻厂秘书管理
 */
angular.module("app").controller('zcIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"场秘",
            page: 1,
            pageSize: 10,
            state:2,
            orderByAll:'OrderByUserRegtime',
            isDes:true
        };
        $scope.SImg=SETTING.ImgUrl;//图片服务器基础路径
//---------------------添加驻场秘书账号 start---------------------------
        var page= 0,howmany=0;
        $scope.UpOrDownImgClass='fa-caret-down';
        $scope.getList  = function(orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll;
                if($scope.searchCondition.isDes==true){
                    $scope.searchCondition.isDes=false;
                    $scope.UpOrDownImgClass='fa-caret-up'
                }
                else if($scope.searchCondition.isDes==false){
                    $scope.searchCondition.isDes=true;
                    $scope.UpOrDownImgClass='fa-caret-down'
                }
            }

            if($scope.searchCondition.phone==undefined)
            {$scope.searchCondition.phone="";}
            $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                // alert(data.list!=null);
                if(data.List.length>0) {
                    page= $scope.searchCondition.page = data.Condition.Page;
                    howmany=data.List.length;//保存当页数据数量
                    $scope.searchCondition.pageSize = data.Condition.PageCount;
                    $scope.totalCount = data.totalCount;
                    $scope.visibleif=true;
                    $scope.tips="";
                    $scope.list=[];//清空
                    //循环绑定每一条数据，目的是让btnVisibleCan，btnVisibleDel可见与否
                    for(var i=0;i<data.List.length;i++)
                    {
                        $scope.list.push( data.List[i]);
                        if($scope.list[i].State==-1)//注销状态，注销按钮无效，删除有效
                        {
                            $scope.list[i].btnVisibleCan=false;//注销或回复按钮是否可用
                            $scope.list[i].btnVisibleDel=false;//删除按钮是否可用
                            $scope.list[i].btnname="恢复";//按钮名称，恢复或注销
                            $scope.list[i].backcolor={backgroundColor:'cadetblue'};//恢复按钮颜色
                            $scope.list[i].color={backgroundColor:'#FFEB3B'};//删除按钮颜色
                        }
                        else if($scope.list[i].State==0)//删除状态
                        {
                            $scope.list[i].btnVisibleCan=true;
                            $scope.list[i].btnVisibleDel=true;
                            $scope.list[i].btnname="恢复";
                            $scope.list[i].backcolor={backgroundColor:'lightgrey'};
                            $scope.list[i].color={backgroundColor:'lightgrey'};
                        }
                        else if($scope.list[i].State==1)//正常状态
                        {
                            $scope.list[i].btnVisibleCan=false;
                            $scope.list[i].btnVisibleDel=false;
                            $scope.list[i].btnname="注销";
                            $scope.list[i].color={color:'white'};
                            $scope.list[i].backcolor={backgroundColor:'#3F51B5'};
                            $scope.list[i].color={backgroundColor:'#FFEB3B'};
                        }

                    }
                }
                else{
                    $scope.visibleif=false;
                    $scope.tips="没有数据"
                }

            });
        };
        //初始化page
        $scope.initPage=function(){
            $scope.searchCondition.page=1;
        }
        $scope.getList();
//---------------------添加驻场秘书账号 end---------------------------

//---------------------删除驻场秘书账号 start-------------------------
        $scope.deleteBroker=function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要删除吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.post(SETTING.ApiUrl+'/BrokerInfo/DeleteBroker',id,{
                    'withCredentials':true
                }).success(function (data) {
                    // $scope.divtips=true;
                    alert(data.Msg);

                    if (data.Status) {

                        //  alert(data.Msg);
                        if(howmany==1)
                        {
                            if(page>1){
                                $scope.searchCondition.page--;}
                            else{
                                $scope.searchCondition.page=1;
                            }
                        }
                        $scope.getList();


                    }
                    else{

                        //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
            });
        }
//---------------------删除驻场秘书账号 end---------------------------

//---------------------注销驻场秘书账号 start-------------------------
        $scope.cancelBroker=function (id,btnname) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要"+btnname+"吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.post(SETTING.ApiUrl+'/BrokerInfo/CancelBroker?id='+id+"&btnname="+btnname,{
                    'withCredentials':true
                }).success(function (data) {
                    alert(data.Msg);
                    if (data.Status) {

                        if(howmany==1)
                        {
                            if(page>1){
                                $scope.searchCondition.page--;}
                            else{
                                $scope.searchCondition.page=1;
                            }
                        }
                        $scope.getList();


                    }
                    else{

                        //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
            });
        }
//---------------------注销驻场秘书账号 end---------------------------
    }
]);


angular.module("app").controller('zcDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
    $scope.SImg=SETTING.ImgUrl;//图片服务器基础路径
//----------------根据驻场秘书账号id查询相关信息 start----------------------
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBrokerByAgent?id=' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        if(data.List.State==1)
        {data.List.State="正常"}
        else if(data.List.State==0)
        {data.List.State="删除"}
        else if(data.List.State==-1)
        {data.List.State="注销"}
        $scope.BusmanModel =data.List;
    });
//----------------根据驻场秘书账号id查询相关信息 end----------------------
}]);

angular.module("app").controller('zcCreateController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){

    $scope.UserModel={

        Password:"",
        Brokername:"",
        Phone:"",
        UserType:"场秘",
        UserName:""
    };
//-----------------------添加驻场秘书账号 start-------------------------
    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/AdminRecom/AddBroker',$scope.UserModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                alert(data.Msg);
                $scope.UserModel.Password="",
                    $scope.UserModel.Brokername="",
                    $scope.UserModel.Phone="",
                    $scope.UserModel.UserName=""


            }else{
                alert(data.Msg);
            }
        });
    }
//-----------------------添加驻场秘书账号 end---------------------------
}]);