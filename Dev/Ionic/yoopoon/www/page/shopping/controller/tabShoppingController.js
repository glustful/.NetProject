/**
 * Created by Administrator on 2015/9/7.
 */
app.controller('TabShoppingCtrl',['$http','$scope',function($http,$scope){
    //ҳ����ת
    $scope.go=function(state){
        window.location.href=state;
    }

//���¹���ˢ��
    $scope.items = [];
    var base = 0;
    $scope.load_more = function(){
        $timeout(function(){
            for(var i=0;i<10;i++,base++)
                $scope.items.push(["item ",base].join(""));
            $scope.$broadcast("scroll.infiniteScrollComplete");
        },500);
    };


}]);





