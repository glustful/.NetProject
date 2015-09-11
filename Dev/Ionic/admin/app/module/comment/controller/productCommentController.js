/**
 * Created by yangyue on 2015/9/10.
 */
angular.module("app").controller('productCommentController',['$http','$scope','$modal',function($http,$scope,$modal){
    $scope.searchCondition = {
        Page: 1,
        PageCount:'10'

        //ProductId:''

    };
    //查询所有
    var getCommentList=function() {
        $http.get('http://localhost:50597/api/ProductComment/Get', {
            params: $scope.searchCondition,
            'withCredentials': true
        }).success(function (data) {
            $scope.list = data.Model;
            $scope.searchCondition.Page = data.Condition.Page;
            $scope.searchCondition.PageCount = data.Condition.PageCount;
            $scope.totalCount = data.TotalCount;
        })
    }
    getCommentList();
    $scope.getList=getCommentList;
    //删除
    //$scope.del =(function(Id){
    //        $http.delete('http://localhost:50597/api/ProductComment/Delete?Id='+Id,{
    //
    //                'withCredentials':true
    //            }
    //        ).success(function(data) {
    //                if (data.Status) {
    //                    getCommentList();
    //                }
    //            });
    //    })

    $scope.del = function (Id) {
        $scope.selectedId = Id;
        var modalInstance = $modal.open({
            templateUrl: 'myModalContent.html',
            controller:'ModalInstanceCtrl',
            resolve: {
                msg:function(){return "你确定要删除吗？";}
            }
        });
        modalInstance.result.then(function(){
            $http.delete('http://localhost:50597/api/ProductComment/Delete?Id='+Id,{

                    'withCredentials':true
                }
            ).success(function(data) {
                    if (data.Status) {
                        getCommentList();
                    }
                });

        })
    }



}
])
//时间格式过滤
app.filter('dateFilter',function(){
    return function(date){
        if(!date)
            return "";
        var jsondate = date.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        }
        else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var newDate = new Date(parseInt(jsondate, 10));
        var month = newDate.getMonth() + 1 < 10 ? "0" + (newDate.getMonth() + 1) : newDate.getMonth() + 1;
        var currentDate = newDate.getDate() < 10 ? "0" + newDate.getDate() : newDate.getDate();

        return newDate.getFullYear()
            + "-"
            + month
            + "-"
            + currentDate
            + "  "
            + newDate.getHours()
            + ":"
            + newDate.getMinutes()
            + ":"
            + newDate.getSeconds()
            ;
    }
});