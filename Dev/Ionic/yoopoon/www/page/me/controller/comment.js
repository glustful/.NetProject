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
        Content:"",
        ProductDetailsId:0
    }
    $scope.UpOrderDatail={
        Id:0
    }
    $scope.proName="";
    $scope.Img="";
    $scope.Sta="";
$scope.openshow=function(proId,proname,proimg,orId,comSta){
    document.getElementById("userComment").style.display = "inline";
    $scope.AddComment.ProductId=proId;
    $scope.proName=proname;
    $scope.Img=proimg;
    $scope.UpOrderDatail.Id =orId;
    $scope.Sta=comSta;
    $scope.AddComment.ProductDetailsId=orId;
    if(comSta=="已评价")
    {
        alert("你已评价过，不能再次评价");
    }
    //alert($scope.Id);
   // $("userComment").toggle();
};
  $scope.cance=  function (){
        document.getElementById("userComment").style.display = "none";
//
    }
    $scope.putComment=function(){
        if($scope.Sta==="已评价")
        {
            alert("你已评价过，不能再次评价");
            return;
        }
        if($scope.AddComment.Content.length===0)
        {
            alert("请输入评价内容");
            return;
        }
       $http.post(SETTING.ApiUrl+"/ProductComment/Post",$scope.AddComment,{'withCredentials':true}).
           success(function(data){
            if(data.Status)
            {
                $http.put(SETTING.ApiUrl+"/OrderDetail/Put",$scope.UpOrderDatail,{'withCredentials':true}).
                    success(function(data){
                        //alert(data.Status+"11");
                        $scope.getOrderById();
                        $http.get(SETTING.ApiUrl+"/CommunityOrder/UpOrderStatus?id="+$stateParams.Id,{'withCredentials':true}).success(function(data){

                            document.getElementById("userComment").style.display = "none";
                        })
                    });
            }
           });
    }
    }]);