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
                if(data.List.state==0){
                    $scope.BrokerWithdraw.state="������";
                }
                if(data.List.state==1){
                    $scope.BrokerWithdraw.state="���";
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
    '$http','$scope','$stateParams',function($http,$scope,$stateParams){

        $http.get(SETTING.ApiUrl+ '/BrokerWithdrawDetail/GetBrokerWithdrawDetailListByUserId?id='+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.BrokerWithdrawDetail = data.List;
        });
    }
])
/*===============================================================================================================*/