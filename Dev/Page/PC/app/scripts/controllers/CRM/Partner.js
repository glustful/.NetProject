/**
 * Created by lhl on 2015/5/12 合伙人
 */
//经纪人列表
angular.module("app").controller('PartnerIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            status:1,
            pageSize: 6
        };
        $scope.getList  = function() {
            $http.get(SETTING.ApiUrl+'/PartnerList/SearchPartnerList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list = data.List;
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;
            });
        };
        $scope.getList();
      //  getPartnerList();
     //   $scope.getList = getPartnerList;
    }
]);

angular.module("app").controller('PartnerPerController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
//查合伙人上家信息
    $http.get(SETTING.ApiUrl+'/PartnerList/SearchPartnerList1?PartnersId='+$stateParams.PartnersId,{
        'withCredentials':true
    }).success(function(data){
        console.log(data);
        $scope.list = data.List;
    });
}]);


angular.module("app").controller('PartnerDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
//个人信息
    $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed?userId=' + $stateParams.userId,{
        'withCredentials':true
    }).success(function(data){
        $scope.list = data.partnerList;
        console.log(data);
        if(data.partnerList.length==0){
            $scope.tips=" 无合伙人";
        }
    });

}]);

function ConvertJSONDateToJSDateObject(JSONDateString) {
    jsondate = JSONDateString.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
}
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    return date.getFullYear()
        + "-"
        + month
        + "-"
        + currentDate
        + "-"
        + date.getHours()
        + ":"
        + date.getMinutes();

}

