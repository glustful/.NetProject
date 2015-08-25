/**
 * Created by Yunjoy on 2015/7/28.
 */
app.controller('EditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/Event/GetEventDetail/' +$stateParams.id,{
        'withCredentials':true
    }).success(function(data){
         console.log(data);
        $scope.activity =data;document.getElementById("domstarttime").value=FormatDate(data.StartTime);
        document.getElementById("domendtime").value=FormatDate(data.EndTime);

    });

    $scope.Save = function(){
        $scope.activity.StartTime=document.getElementById("domstarttime").value;
        $scope.activity.EndTime=document.getElementById("domendtime").value;
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