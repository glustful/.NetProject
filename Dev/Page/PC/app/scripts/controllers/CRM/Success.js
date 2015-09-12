
/**
 * Created by yangdingpeng on 2015/5/15.
 */

//region 推荐洽谈成功相关信息
angular.module("app").controller('SuccessListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈成功",
            brokername:"",
            page: 1,
            pageSize: 10,
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序,默认为降序
        };
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderByProjectname="footable-sort-indicator";
            $scope.OrderByUptime="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderById="fa-caret-down";
        var getTagList = function(orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
                if($scope.searchCondition.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.searchCondition.isDes=false;//则变成升序
                }
                else if($scope.searchCondition.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.searchCondition.isDes=true;
                }
            }
            $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.Brokerlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip="当前不存在洽谈成功业务";
                }
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
                console.log($scope.Brokerlist);
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);
//endregion
//region 带客洽谈成功信息
angular.module("app").controller('DKSuccessController',[
    '$http','$scope',function($http,$scope){
        $scope.searchDKCondition = {
            status:"洽谈成功",
            Brokername:"",
            page:1,
            pageSize:10,
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序,默认为降序
        };
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderByProjectname="footable-sort-indicator";
            $scope.OrderByUptime="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderById="fa-caret-down";
        var getTagList = function(orderByAll){
            if(orderByAll!=undefined){
                $scope.searchDKCondition.orderByAll=orderByAll ;
                if($scope.searchDKCondition.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.searchDKCondition.isDes=false;//则变成升序
                }
                else if($scope.searchDKCondition.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.searchDKCondition.isDes=true;
                }
            }
            $http.get(SETTING.ApiUrl+'/BrokerLeadClient/GetLeadClientInfoByBrokerName',{
               params:$scope.searchDKCondition
            }).success(function(data){
                $scope.BrokerDKlist = data.list1;
                if(data.list1 == ""){
                    $scope.errorTip="当前不存在洽谈成功业务";
                }
                $scope.searchDKCondition.page=data.condition1.Page;
                $scope.searchDKCondition.PageCount=data.condition1.PageCount;
                $scope.totalCount=data.totalCont1;
                //console.log($scope.Brokerlist);
            });
        };
        $scope.getDKList = getTagList;
        getTagList();
    }
])
//endregion

//region 获取推荐洽谈成功详细信息
angular.module("app").controller('SuccessDetialController',[
    '$http','$state','$scope','$stateParams',function($http,$state,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl+ '/AdminRecom/GetAuditDetail?id='+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
            console.log($scope.ARDetialModel);
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/AdminRecom/PassAudit',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    $state.go('page.CRM.talking.index');

                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion

////region
//angular.module("app").controller('BRECPayController',[
//   'AuthService', '$http','$scope','$stateParams',function(AuthService,$http,$scope,$stateParams) {
//
//        $scope.PayInfo = {
//            Id:$stateParams.id,
//            Name:"",
//            Statusname:"洽谈成功",
//            Describe:"",
//            Amount:"",
//            BankCard:"",
//            Accountantid:"",
//            Upuser:"",
//            Adduser:""
//        };
//        $scope.currentUser=AuthService.CurrentUser();
//        $scope.PayInfo.Accountantid = $scope.currentUser.UserId;
//        $scope.PayInfo.AddUser = $scope.currentUser.UserId;
//        $scope.PayInfo.Upuser = $scope.currentUser.UserId;
//        //变更用户状态
//        $scope.SetPay=function(){
//            $http.post(SETTING.ApiUrl + '/AdminPay/SetBREPay',$scope.PayInfo,{
//                'withCredentials':true
//            }).success(function(data){
//                if(data.Status){
//                    console.log(data.Msg);
//                }else{
//                    console.log(data.Msg);
//                }
//            });
//        };
//    }
//]);
////endregion

////详细信息
//angular.module("app").controller('BLPayController',[
//   'AuthService', '$http','$scope','$stateParams',function(AuthService,$http,$scope,$stateParams) {
//
//        $scope.PayInfo = {
//            Id:$stateParams.id,
//            Name:"",
//            Statusname:"洽谈成功",
//            Describe:"",
//            Amount:"",
//            BankCard:"",
//            Accountantid:"",
//            Adduser:"",
//            Upuser:""
//        };
//        $scope.currentUser=AuthService.CurrentUser();
//        $scope.PayInfo.Accountantid = $scope.currentUser.UserId;
//        $scope.PayInfo.AddUser = $scope.currentUser.UserId;
//        $scope.PayInfo.Upuser = $scope.currentUser.UserId;
//        //变更用户状态
//        $scope.SetPay=function(){
//            $http.post(SETTING.ApiUrl + '/AdminPay/SetBLPay',$scope.PayInfo,{
//                'withCredentials':true
//            }).success(function(data){
//                if(data.Status){
//                    alert(data.Msg);
//                }else{
//                    alert(data.Msg);
//                }
//            });
//        };
//    }
//]);

//region  获取带客洽谈成功详细信息
angular.module("app").controller('DKSuccessDetialController',[
    '$http','$state','$scope','$stateParams',function($http,$state,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/BrokerLeadClient/GetBlDetail/'+ $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.BrokerLeadClientDtail = data;
            console.log(data);
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.BrokerLeadClientDtail.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/BrokerLeadClient/UpdateLeadClient',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    alert(data.Msg);
                    $state.go('page.CRM.DKSuccess.index');

                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);
//endregion
