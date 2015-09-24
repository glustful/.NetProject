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


app.controller('newaddress',['$http','$scope','$stateParams','$state','AuthService','$ionicLoading','$timeout',function($http,$scope,$stateParams,$state,AuthService,$ionicLoading,$timeout) {
    $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
    if( $scope.currentuser==undefined ||  $scope.currentuser=="")
    {
        $state.go("page.login");//调到登录页面
    }
  if( $stateParams.name==undefined ||  $stateParams.name=="" ||  $stateParams.id==undefined ||  $stateParams.id=="" )
  {
      $state.go("page.addressAdm");
  }

    $scope.AreaName=$stateParams.name;
        $scope.Addre={
        AreaId: $stateParams.id,
        Address:'',
        Zip :'',
        Linkman :'',
        Tel:''
    };


    $scope.saves = function () {
        if( $scope.Addre.Address=="" ||  $scope.Addre.Address==undefined ||  $scope.Addre.Zip=="" ||  $scope.Addre.Zip==undefined ||  $scope.Addre.Linkman=="" ||  $scope.Addre.Linkman==undefined ||  $scope.Addre.Tel=="" ||  $scope.Addre.Tel==undefined  )
        {
            return;
        }
        if( $scope.Addre.Zip.length!=6)
        {
            return;
        }
        $http.post(SETTING.ApiUrl+ '/MemberAddress/Post', $scope.Addre, {'withCredentials': true}).success(function (data) {
                if (data.Status) {

                    $state.go("page.addressAdm");
                }
            });
    }
}])

