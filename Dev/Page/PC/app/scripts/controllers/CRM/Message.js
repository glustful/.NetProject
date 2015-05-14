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
        type: '消息提示',
        page: 1,
        pageSize: 10,
        totalPage: 2
    };

    // 检索
    var getData = function () {
        $http.get(SETTING.ApiUrl + '/MessageDetail/SearchMessageDetail', { params: $scope.searchCondition }).success(function (data) {
            $scope.list = data;
        });
    };
    $scope.getDataByTime = getData;
    getData();

}]);


angular.module("app").controller('MessageConfigController', ['$http', '$scope', '$state', function ($http, $scope, $state) {

    //=====================================================添加 start==========================================================================
    $scope.MessageConfigModel = {
        Name: '',
        Template: ''

    };

    // 添加
    var AddMessage = function () {
        $http.post(SETTING.ApiUrl + '/MessageConfig/AddMessageConfig', $scope.MessageConfigModel).success(function (data) {
            // 从新刷新底部
            if (data.Status) {

                $scope.MessageConfigModel.Name = '';
                $scope.MessageConfigModel.Template = '';
                getMessageConfig();

            }
        });
    };

    $scope.AddNewMessage = AddMessage;
    //=====================================================添加 end==========================================================================

    //======================================================检索 start=======================================================================
    $scope.SeachMessageConfigCondition = {
        page: '1',
        pageSize: '1',
        totalPage: '100'
    };

    var getMessageConfig = function () {
        $http.get(SETTING.ApiUrl + '/MessageConfig/SearchMessageConfig', { params: $scope.SeachMessageConfigCondition }).success(function (data) {
            $scope.list = data;
        });
    };
   
    getMessageConfig();




    //======================================================检索 end=======================================================================






}
]);





