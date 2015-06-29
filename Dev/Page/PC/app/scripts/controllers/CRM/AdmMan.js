/**
 * Created by lhl on 2015/5/16 经纪人管理
 */
angular.module("app").controller('adminIndexController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.searchCondition = {
            name:'',
            phone:'',
            userType:"管理员",
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
                        if($scope.list[i].State==-1)//注销状态，注销按钮无效，删除有效
                        {
                            $scope.list[i].btnVisibleCan=false;//注销或回复按钮是否可用
                            $scope.list[i].btnVisibleDel=false;//删除按钮是否可用
                            $scope.list[i].btnname="恢复";//按钮名称，恢复或注销
                            $scope.list[i].backcolor={backgroundColor:'cadetblue'};//恢复按钮颜色
                            $scope.list[i].color={backgroundColor:'#FFEB3B'};//删除按钮颜色
                        }
                        else if($scope.list[i].State==0)//删除状态
                        {
                            $scope.list[i].btnVisibleCan=true;
                            $scope.list[i].btnVisibleDel=true;
                            $scope.list[i].btnname="恢复";
                            $scope.list[i].backcolor={backgroundColor:'lightgrey'};
                            $scope.list[i].color={backgroundColor:'lightgrey'};
                        }
                        else if($scope.list[i].State==1)//正常状态
                        {
                            $scope.list[i].btnVisibleCan=false;
                            $scope.list[i].btnVisibleDel=false;
                            $scope.list[i].btnname="注销";
                            $scope.list[i].color={color:'white'};
                            $scope.list[i].backcolor={backgroundColor:'#3F51B5'};
                            $scope.list[i].color={backgroundColor:'#FFEB3B'};


                        }

                    }
                }
                else{
                    $scope.visibleif=false;
                    $scope.tips="没有数据"
                }

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
        $scope.cancelBroker=function (id,btnname) {
            $scope.selectedId = id;
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    msg: function () {
                        return "你确定要"+btnname+"吗？";
                    }
                }
            });
            modalInstance.result.then(function () {
                $http.post(SETTING.ApiUrl+'/BrokerInfo/CancelBroker?id='+id+"&btnname=" +btnname,{
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

angular.module("app").controller('configureDetailedController',['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams){

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
        $scope.BrokerModel =data.List;
    });

}]);

angular.module("app").controller('UserCreateController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){

    $scope.UserModel={

        Password:"",
        Brokername:"",
        Phone:"",
        UserType:"管理员",
        UserName:""
    };

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/AdminRecom/AddBroker',$scope.UserModel,{
            'withCredentials':true
        }).success(function(data){
            if(data.Status){
               alert(data.Msg);
                $scope.UserModel.Password ="";
                $scope.UserModel.UserName ="";
                $scope.UserModel.Phone ="";
            }else{
                alert(data.Msg);
            }
        });
    }
}]);