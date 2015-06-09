/**
 * Created by 黄秀宇 on 2015/6/1.
 */
app.controller('cusListController',['$http','$scope','AuthService',function($http,$scope,AuthService) {
    $scope.searchCondition = {
        id:5,
        page: 1,
        pageSize: 10
    };
    $scope.warm="";
    $scope.parentVi=true;
    //查询客户
    var getcustomerList  = function() {
        $http.get(SETTING.ApiUrl+'/ClientInfo/GetClientInfoListByUserId/',{'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.list!=null){
                $scope.warm="";
                $scope.list = data.list;
                $scope.parentVi=true;
              }
            else{
                $scope.parentVi=false;
                $scope.warm="目前没有客户，革命还需努力";
            }
        });
    };
    getcustomerList();
//隐藏显示元素
    $scope.visible = false;
    $scope.toggle = function (id) {
       $("#"+id).slideToggle("slow");
    }
  }]);

