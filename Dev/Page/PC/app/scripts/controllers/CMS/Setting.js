/**
 * Created by Yunjoy on 2015/5/9.
 */
angular.module("app").controller('SettingController', [
    '$http','$scope','$state','$filter',function($http,$scope,$state,$filter) {
        $scope.searchCondition = {
            key: '',
            page: 1,
            pageSize: 99,
            totalPage:1
        };

        var getAdvertisementList = function() {
            $http.get(SETTING.ApiUrl+'/Setting/Index',{params:$scope.searchCondition}).success(function(data){
                //$scope.list = data;
                $scope.SiteName = data[0];
                $scope.SiteUrl = data[1];
            });
        };
        $scope.getList = getAdvertisementList;
        getAdvertisementList();

        $scope.Save = function(){
            $http.post(SETTING.ApiUrl +'/Setting/Edit',[$scope.SiteName,$scope.SiteUrl],{
                'withCredentials':true
            }).success(function(data){
                    if(data.Status){
                        $scope.Message=data.Msg;
                    }
                }
            )
//            $http.post(SETTING.ApiUrl +'/Setting/Edit',$scope.SiteUrl,{
//                'withCredentials':true
//            })
            $state.go("page.CMS.set.index");
        }
    }
]);
