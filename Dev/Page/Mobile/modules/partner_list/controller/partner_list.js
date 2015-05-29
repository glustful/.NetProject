/**
 * Created by 杨波 on 2015/5/29.
 */
app.controller('partnerListController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.searchCondition = {
        name: '',
        Id:0,
        page: 1

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

}
]);

