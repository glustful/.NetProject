/**
 * Created by yangdingpeng on 2015/5/12.
 */

//推荐列表
angular.module("app").controller('WaitListController', [
    '$http','$scope',function($http,$scope) {
        $scope.searchCondition = {
            status:"审核中",
            Brokername:"",
            page: 1,
            pageSize: 10
};

var getTagList = function() {
    $http.get(SETTING.ApiUrl+'/AdminRecom/BrokerList',{params:$scope.searchCondition}).success(function(data){
        $scope.Brokerlist = data.list1;
        $scope.searchCondition.page=data.condition1.Page;
        $scope.searchCondition.PageCount=data.condition1.PageCount;
        $scope.searchCondition.totalCount=data.totalCont1;

        console.log("////////////////////////////////////////////////////////////////////\n"+
            "//							_ooOoo_								  //\n"+
            "//						   o8888888o							  //\n"+
            "//						   88\" . \"88							  //\n"+
            "//						   (| ^_^ |)							  //\n"+
            "//						   O\\  =  /O							  //\n"+
            "//						____/`---'\\____							  //\n"+
            "//					  .'  \\\\|     |//  `.						  //\n"+
            "//					 /  \\\\|||  :  |||//  \\						  //\n"+
            "//				    /  _||||| -:- |||||-  \\						  //\n"+
            "//				    |   | \\\\\\  -  /// |   |						  //\n"+
            "//					| \\_|  ''\\---/''  |   |						  //\n"+
            "//					\\  .-\\__  `-`  ___/-. /						  //\n"+
            "//					\\  .-\\__  `-`  ___/-. /						  //\n"+
            "//				.\"\" '<  `.___\\_<|>_/___.'  >'\"\".				  //\n"+
            "//			  | | :  `- \\`.;`\\ _ /`;.`/ - ` : | |				  //\n"+
            "//			  \\  \\ `-.   \\_ __\\ /__ _/   .-` /  /                 //\n"+
            "//		========`-.____`-.___\\_____/___.-`____.-'========		  //\n"+
            "//				             `=---='                              //\n"+
            "//		^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //\n"+
            "//         佛祖保佑       永无BUG		永不修改		  	  	          //\n"+
            "////////////////////////////////////////////////////////////////////\n");
    });
};
$scope.getList = getTagList;
getTagList();
}
]);

//详细信息
angular.module("app").controller('ARDetialController',[
    '$http','$scope','$stateParams',function($http,$scope,$stateParams) {
        //获取详细信息
        $http.get(SETTING.ApiUrl + '/AdminRecom/GetAuditDetail/' + $stateParams.id).success(function (data) {
            $scope.ARDetialModel = data;
        });
        //获取带客人员列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/WaiterList').success(function (data) {
            $scope.WaiterList = data;
        });
        //获取驻场秘书列表
        $http.get(SETTING.ApiUrl + '/AdminRecom/SecretaryList').success(function (data) {
            $scope.SecretaryList = data;
        });

        $scope.PassAudit = {
            Id:"",
            Status:""
        };

        //变更用户状态
        $scope.passAudit1=function(enum1){
            $scope.PassAudit.Id= $scope.ARDetialModel.Id;
            $scope.PassAudit.Status=enum1;

            $http.post(SETTING.ApiUrl + '/AdminRecom/PassAudit',$scope.PassAudit,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    console.log(data.Msg);
                }else{
                    console.log(data.Msg);
                }
            });
        };
    }
]);