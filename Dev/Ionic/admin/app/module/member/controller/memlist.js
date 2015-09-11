/*

 */
app.controller('memController', ['$scope', '$http','$stateParams','$modal',
    function($scope, $http,$stateParams,$modal) {
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };

        var getMember = function() {
            $http.get(SETTING.ZergWcApiUrl+"/Member/Get",{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list=data.List;
            });
        }
        getMember();
        //根据Id获取会员详情
        var getMemById = function(){
            $http.get(SETTING.ZergWcApiUrl+"/Member/Get?id="+$stateParams.id,{
                //params:$stateParams.id,
                'withCredentials':true
            }).success(function(data){
                $scope.memModels=data;
                console.log(data);
            });
        }
        getMemById();
        //获取会员地址信息

       var getMemAddress = function() {
           $http.get(SETTING.ZergWcApiUrl + "/MemberAddress/Get?id="+$stateParams.id, {
               //params: $stateParams.id,
               'withCredentials': true
           }).success(function (data) {
               $scope.memAddress = data.List;

           });
       }
        getMemAddress();

        $scope.deleteMem=function (Id) {
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve: {
                    msg:function(){return "你确定要删除吗？";}
                }
            });
            modalInstance.result.then(function() {
                $http.delete(SETTING.ZergWcApiUrl + '/Member/Delete?id=' + Id, {
                    'withCredentials': true
                }).success(function (data) {

                    if (data.Status) {
                        getMember();
                    }else{
                        $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }
                });
            });
        }
//-------------------------删除会员 end------------------------
    }]);