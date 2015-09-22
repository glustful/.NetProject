/**
 * Created by huangxiuyu
 * on 2015/9/19.
 */
app.controller('comment',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
console.log($stateParams);
    $scope.getOrderById=function(){
            $http.get(SETTING.ApiUrl +'/CommunityOrder/Get?id='+$stateParams.Id,{'withCredentials':true}).
                success(function(data){
                    console.log(data);
                    $scope.list=data.List;
                })
    };
    $scope.getOrderById();
    //评价
    $scope.AddComment={
        ProductId:0,
        Content:""
    }
    $scope.proName="";
    $scope.Img="";
$scope.openshow=function(proId,proname,proimg){
    $scope.AddComment.ProductId=proId;
    $scope.proName=proname;
    $scope.Img=proimg;
//    alert($scope.Img);
   // $("userComment").toggle();
}

    $scope.putComment=function(){
       $http.post(SETTING.ApiUrl+"/ProductComment/Post",$scope.AddComment,{'withCredentials':true}).
           success(function(data){
            console.log(data.Msg);
           });
    }
    }])