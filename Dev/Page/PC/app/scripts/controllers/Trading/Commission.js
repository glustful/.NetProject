/**
 * Created by Administrator on 2015/7/17.
 */
angular.module("app").controller("CommissionController",[
    '$http','$scope','$modal',function($http,$scope,$modal){
        $scope.Model={
            Id:'',
            RecCfbScale:'',
            RecAgentScale:'',
            TakeCfbScale:'',
            TakeAgentScale:'',
            TakePartnerScale:'',
            RecPartnerScale:''
        }
        $http.get(SETTING.ApiUrl+'/CommissionRatio/Index',{'withCredentials':true}).success(function(data){
            $scope.Model = data;
        })
        $scope.createOrUpdate=function(){
            $http.post(SETTING.ApiUrl+'/CommissionRatio/CreateOrUpdate',$scope.Model,{'withCredentials':true}).success(function(data){
                if(data.Status)
                {
                    var modalInstance = $modal.open({
                        templateUrl: 'myModalContent.html',
                        controller: 'ModalInstanceCtrl',
                        resolve: {
                            msg: function () {
                                return data.Msg;
                            }
                        }
                    });
                }
            })
        }
    }
])