/**
 * Created by lhl on 2015/5/16 经纪人管理
 */
angular.module("app").controller('agentmanagerIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"经纪人",
            page: 1,
            pageSize: 10,
            state:2,//经纪人状态，1正常，0删除，-1注销
            orderByAll:"OrderByUserRegtime",//排序
            isDes:true//升序or降序
        };
        $scope.UpOrDownImgClass="fa-caret-down";//升降序图标
        $scope.SImg=SETTING.ImgUrl;//图片服务器基础路径
//------------------------获取经纪人列表 start--------------------
        var page= 0,howmany=0;
        $scope.getList  = function(orderByAll) {

            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
            if($scope.searchCondition.isDes==true)//如果为降序，
            {
                $scope.UpOrDownImgClass="fa-caret-up";//改变成升序图标
                $scope.searchCondition.isDes=false;//则变成升序
            }
            else if($scope.searchCondition.isDes==false)
            {
                $scope.UpOrDownImgClass="fa-caret-down";
                $scope.searchCondition.isDes=true;
            }
            }
            if($scope.searchCondition.phone==undefined)
            {$scope.searchCondition.phone="";}
            $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                console.log(data);
                if(data.List.length>0) {
                    console.log(data);
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
//-------------------------获取经纪人列表 end---------------------
        //初始化page
        $scope.initPage=function(){
            $scope.searchCondition.page=1;
        }
        $scope.getList();

//-------------------------删除经纪人 start----------------------
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
//-------------------------删除经纪人 end------------------------

//-----------------------注销经纪人 start------------------------
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
                $http.post(SETTING.ApiUrl+'/BrokerInfo/CancelBroker?id='+id+"&btnname=" +btnname,{
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
//-----------------------注销经纪人 end--------------------------
    }
]);


angular.module("app").controller('agentmanagerDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
    $scope.SImg=SETTING.ImgUrl;//图片服务器基础路径

//----------------根据经纪人id获取经纪人信息 start-----------------
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBrokerByAgent?id=' + $stateParams.userid,{
        'withCredentials':true
    }).success(function(data){
        if(data.List.State==1)
        {data.List.State="正常"}
        else if(data.List.State==0)
        {data.List.State="删除"}
        else if(data.List.State==-1)
        {data.List.State="注销"}
        $scope.BrokerModel =data.List;
    });
//----------------根据经纪人id获取经纪人信息 end-----------------------

//----------------查询该经纪人出入账信息 start-------------------------
    $scope.searchCRZCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getCRZList  = function() {
        $http.get(SETTING.ApiUrl+'/BrokeAccount/GetPointDetailListByUserId',{
            params:$scope.searchCRZCondition,
            'withCredentials':true
        }).success(function(data){
if(data.totalCount>0){
            $scope.listCRZ = data.List;
            $scope.searchCRZCondition.page = data.Condition.Page;
            $scope.searchCRZCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountCRZ = data.totalCount;
    $scope.visibleAccount=true;
    $scope.tipsAccount="";
}
            else{
            $scope.visibleAccount=false;
            $scope.tipsAccount="当前没有出入账数据";
}
        });
    };
    $scope.getCRZList();
//----------------查询该经纪人出入账信息 end-------------------------

//----------------根据经纪人id查询提现明细 start---------------------
    $scope.searchTXCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getTXList  = function() {
        $http.get(SETTING.ApiUrl+'/BrokerWithdrawDetail/GetBrokerWithdrawDetailListByUserId',{
            params:$scope.searchTXCondition,
            'withCredentials':true
        }).success(function(data){
if(data.totalCount>0) {
    $scope.listTX = data.List;
    $scope.searchTXCondition.page = data.Condition.Page;
    $scope.searchTXCondition.pageSize = data.Condition.PageCount;
    $scope.totalCountTX = data.totalCount;
    $scope.visibleDraw=true;
    $scope.tipsDraw="";
}
else{
    $scope.visibleDraw=false;
    $scope.tipsDraw="当前没有提现数据";

}
        });
    };
    $scope.getTXList();
//----------------根据经纪人id查询提现明细 end---------------------
//----------------根据经纪人id查询银行卡明细 start-----------------
    $scope.searchBankCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getBankList  = function() {
        $http.get(SETTING.ApiUrl+'/BankCard/SearchBankCardsByUserID',{
            params:$scope.searchBankCondition,
            'withCredentials':true
        }).success(function(data){

            if(data.totalCount1>0) {
                $scope.listBank = data.List;
                $scope.searchBankCondition.page = data.Condition.Page;
                $scope.searchBankCondition.pageSize = data.Condition.PageCount;
                $scope.totalCountBank = data.totalCount1;
                $scope.visibleBank=true;
                $scope.tipsBank="";
            }
            else{
                $scope.visibleBank=false;
                $scope.tipsBank="当前没有绑定任何银行卡";

            }
        });
    };
    $scope.getBankList();
//----------------根据经纪人id查询提现明细 end---------------------


//----------------根据经纪人id查询积分明细 start---------------------
    $scope.searchJFCondition = {
        userId: $stateParams.userid,
        page: 1,
        pageSize: 10
    };
    $scope.getJFList  = function() {
        $http.get(SETTING.ApiUrl+'/PointDetail/GetPointDetailByUserId',{
            params:$scope.searchJFCondition,
            'withCredentials':true
        }).success(function(data){
            if(data.totalCount>0){
            $scope.listJF = data.List;
            $scope.searchJFCondition.page = data.Condition.Page;
            $scope.searchJFCondition.pageSize = data.Condition.PageCount;
            $scope.totalCountJF = data.totalCount;
                $scope.visibleJF=true;
                $scope.tipsJF="";
            }
            else{
                $scope.visibleJF=false;
                $scope.tipsJF="当前没有积分数据";
            }
        });
    };
    $scope.getJFList();
//----------------根据经纪人id查询提现明细 end---------------------
}]);