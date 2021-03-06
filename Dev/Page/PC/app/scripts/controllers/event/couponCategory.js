/**
 * Created by Administrator on 2015/7/1.
 */
angular.module("app").controller('CouponCategoryController', ['$http','$scope','$modal',function($http,$scope,$modal) {
    $scope.condition={
        name:'',
        page:1,
        pageSize:10
    }
    var getCouponCategoryList=function(){
        $http.get(SETTING.eventApiUrl+'/CouponCategory/Index', {params: $scope.condition}, {'withCredentials': true}).success(
            function (data) {
                $scope.List=data.List;
                $scope.condition.page=data.Condition.Page;
                $scope.condition.pageSize=data.Condition.PageSize;
                $scope.Count=data.TotalCount
            }
        )
    }
    $scope.getList=getCouponCategoryList;
    getCouponCategoryList();
    $scope.delete=function(id){
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller:'ModalInstanceCtrl',
            resolve: {
                msg:function(){return "你确定要删除吗？";}
            }
        });
        modalInstance.result.then(function(){
            $http.get(SETTING.eventApiUrl+'/CouponCategory/Delete',{params:{
                    id:id
                },'withCredentials': true}
            ).success(function(data) {
                    if (data.Status) {
                        getCouponCategoryList();
                    }
                });
        });
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };
    }
}]);
angular.module("app").controller('CouponCategoryEditController', ['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
    $scope.BrandCondition={
        page:1,
        pageSize:10
    }

    $http.get(SETTING.eventApiUrl+'/CouponCategory/Detailed?id='+$stateParams.id,{'withCredentials': true}).success(
        function(data){
            $scope.CouponCategory=data;
            console.log(data);
        }
    )
    $http.get(SETTING.ApiUrl+'/Brand/GetAllBrand/',{params:$scope.BrandCondition,'withCredentials': true}).success(
        function(data){
            $scope.BrandList=data.List;
        }
    )
    $http.get(SETTING.ApiUrl+'/Classify/GetClassList/',{'withCredentials': true}).success(
        function(data){
            $scope.ClassList=data;
        }
    )
    $scope.update=function(){
        $http.post(SETTING.eventApiUrl+'/CouponCategory/Edit',$scope.CouponCategory,{'withCredentials': true}).success(
            function(data){
                if(data.Status)
                {
                     $state.go('page.event.Coupons.type.index');
                }
            }
        )
    }
}])
angular.module("app").controller('CouponCategoryCreateController', ['$http','$scope','$state','$stateParams',function($http,$scope,$state,$stateParams) {
    $scope.BrandCondition={
        page:1,
        pageSize:10
    }
    $scope.CouponCategoryModel={
        Name:'',
        Price:'',
        BrandId:'',
        ClassId:'',
        Count:'',
        ReMark:'',
        Intro:'',
        Content:''
    }
    $http.get(SETTING.ApiUrl+'/Brand/GetAllBrand/',{params:$scope.BrandCondition,'withCredentials': true}).success(
        function(data){
            $scope.BrandList=data.List;
        }
    )
    $http.get(SETTING.ApiUrl+'/Classify/GetClassList/',{'withCredentials': true}).success(
        function(data){
            $scope.ClassList=data;
        }
    )
    $scope.create=function(){
        $http.post(SETTING.eventApiUrl+'/CouponCategory/Create',$scope.CouponCategoryModel,{'withCredentials': true}).success(
            function(data){
                if(data.Status)
                {
                    $state.go('page.event.Coupons.type.index');
                }
            }
        )
    }
}])