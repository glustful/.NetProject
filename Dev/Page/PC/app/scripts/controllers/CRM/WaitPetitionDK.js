/****
 *
 */


//�Ϸ��б�
angular.module("app").controller('WaitVistController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"1",
            brokername:"",
            page: 1,
            pageSize: 10
        };
        /////////////////////��ȡ���ʹ��Ϸü�¼/////////////////////
        var getDKpetition = function(){
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.DKBrokerlist = data.list1;
                if (data.list1 == ""){
                    $scope.errorTip = "��ǰû�д��Ϸ¼�¼"
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
            });
        };
        $scope.getDKList = getDKpetition;
        getDKpetition();
        ////////////////////////////////////////////////////////////
    }
]);


//��ϸ��Ϣ
angular.module("app").controller('WPDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //��ȡ��ϸ��Ϣ
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
        });

        //////////////////////��ȡ���Ϸô�����ϸ��Ϣ////////////////////////////////
        $http.get(SETTING.ApiUrl + 'BrokerLeadClient/GetBlDetail' + $stateParams.id,{
            'withCredentials':true
        }).success(function(data){
            $scope.detail = data;
        });
        ///////////////////////////////////////////////////////////////////////////



        $scope.PassAudit = {
            Id:"",
            Status:""
        };


        //����û�״̬
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/AdminRecom/PassAudit',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);