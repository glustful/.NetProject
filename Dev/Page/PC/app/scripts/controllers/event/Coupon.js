/**
 * Created by Administrator on 2015/7/1.
 */
angular.module("app").controller('CouponIndexController', ['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.condition={
        number:'',
        page:1,
        pageSize:10,
        orderByAll:"OrderByAddtime",//排序
        isDes:true//升序or降序
        // className:'房地产'
    };
    var iniImg=function(){
        $scope.OrderById="footable-sort-indicator";

    }
    iniImg();
    $scope.OrderById="fa-caret-down";//升降序图标
    var getCouponList=function(orderByAll){
        if(orderByAll!=undefined){
            $scope.condition.orderByAll=orderByAll ;
            if($scope.condition.isDes==true)//如果为降序，
            {
                $scope.d="$scope."+orderByAll+"='fa-caret-up';";
                iniImg();//将所有的图标变成一个月
                eval($scope.d);//把$scope.d当做语句来执行，把当前点击图片变成向上
                $scope.condition.isDes=false;//则变成升序
            }
            else if($scope.condition.isDes==false)
            {
                $scope.d="$scope."+orderByAll+"='fa-caret-down';";
                iniImg();
                eval($scope.d);
                $scope.condition.isDes=true;
            }
        }
        $http.get(SETTING.eventApiUrl+'/Coupon/Index',{params: $scope.condition,'withCredentials': true}).success(function(data){
            $scope.List=data.List;
            $scope.condition.page=data.Condition.Page;
            $scope.condition.pageSize=data.Condition.PageCount;
            $scope.Count=data.TotalCount
        })
    };
    $scope.getList=getCouponList;
    getCouponList();
    $scope.delete=function(id){
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller:'ModalInstanceCtrl',
            resolve: {
                msg:function(){return "你确定要删除吗？";}
            }
        });
        modalInstance.result.then(function(){
            $http.get(SETTING.eventApiUrl+'/Coupon/Delete',{params:{
                    id:id
                },'withCredentials': true}
            ).success(function(data) {
                    if (data.Status) {
                        getCouponList();
                    }
                });
        });
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };
    }
}])
angular.module("app").controller('CouponEditController', ['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
    $scope.condition={
        page:1,
        pageSize:10
    }
    $http.get(SETTING.eventApiUrl+'/CouponCategory/Index', {params: $scope.condition}, {'withCredentials': true}).success(
        function (data) {
            $scope.CouponCategoryList=data.List;
        }
    );
    $http.get(SETTING.eventApiUrl+'/Coupon/Detailed?id='+$stateParams.id,{'withCredentials': true}).success(function(data){
       $scope.Coupon=data;
    });
    $scope.update=function(){
        $http.post(SETTING.eventApiUrl+'/Coupon/Edit?',$scope.Coupon,{'withCredentials': true}).success(function(data){
            if(data.Status)
            {
                $state.go('page.event.Coupons.manage.index');
            }
        });
    }
}])
angular.module("app").controller('CouponCreateController', ['$http','$scope','$state',function($http,$scope,$state) {
    $scope.condition={
        name:'',
        page:1,
        pageSize:10
    }
    $http.get(SETTING.eventApiUrl+'/CouponCategory/Index', {params: $scope.condition}, {'withCredentials': true}).success(
        function (data) {
            $scope.CouponCategoryList=data.List;
        }
    );
    $scope.Coupon={
        Price:'',
        Number:'',
        Status:'',
        CouponCategoryId:'',
        Count:'',
        Content:''
    }
    $scope.create=function(){
        $http.post(SETTING.eventApiUrl+'/Coupon/BlukCreate',$scope.Coupon,{'withCredentials': true}).success(function(data){
            if(data.Status)
            {
                $state.go('page.event.Coupons.manage.index');
            }
        });
    }
}])