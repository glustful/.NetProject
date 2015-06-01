/**
 * Created by 赵旭初 on 2015/5/11.
 * 功能：短信相关，包括配置，列表等
 */

// 短信列表
angular.module("app").controller('MessageSeachController', ['$http', '$scope', function ($http, $scope) {

    //========================================时间控件==============================================================
    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    // 开始时间
    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        class: 'datepicker'
    };

    $scope.initDate = new Date();
    $scope.formats = ['yyyy/MM/dd', 'shortDate'];
    $scope.format = $scope.formats[0];

    //终止时间
    $scope.endtimeopen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.endopened = true;
    };

    $scope.enddateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        class: 'datepicker'
    };

    $scope.initendDate = new Date();
    $scope.endformats = ['yyyy-MM-dd', 'shortDate'];
    $scope.endformat = $scope.formats[0];

    //=======================================时间控件 end===============================================================

    $scope.typelist = [{ value: '消息提示' }, { value: '验证' }];

    $scope.searchCondition = {
        startTime: '',
        endTime: '',
        type: '',
        page: 1,
        pageSize: 10

    };

    // 检索
    var getData = function () {
        $http.get(SETTING.ApiUrl + '/MessageDetail/SearchMessageDetail', {
            params: $scope.searchCondition,
            'withCredentials':true
        }).success(function (data) {
            $scope.list = data.List;
            $scope.searchCondition.page = data.Condition.Page;
            $scope.searchCondition.pageSize = data.Condition.PageCount;
            $scope.totalCount = data.totalCount;
        });
    };
    $scope.getDataByTime = getData;
    getData();


    $scope.GetSMSCount= function () {
        $http.get(SETTING.ApiUrl + '/MessageDetail/GetSMSCount').success(function (data) {
            $scope.SMSCounts = data;
        });
    };
        $scope.GetSMSCount();

}]);


angular.module("app").controller('MessageConfigController', ['$http', '$scope', '$state', function ($http, $scope, $state) {
    $scope.searchCondition = {
        page: 1,
        pageSize: 10
    };
    $scope.getList = function () {
        $http.get(SETTING.ApiUrl + '/MessageConfig/SearchMessageConfig', {
            params: $scope.searchCondition ,
            'withCredentials':true
        }).success(function (data) {
            $scope.list = data.List;
            $scope.searchCondition.page = data.Condition.Page;
            $scope.searchCondition.pageSize = data.Condition.PageCount;
            $scope.totalCount = data.totalCount;
        });
    };
    $scope.getList ();
}]);


angular.module("app").controller('MessageConfigCreateController',['$http','$scope','$state',function($http,$scope,$state){
    $scope.MessageConfigModel = {
        Name: '',
        Template: ''

    };
    $scope.Create = function(){
        $http.post(SETTING.ApiUrl + '/MessageConfig/AddMessageConfig', $scope.MessageConfigModel).success(function (data) {
            if(data.Status){
                $state.go("page.CRM.MessageConfigure.index");
            }else{
                alert(data.Msg);
            }

        });
    }
}]);


angular.module("app").controller('MessageConfigEditController',['$http','$scope','$stateParams','$state',function($http,$scope,$stateParams,$state){
    $http.get(SETTING.ApiUrl + '/MessageConfig/GetMessageConfig/' + $stateParams.id,{
        'withCredentials':true
    }).success(function(data){
        $scope.MessageConfigModel =data;
    });

    $scope.Save = function(){
        $http.post(SETTING.ApiUrl + '/MessageConfig/UpdateMessageConfig',$scope.MessageConfigModel,{

        }).success(function(data){
            if(data.Status){
                $state.go("page.CRM.MessageConfigure.index");
            }else{
                alert(data.Msg);
            }
        });
    }
}]);


app.filter('dateFilter',function(){
    return function(date){
    return FormatDate(date);
    }
})
function FormatDate(JSONDateString) {
    jsondate = JSONDateString.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    return date.getFullYear()
        + "-"
        + month
        + "-"
        + currentDate
        + "-"
        + date.getHours()
        + ":"
        + date.getMinutes()
        + ":"
        + date.getSeconds()
        ;

}


