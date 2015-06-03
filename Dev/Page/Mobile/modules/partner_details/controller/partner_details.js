/** create 杨波 2015.5.26 **/
//查询个人信息
app.controller('partnerDetailedController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.getCondition = {
        userId:0
    };
    var getPersonalInformation=function(){
        $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed?userId='+$stateParams.userId,{'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list=data.list;
        })
    };
    getPersonalInformation();
}
]);