/**
 * Created by chenda on 2015/7/9.
 */

/*======================================获取提现信息==============================================================*/
angular.module("app").controller('playMoney',[
    '$http','$scope','AuthService',function($http,$scope,AuthService){

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

            });
        };
        $scope.getList = getTagList;
        getTagList();


        //////////////////////////////////////////////////////////////////////////////////////////
        $scope.BrokerWithdrawId=0;
        $scope.GetBrokerWithdrawById = function (WithdrawId) {
            $scope.BrokerWithdrawId=WithdrawId;
            $http.get(SETTING.ApiUrl + '/BrokerWithdraw/GetBrokerWithdrawById?id=' + WithdrawId,{'withCredentials':true}).success(function (data) {
                $scope.List = data;
                $scope.PayInfo.Id = data.ID;
            });
        };
        //////////////////////////////////////////////////////////////////////////////////////////////
        $scope.PayInfo = {
            Id:"",
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
        ///////////////////////////////////ȷ�ϴ��////////////////////////////////////////////////////////////////////
        $scope.SetPay=function(){
            $http.post(SETTING.ApiUrl + '/AdminPay/SetPay',$scope.PayInfo,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    console.log($scope.PayInfo);
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
/*===================================================   ==============================================================*/
/*===================================提取提现详细信息=====================================================*/
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
        //////////////////////////////////////////////////////////////////////////////////////////////
        $scope.PayInfo = {
            BrokerWithdrawId:$stateParams.id,
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
        ///////////////////////////////////ȷ�ϴ��////////////////////////////////////////////////////////////////////
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
