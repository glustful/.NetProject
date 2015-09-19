/**
 * Created by huangxiuyu on 2015/9/16.
 */
//start--------------------------地址管理 huangxiuyu 2015.09.15--------------------------
app.controller('addressAdm',['$http','$scope',function($http,$scope, $ionicSlideBoxDelegate) {
    $scope.searchCondition={
        Adduser:1
    }
    $scope.getAddress=function(){
        $http.get(SETTING.ApiUrl+'/MemberAddress/Get/',{params:$scope.searchCondition,'withCredentials':true}).
            success(function(data){

                $scope.list=data.List;
                console.log(data);
            })};
    $scope.getAddress();

    //删除地址
    $scope.delCondition={
        id:0
    }
    $scope.delAddress=function(id){
        $scope.delCondition.id=id;
        $http.delete(SETTING.ApiUrl+'/MemberAddress/Delete/',{params:$scope.delCondition,withCredentials:true}).
            success(function(data){
                alert(data.Msg);
                $scope.getAddress();
//                if($scope.listRe.length>1)
//                {
//               for(var i=0;i<$scope.listRe.length;i++)
//               {
//                   if($scope.listRe[i].Id===id)
//                   {
//                       delete $scope.listRe[i];
//                       $scope.list=$scope.listRe;
//                       break;
//
//                   }
//               }
//                }
            });
    }

}]);
//end--------------------------地址管理 huangxiuyu 2015.09.15--------------------------

var fId;
app.controller('selProvice',['$http','$scope',function($http,$scope){
    //查找省份地址
    $scope.searchCondition={
        father:true
    };

    $scope.selProvice=function(){
        $http.get(SETTING.ApiUrl +"/CommunityArea/Get/",{params:$scope.searchCondition,withCredentials:true}).
            success(function(data){
                $scope.listPro=data.List;
            });
    };
    $scope.selProvice();

}]);

app.controller('selectCity',['$http','$scope','$stateParams',function($http,$scope,$stateParams){

    $scope.searchCondition={
        father:false,
        fatherid:0
    };
    $scope.selCity=function(){
        console.log($stateParams);
        $scope.name= $.trim($stateParams.name);
        $scope.searchCondition.fatherid=$stateParams.id;
        $http.get(SETTING.ApiUrl +"/CommunityArea/Get/",{params:$scope.searchCondition,withCredentials:true}).
            success(function(data){

                $scope.listCity=data.List;
                console.log($scope.listCity);
            });
    }
    $scope.selCity();
}]);
app.controller('selectCounty',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
   $scope.name= $.trim($stateParams.name);
    $scope.searchCondition={
        father:false,
        fatherid:0
    };
    $scope.searchCondition.fatherid=$stateParams.id;
   $scope.getCounty= $http.get(SETTING.ApiUrl+ "/CommunityArea/Get/",{params:$scope.searchCondition,withCredentials:true}).
       success(function(data){
        $scope.listCounty=data.List;
    });
}]);

app.controller('newAddress',['$http','$scope','$stateParams',function($http,$scope,$stateParams){

    $scope.searchCondition={
        father:false,
        fatherid:0
    };
    $scope.selCity=function(){
        console.log($stateParams);
        $scope.name= $.trim($stateParams.name);
        $scope.searchCondition.fatherid=$stateParams.id;
        $http.get(SETTING.ApiUrl +"/CommunityArea/Get/",{params:$scope.searchCondition,withCredentials:true}).
            success(function(data){

                $scope.listCity=data.List;
                console.log($scope.listCity);
            });
    }
    $scope.selCity();
}]);
