/**
 * Created by gaofengming on 2015/7/22.
 */
app.controller('activityController',['$scope','$http','$modal',function($scope,$http,$modal){
    $scope.condition={
        //Id:'',
        //EventContent:'',
        //Starttime:'',
        //Endtime:'',
        //State:'',
        orderByAll:"OrderById",//排序
        isDes:true//升序or降序
    }
    $scope.UpOrDownImgClass="fa-caret-down";
    $scope.getlist=function(orderByAll){
        if(orderByAll!=undefined) {
            $scope.condition.orderByAll=orderByAll ;
            if ($scope.condition.isDes == true)//如果为降序，
            {
                $scope.UpOrDownImgClass = "fa-caret-up";//改变成升序图标
                $scope.condition.isDes = false;//则变成升序
            }
            else if ($scope.condition.isDes == false) {
                $scope.UpOrDownImgClass = "fa-caret-down";
                $scope.condition.isDes = true;
            }
        }
        $http.get(SETTING.ApiUrl+'/Event/GetEventList',{params:$scope.condition,'withCredentials':true}).success(function(data){
            console.log(data);

            //document.getElementById("Starttime").value=FormatDate( data[0].Starttime);
            //document.getElementById("Endtime").value=FormatDate(data.Endtime);

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
