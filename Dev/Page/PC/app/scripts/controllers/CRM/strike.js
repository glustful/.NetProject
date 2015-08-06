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
            pageSize: 10,
            orderByAll:"OrderById",//排序
            isDes:true//升序or降序,默认为降序
        };
//初始化所有图标
        var iniImg=function(){
            $scope.OrderById="footable-sort-indicator";
            $scope.OrderByClientname="footable-sort-indicator";
            $scope.OrderByPhone="footable-sort-indicator";
            $scope.OrderByBrokername="footable-sort-indicator";
            $scope.OrderByUptime="footable-sort-indicator";
        }
        iniImg();
        $scope.OrderById="fa-caret-down";//升降序图标
        var getTagList = function (orderByAll) {
            if(orderByAll!=undefined){
                $scope.searchCondition.orderByAll=orderByAll ;
                if($scope.searchCondition.isDes==true)//如果为降序，
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                    iniImg();//将所有的图标变成一个月
                    eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                    $scope.searchCondition.isDes=false;//则变成升序
                }
                else if($scope.searchCondition.isDes==false)
                {
                    $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                    iniImg();
                    eval($scope.d);
                    $scope.searchCondition.isDes=true;
                }
            }
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