/**
 * Created by 杨波 on 2015/5/29.
 */
app.controller('partnerListController',['$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService) {
    $scope.searchCondition = {
      userId:4
    };
    //查询合伙人
    $scope.currentuser= AuthService.CurrentUser();
    var getPartnerList  = function() {
        $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed?userId='+$scope.currentuser.UserId,{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list = data.list;
        });
    };
    getPartnerList();

}
]);
//查询经纪人收到的邀请
app.controller ('searchInviteController',['$http','$scope','$stateParams','AuthService',function($http,$scope,$stateParams,AuthService){
    $scope.searchCondition={
        brokerId:0
    };
    $scope.currentuser= AuthService.CurrentUser();
    var getInvite = function(){
        $http.get(SETTING.ApiUrl + '/PartnerList/GetInviteForBroker?brokerId='+$scope.currentuser.UserId,{'withCredentials': true}).success(function (data) {
           console.log(data);

                $scope.list=data.list;


        })
    };
    getInvite();
    var agreeAdd = function(status,id){
        $http.get(SETTING.ApiUrl + '/PartnerList/SetPartner?status='+status+"&id="+id, {'withCredentials': true}).success(function (data) {

            if(data.Status){
                getInvite();
                $scope.warming=data.Msg;
            }

        })
    };
    $scope.agreeAddPartner = agreeAdd;
}
]);


