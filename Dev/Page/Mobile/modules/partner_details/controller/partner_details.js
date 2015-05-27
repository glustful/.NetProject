/** create 杨波 2015.5.26 **/
app.controller('partnerListController',['$http','$scope',function($http,$scope) {
    $scope.searchCondition = {
        name: '',
        Id:0,
        page: 1,
        pageSize: 10,
        userId:0,
        partnerList:''
    };
    //查询任务
    var getPartnerList  = function() {
        $http.get(SETTING.ApiUrl+'/PartnerList/PartnerListDetailed/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){
            console.log(data);
            $scope.list = data.list;
//            if(data.totalCount>0){
//
//                $scope.list = data.list;
//
//            }
//            else{
//
//            }
        });
    };
    $scope.getList = getPartnerList;
    getPartnerList();
    var addPartnerList=function(){
        $http.post(SETTING.ApiUrl+'/PartnerList/AddPartnerList/',{params:$scope.searchCondition,'withCredentials':true}).success(function(data){

        })
    }

}
]);