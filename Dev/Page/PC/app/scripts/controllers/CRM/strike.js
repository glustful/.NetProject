/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('SCInfoListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"洽谈成功",
            clientName:"",
            page: 1,
            pageSize: 10
        };

        var getTagList = function() {
            $http.get(SETTING.ApiUrl+'/ClientInfo/GetClientInfoList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                console.log(data);
                $scope.Brokerlist = data.list1;
                $scope.searchCondition.page=data.condition1.Page;
                $scope.searchCondition.PageCount=data.condition1.PageCount;
                $scope.searchCondition.totalCount=data.totalCont1;
                if(data.list1==""){
                    $scope.errorTips="你查找的客户不存在，请重新查找！"
                }
                else{
                    $scope.errorTips="";
                }
                console.log($scope.errorTips);
            });
        };
        $scope.getList = getTagList;
        getTagList();
    }
]);

//详细信息
angular.module("app").controller('SCIDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $scope.ARDetialModel={
            clientModel:{
                Clientname:'',
                Phone:'',
                Housetype:'',
                Houses:'',
                Uptime:'',
                Note:''
            },
            brokerModel:{
                Brokername:'',
                Brokerlevel:'',
                Qq:'',
                Phone:'',
                RegTime:'',
                Projectname:''
            }
        }
        $http.get(SETTING.ApiUrl + '/ClientInfo/ClientInfo/' + $stateParams.id,{
            'withCredentials':true
        }).success(function (data) {
            $scope.ARDetialModel = data;
        });

    }
]);