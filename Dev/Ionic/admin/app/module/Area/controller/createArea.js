app.controller('AreaCreateCtr', ['$http', '$scope', '$state', '$timeout', function ($http, $scope, $state,$timeout) {
    //$http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?id=" + $scope.id, {
    //    'withCredentials': true
    //}).success(function (data) {
    //    $scope.list = data;
    //})

    //==============================================级联操作===============================================================
    //$scope.text_change = function () {
    //    $scope.isShow3 = false;
    //    //$scope.data.area = "";
    //}

    //$scope.show3 = function () {
       
    //    $scope.isShow3 = true;
        
    //}

    $scope.isShow2 = false;//下拉框状态
    $scope.isShow3 = false;

    $scope.selectId1 = 0;
    $scope.selectId2 = 0;
    $scope.selectId3 = 0;





    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get",{
        'withCredentials': true
    }).success(function (data) {
        $scope.province = data.List;
    })

    $scope.text_change_province = function () {

        $scope.isShow2 = true;
        $scope.isShow3 = false;

        $scope.selectId2 = 0;
        $scope.selectId3 = 0;
        //$scope.data.area = {
        //    Codeid: '',
        //    Name: '',
        //    Parent: { Id: 0 }
        //}
        //$scope.data.city = {
        //    Codeid: '',
        //    Name: '',
        //    Parent: { Id: 0 }
        //}
        $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?Parent_Id="+$scope.selectId1, {
            'withCredentials': true
        }).success(function (data) {
            $scope.area = data.List;
        })
    }

    $scope.text_change_area = function () {
        $scope.isShow3 = true;
        $scope.selectId3 = 0;
        //$scope.data.city = null
        $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?Parent_Id=" + $scope.selectId2, {
            'withCredentials': true
        }).success(function (data) {
            $scope.city = data.List;
        })
    }

    //=============================================================================================================


    $scope.submit = {
        Codeid: '',
        Name: '',
        Parent: {Id:0}
    }
    $scope.save = function () {
        $scope.submit.Parent.Id = $scope.selectId3 != 0 ? $scope.selectId3 : $scope.selectId2 != 0 ? $scope.selectId2 : $scope.selectId1 != 0 ?  $scope.selectId1 : null;
        //if ($scope.data.province != null || $scope.data.province != null && $scope.data.area != null || $scope.data.province != null && $scope.data.area != null && $scope.data.city != null) {
        $http.post(SETTING.ZergWcApiUrl + "/CommunityArea/Post", $scope.submit, {
                'withCredentials': true
            }).success(function (data) {
                if (data.Status) {
                    //$state.go("app.area.show")
                    $scope.state = "成功添加数据！";
                    $timeout(function () {
                        $scope.state = " ";
                    }, 2000);
                }else
                {
                    $scope.state = "数据添加失败！";
                    $timeout(function () {
                        $scope.state = " ";
                    }, 2000);
                }
            })
        }//else
        //{
        //    $scope.state = "请选择归属城市！";
        //}
    }
])