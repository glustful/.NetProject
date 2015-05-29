/** create 杨波 2015.5.26 **/
app.controller('partnerListController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.searchCondition = {
        name: '',
        Id:0,
        page: 1,
        pageSize: 10
    };
    $scope.addCondition = {
        Id:0,
        Broker: '',
        PartnerId: 0,
        userId:0,
        BrokerId:0,
        Phone:''
    };
    $scope.getCondition = {
       userId:0
    };
    //查询合伙人
    var getPartnerList  = function() {
        $http.get(SETTING.ApiUrl+'/PartnerList/SearchPartnerList/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list = data.List;
        });
    };
    $scope.getList = getPartnerList;
    getPartnerList();
    //增加合伙人
    var addPartner=function() {
        $http.post(SETTING.ApiUrl + '/PartnerList/AddPartnerList', $scope.addCondition, {'withCredentials': true}).success(function (data) {
            console.log(data);
        })
    };
    $scope.addPartner1=addPartner;
}
]);

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