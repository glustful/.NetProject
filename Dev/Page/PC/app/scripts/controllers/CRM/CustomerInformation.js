/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('CInfoListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"等待上访",
            clientName:"",
            page: 1,
            pageSize: 10,
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序,
        };
        $scope.UpOrDownImgClass="fa-caret-down";//升降序图标
        var getTagList = function(orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
                if($scope.searchCondition.isDes==true)//如果为降序，
                {
                    $scope.UpOrDownImgClass="fa-caret-up";//改变成升序图标
                    $scope.searchCondition.isDes=false;//则变成升序
                }
                else if($scope.searchCondition.isDes==false)
                {
                    $scope.UpOrDownImgClass="fa-caret-down";
                    $scope.searchCondition.isDes=true;
                }
            }
            $http.get(SETTING.ApiUrl+'/ClientInfo/GetClientInfoList',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                    $scope.Brokerlist = data.list1;
                    $scope.searchCondition.page=data.condition1.Page;
                    $scope.searchCondition.PageCount=data.condition1.PageCount;
                    $scope.searchCondition.totalCount=data.totalCont1;
                    console.log(data);
                if(data.list1==""){
                    $scope.errorTip="你查找的用户不存在，请重新查找！";
                }
                else{
                    $scope.errorTip="";
                }

            });
        };
        $scope.getList = function(orderByAll){
           // if( $scope.searchCondition.clientName==""){
                getTagList(orderByAll)
           // }
        }
        getTagList();
    }
]);

//详细信息
angular.module("app").controller('CIDetialController',[
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
            console.log($scope.ARDetialModel);
        });

    }
]);