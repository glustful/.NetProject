/*

 */
app.controller('memController', ['$scope', '$http','$stateParams',
    function($scope, $http,$stateParams) {
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };

        //$scope.getMember = function() {
            $http.get(SETTING.ZergWcApiUrl+"/Member/Get",{
                params:$scope.searchCondition,
                'withCredentials':true
            }).success(function(data){
                $scope.list=data.List;
            });
        //}
        //根据Id获取会员详情
        //$scope.getMemById = function(){
            $http.get(SETTING.ZergWcApiUrl+"/Member/Get?id="+$stateParams.id,{
                //params:$stateParams.id,
                'withCredentials':true
            }).success(function(data){
                $scope.memModels=data;
                console.log(data);
            });
        //}
        //获取会员地址信息

       //$scope.getMemAddress = function() {
           $http.get(SETTING.ZergWcApiUrl + "/MemberAddress/Get?id="+$stateParams.id, {
               //params: $stateParams.id,
               'withCredentials': true
           }).success(function (data) {
               $scope.memAddress = data.List;

           });
       //}

        $scope.deleteMem=function (id) {

                $http.delete(SETTING.ZergWcApiUrl+'/Member/Delete',id,{
                    'withCredentials':true
                }).success(function (data) {
                    alert(data.Msg);
                    if (data.Status){
                        $scope.getMember();
                    }

                });
        }
//-------------------------删除会员 end------------------------
    }]);