/**
 * Created by 黄秀宇 on 2015/6/1.
 */
app.controller('cusListController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        id:5,
        page: 1,
        pageSize: 10
    };
    $scope.warm="";
    //查询任务
    var getcustomerList  = function() {
        $http.get(SETTING.ApiUrl+'/ClientInfo/ClientInfo/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            if(data.clientModel!=null){
                $scope.warm="";
                $scope.list = data.clientModel;
              }
            else{
                $scope.warm="目前没有客户，革命还需努力";
            }
        });
    };
    getcustomerList();
  }]);

