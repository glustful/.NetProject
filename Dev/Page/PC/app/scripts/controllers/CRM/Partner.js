/**
 * Created by lhl on 2015/5/12 合伙人
 */

angular.module("app").controller('PartnerIndexController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            name: '',
            page: 1,
            status:1,
            pageSize: 10
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

angular.module("app").controller('PartnerDetailedController', [
            '$http','$scope','$stateParams',function($http,$scope,$stateParams) {


                $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed?userId=' + $stateParams.userId,{
                    'withCredentials':true
                }).success(function(data){
            $scope.list = data;
        });

    }
]);



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

