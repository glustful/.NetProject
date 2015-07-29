/**
 * Created by Yunjoy on 2015/7/28.
 */
app.controller('EditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Event/GetEventDetail/' +$stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.activity =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/Event/UpEvent',$scope.activity,{'withCredentials':true
        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.activity.activityList");
            }else{
                alert(data.Msg);
            }
        });
    }

}]);




app.filter('dateFilter',function(){
    return function(date){
        return FormatDate(date);
    }
})
function FormatDate(JSONDateString) {
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
        //+ "-"
        //+ date.getHours()
        //+ ":"
        //+ date.getMinutes()
        //+ ":"
        //+ date.getSeconds()
        ;

}