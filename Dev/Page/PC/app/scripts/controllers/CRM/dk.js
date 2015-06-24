/**
 * Created by lhl on 2015/5/16 代客管理
 */
angular.module("app").controller('dkIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"带客人员",
            page: 1,
            pageSize: 10,
            state:2
        };
        var page= 0,howmany=0;
        $scope.getList  = function() {

            if($scope.searchCondition.phone==undefined)
            {$scope.searchCondition.phone="";}
            $http.get(SETTING.ApiUrl+'/BrokerInfo/SearchBrokers',{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                // alert(data.list!=null);
                if(data.List.length>0) {
                    console.log(data);

                    page= $scope.searchCondition.page = data.Condition.Page;
                    howmany=data.List.length;//保存当页数据数量
                    $scope.searchCondition.pageSize = data.Condition.PageCount;
                    $scope.totalCount = data.totalCount;
                    $scope.visibleif=true;
                    $scope.tips="";
                    $scope.list=[];//清空
                    //循环绑定每一条数据，目的是让btnVisibleCan，btnVisibleDel可见与否
                    for(var i=0;i<data.List.length;i++)
                    {
                        $scope.list.push( data.List[i]);
                        if($scope.list[i].State==-1)
                        {
                            $scope.list[i].btnVisibleCan=false;
                            $scope.list[i].btnVisibleDel=true;
                        }
                        else if($scope.list[i].State==0)
                        {
                            $scope.list[i].btnVisibleCan=false;
                            $scope.list[i].btnVisibleDel=false;
                        }
                        else if($scope.list[i].State==1)
                        {
                            $scope.list[i].btnVisibleCan=true;
                            $scope.list[i].btnVisibleDel=true;
                        }

                    }
                }
                else{
                    $scope.errorTip="不存在数据";
                    $scope.visibleif=false;
                    $scope.tips="没有数据"
                }
                $scope.list = data.List;
                if(data.List == ""){
                    $scope.errorTip="不存在数据";
                }
                $scope.searchCondition.page = data.Condition.Page;
                $scope.searchCondition.pageSize = data.Condition.PageCount;
                $scope.totalCount = data.totalCount;

            });
        };
        //初始化page
        $scope.initPage=function(){
            $scope.searchCondition.page=1;
        }
        $scope.getList();

        //删除经纪人
        $scope.deleteBroker=function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要删除吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.post(SETTING.ApiUrl+'/BrokerInfo/DeleteBroker',id,{
                    'withCredentials':true
                }).success(function (data) {
                    // $scope.divtips=true;
                    alert(data.Msg);

                    if (data.Status) {

                        //  alert(data.Msg);
                        if(howmany==1)
                        {
                            if(page>1){
                                $scope.searchCondition.page--;}
                            else{
                                $scope.searchCondition.page=1;
                            }
                        }
                        $scope.getList();


                    }
                    else{

                        //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
            });
        }
//            function(id){
//        $http.post(SETTING.ApiUrl+'/BrokerInfo/DeleteBroker',id,{
//            'withCredentials':true
//        }).success(function(data) {
//           if(data.Status)
//           {
//               alert(data.Msg);
//               if(howmany==1)
//               {
//                   if(page>1){
//                   $scope.searchCondition.page--;}
//                   else{
//                       $scope.searchCondition.page=1;
//                   }
//               }
//               $scope.getList();
//           }
//
//        })}
        //注销经纪人
        $scope.cancelBroker=function (id) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要注销吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.post(SETTING.ApiUrl+'/BrokerInfo/CancelBroker',id,{
                    'withCredentials':true
                }).success(function (data) {
                    alert(data.Msg);
                    if (data.Status) {

                        if(howmany==1)
                        {
                            if(page>1){
                                $scope.searchCondition.page--;}
                            else{
                                $scope.searchCondition.page=1;
                            }
                        }
                        $scope.getList();


                    }
                    else{

                        //  $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
            });
        }
//        $scope.cancelBroker=function(id){
//            $http.post(SETTING.ApiUrl+'/BrokerInfo/CancelBroker',id,{
//                'withCredentials':true
//            }).success(function(data) {
//                if(data.Status)
//                {
//                    alert(data.Msg);
//                    if(howmany==1)
//                    {
//                        if(page>1){
//                            $scope.searchCondition.page--;}
//                        else{
//                            $scope.searchCondition.page=1;
//                        }
//                    }
//                    $scope.getList();
//                }
//
//            })}

    }
]);


angular.module("app").controller('dkDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){
//个人信息
    $http.get(SETTING.ApiUrl + '/BrokerInfo/GetBrokerByAgent?id=' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        if(data.List.State==1)
        {data.List.State="正常"}
        else if(data.List.State==0)
        {data.List.State="删除"}
        else if(data.List.State==-1)
        {data.List.State="注销"}
        $scope.BusmanModel =data.List;
    });
}]);

angular.module("app").controller('UserCreateController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){

    $scope.UserModel={

        Password:"",
        Brokername:"",
        Phone:"",
        UserType:"带客人员",
        UserName:""
    };

    $scope.Save = function(){
        console.log($scope.UserModel.UserType);
        $http.post(SETTING.ApiUrl + '/AdminRecom/AddBroker',$scope.UserModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
                alert(data.Msg);
                $scope.UserModel.Password="",
                    $scope.UserModel.Brokername="",
                    $scope.UserModel.Phone="",
                    $scope.UserModel.UserName=""


            }else{
                alert(data.Msg);
            }
        });
    }
}]);