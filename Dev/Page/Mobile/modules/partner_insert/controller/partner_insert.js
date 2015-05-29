/**
 * Created by 杨波 on 2015/5/29.
 */

app.controller('partnerListController',['$http','$scope','$stateParams',function($http,$scope) {
    $scope.addCondition = {
        Id:0,
        Broker: '',
        PartnerId: 0,
        userId:0,
        BrokerId:0,
        Phone:''
    };

    //增加合伙人
    $scope.save=function() {
        $http.post(SETTING.ApiUrl + '/PartnerList/AddPartnerList', $scope.addCondition, {'withCredentials': true}).success(function (data) {
            console.log(data);
        })
    };
}
]);
