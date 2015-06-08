/**
 * Created by 杨波 on 2015/5/29.
 */
app.controller('partnerListController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.searchCondition = {
      userId:4
    };
    //查询合伙人
    var getPartnerList  = function() {
        $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed?userId='+6,{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list = data.list;
        });
    };
    getPartnerList();

}
]);
//查询经纪人收到的邀请
app.controller ('searchInviteController',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
    $scope.searchCondition={
        brokerId:0
    };
    var getInvite = function(){
        $http.get(SETTING.ApiUrl + '/PartnerList/GetInviteForBroker?brokerId='+4, {'withCredentials': true}).success(function (data) {
         console.log(data);
         $scope.list=data.list;
        })
    };
    getInvite();
    var agreeAdd = function(status,id){
        $http.get(SETTING.ApiUrl + '/PartnerList/SetPartner?status='+status+"&id="+id, {'withCredentials': true}).success(function (data) {
            if(data.Status){
                getInvite();

            }
        })
    };
    $scope.agreeAddPartner = agreeAdd;
}
]);


