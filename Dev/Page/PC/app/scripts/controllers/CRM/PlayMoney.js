/**
 * Created by chenda on 2015/7/9.
 */

//region 获取提现信息
angular.module("app").controller('playMoney',[
    '$http','$scope','AuthService',function($http,$scope,AuthService){

        $scope.searchCondition = {
            page:1,
            pageSize:10,
            orderByAll:'OrderById',
            isDes:true
        };
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByWithdrawTime="footable-sort-indicator";
            $scope.OrderByaccacount="footable-sort-indicator";
            $scope.OrderByBrokername="footable-sort-indicator";
            $scope.OrderByState="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderById="fa-caret-down";
        var getTagList = function(orderByAll){
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll;
                if($scope.searchCondition.isDes==true){
                    $scope.searchCondition.isDes=false;
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                }
                else if($scope.searchCondition.isDes==false){
                    $scope.searchCondition.isDes=true;
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                }
            }
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

        $scope.BrokerWithdrawId=0;
        $scope.GetBrokerWithdrawById = function (WithdrawId) {
            $scope.BrokerWithdrawId=WithdrawId;
            $http.get(SETTING.ApiUrl + '/BrokerWithdraw/GetBrokerWithdrawById?id=' + WithdrawId,{'withCredentials':true}).success(function (data) {
                $scope.List = data;
                $scope.PayInfo.Id = data.ID;
            });
        };

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
    }
])
//endregion

//region  获取提现详细信息以及打款流程操作
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

    }
])
//endregion

