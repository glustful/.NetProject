/**
 * Created by gaofengming on 2015/7/22.
 */
app.controller('activityController',['$scope','$http','$modal',function($scope,$http,$modal){
    $scope.list={
        Id:'',
        EventContent:'',
        Starttime:'',
        Endtime:'',
        State:''
    }
    $scope.getlist=function(){
        $http.get(SETTING.ApiUrl+'/Event/GetEventList',{params:$scope.list,'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list = data;
        })
    }
    $scope.getlist();
    $scope.open = function(id){
       $scope.selectId=id;
        var modalInstance=$modal.open({
            templateUrl:'myModalContent.html',
            controller:'ModalInstanceCtrl',
            resolve:{
                msg:function(){
                    return "你确定要删除吗？";
                }
            }
        });
        modalInstance.result.then(function () {
            $http.post(SETTING.ApiUrl+'/Event/DelEventById/'+$scope.selectId,{'withCredentials':true}).success(function(data){
                if(data.Status){
                    console.log("121212");
                    location.reload([true]);
                }
                else{
                    $scope.tip=data.Msg;

                }})
        })
    }

}])




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
    var currentDate = date.getDate() < 10 ? "0" +( date.getDate()) : date.getDate();

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