/**
 * Created by ¸ß·åÃù on 2015/7/28.
 */
app.controller('addController',['$scope','$http','$state',function($scope,$http,$state){
    $scope.list={
        EventContent:'',
        Starttime:'',
        Endtime:'',
        State:'false'
    }
    $scope.add=function(){

        $http.post(SETTING.ApiUrl+'/Event/AddEvent',$scope.list,{ 'withCredentials':true}).success(function(data){
            if(data.Status){
                console.log(data);
                $state.go("page.CRM.activity.activityList");
            }
            else{
                $scope.tip=data.Msg;
                alert(data.Msg)
            }
        })
    }
}])



