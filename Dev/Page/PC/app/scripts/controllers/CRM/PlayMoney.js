/**
 * Created by chenda on 2015/7/9.
 */

/*======================================��ѯ����������Ϣ==============================================================*/
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
                    $scope.errorTip="��ǰ������������Ϣ";
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
/*===================================���ݾ�����ID��ѯ������ϸ=====================================================*/
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
        ////////////////////////////////////�������//////////////////////////////////////////////////////////
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
        ///////////////////////////////////ȷ�ϴ��////////////////////////////////////////////////////////////////////
        ///����������ϸ��������������ͣ��ֱ�����ʹ����Լ��Ƽ����������������,���� 0 ��ʾ���ͣ�1��ʾ�Ƽ�////////
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
