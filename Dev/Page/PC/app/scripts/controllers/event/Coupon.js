/**
 * Created by Administrator on 2015/7/1.
 */
angular.module("app").controller('CouponIndexController', ['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.condition={
        page:1,
        pageSize:10
    }
    var getCouponList=function(){$http.get('http://localhost:16857//api/Coupon/Index',
        {params: $scope.condition,'withCredentials': true}).success(function(data){
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
            $http.get('http://localhost:16857//api/Coupon/Delete',{params:{
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
    $http.get('http://localhost:16857//api/CouponCategory/Index', {params: $scope.condition}, {'withCredentials': true}).success(
        function (data) {
            $scope.CouponCategoryList=data.List;
        }
    );
    $http.get('http://localhost:16857//api/Coupon/Detailed?id='+$stateParams.id,{'withCredentials': true}).success(function(data){
       $scope.Coupon=data;
    });
    $scope.update=function(){
        $http.post('http://localhost:16857//api/Coupon/Edit?',$scope.Coupon,{'withCredentials': true}).success(function(data){
            if(data.Status)
            {
                $state.go('page.event.Coupons.manage.index');
            }
        });
    }
}])
angular.module("app").controller('CouponCreateController', ['$http','$scope','$state',function($http,$scope,$state) {
    $scope.condition={
        page:1,
        pageSize:10
    }
    $http.get('http://localhost:16857//api/CouponCategory/Index', {params: $scope.condition}, {'withCredentials': true}).success(
        function (data) {
            $scope.CouponCategoryList=data.List;
        }
    );
    $scope.Coupon={
        Price:'',
        Number:'',
        Status:'',
        CouponCategoryId:'',
        Count:''
    }
    $scope.create=function(){
        $http.post('http://localhost:16857//api/Coupon/BlukCreate',$scope.Coupon,{'withCredentials': true}).success(function(data){
            if(data.Status)
            {
                $state.go('page.event.Coupons.manage.index');
            }
        });
    }
}])