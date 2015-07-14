/**
 * Created by chenda on 2015/7/9.
 */

/*======================================查询所有提现信息==============================================================*/
angular.module("app").controller('playMoney',[
    '$http','$scope',function($http,$scope){
        $scope.searchCondition = {
            page:1,
            pageSize:10
        };
        var getTagList = function(){
            $http.get(SETTING.ApiUrl+'/BrokerWithdraw/GetBrokerWithdraw',{
                params:$scope.searchCondition
            }).success(function(data){
                $scope.BrokerWithdraw = data.List;
                if(data.List == ""){
                    $scope.errorTip="当前不存在提现信息";
                }

                $scope.searchCondition.page=data.condition.Page;
                $scope.searchCondition.PageCount=data.condition.PageCount;
                $scope.totalCount=data.totalCont;
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
])
/*===================================================   ==============================================================*/
/*===================================根据经纪人ID查询提现明细=====================================================*/
angular.module("app").controller('playMoneyDetails',[
    '$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService){

        $http.get(SETTING.ApiUrl+ '/BrokerWithdrawDetail/GetBrokerWithdrawDetailByBrokerWithdrawId?id='+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.BrokerWithdrawDetail = data.List;
            $scope.PayInfo.Ids=data.Ids;
            $scope.PayInfo.BrokeAccountId = data.BrokeAccountId;
            console.log( $scope.BrokerWithdrawDetail);
        });
        ////////////////////////////////////打款款项表单//////////////////////////////////////////////////////////
        $scope.PayInfo = {
            Id:$stateParams.id,
            Ids:"",
            BrokeAccountId:"",
            Describe:"",
            Name:"",
            Accountantid:"",
            Upuser:"",
            Adduser:""
        };
        $scope.currentUser=AuthService.CurrentUser();
        $scope.PayInfo.Accountantid = $scope.currentUser.UserId;
        $scope.PayInfo.AddUser = $scope.currentUser.UserId;
        $scope.PayInfo.Upuser = $scope.currentUser.UserId;
        ///////////////////////////////////确认打款////////////////////////////////////////////////////////////////////
        ///根据提现明细表里面的提现类型，分别向带客打款表以及推荐打款表里面插入数据,其中 0 表示带客，1表示推荐////////
            $scope.SetPay=function(){
                $http.post(SETTING.ApiUrl + '/AdminPay/SetPay',$scope.PayInfo,{
                    'withCredentials':true
                }).success(function(data){
                    if(data.Status){
                        alert(data.Msg);
                    }else{
                        console.log($scope.PayInfo);
                        alert(data.Msg);
                    }
                });
            };
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
])
/*==================================================================================================================*/
