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