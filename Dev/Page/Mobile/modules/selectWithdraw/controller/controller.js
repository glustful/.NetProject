/**
 * Created by chen on 2015/7/13.
 */
app.controller('withdrawController',['$http','$scope','AuthService','$state',function($http,$scope,AuthService,$state) {
    $scope.Withdraw={
      UserId:""
    };


    $scope.currentUser=AuthService.CurrentUser();
    $scope.Withdraw.UserId = $scope.currentUser.UserId;

    var getwithdrawResult  = function() {
        $http.get(SETTING.ApiUrl+'/BrokeAccount/GetBrokeAccountByUserId/',{params:$scope.Withdraw,'withCredentials':true}).success(function(data){
            $scope.list=data.List;
        });
    };
    getwithdrawResult();
    ///
   // $scope.Ids=[];
    $scope.Ids="";

    $scope.toggle = function (Id) {
     //   $scope.Ids.push (Id);
        $scope.Ids+=Id+",";
    }

    $scope.btnClick=function(Ids){
        if (Ids != null && Ids!=undefined){
            $state.go("app.withdrawals",{Ids:Ids});
        }else{
            alert("请选择提现金额");
        }
    }
}])