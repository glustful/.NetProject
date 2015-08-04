/**
 * Created by �߷��� on 2015/7/28.
 */
app.controller('addController',['$scope','$http','$state',function($scope,$http,$state){
    $scope.list={
        EventContent:'',
        Starttime:'',
        Endtime:'',
        State:'false'
    }
    $scope.add=function(){

        $http.post(SETTING.ApiUrl+'/Event/AddEvent',{
            EventContent:$scope.list.EventContent,
            Starttime:$scope.list.Starttime.toLocaleDateString() ,
            Endtime:$scope.list.Endtime.toLocaleDateString() ,
            State:'false'
        },{ 'withCredentials':true}).success(function(data){
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



