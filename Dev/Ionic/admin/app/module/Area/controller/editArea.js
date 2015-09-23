app.controller('AreaEditCtr', ['$http', '$scope', '$state', '$stateParams',function ($http, $scope, $state,$stateParams) {
    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?id=" + $stateParams.id, {
        'withCredentials': true  //跨域
    }).success(function (data) {
        $scope.list = data;
    })



    $scope.isShow2 = false;//下拉框状态
    $scope.isShow3 = false;

    $scope.selectId1 = 0;
    $scope.selectId2 = 0;
    $scope.selectId3 = 0;





    $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get", {
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
        $http.get(SETTING.ZergWcApiUrl + "/CommunityArea/Get?Parent_Id=" + $scope.selectId1, {
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

    $scope.submit = {
        Id:'',
        Codeid: '',
        Name: '',
        Parent: { Id: 0 }
    }


    $scope.Create = function () {
        $scope.submit.Id = $scope.list.Id;
        $scope.submit.Codeid = $scope.list.Codeid;
        $scope.submit.Name = $scope.list.Name;
        $scope.submit.Parent.Id = $scope.selectId3 > 0 ? $scope.selectId3 : $scope.selectId2 > 0 ? $scope.selectId2 : $scope.selectId1 > 0 ? $scope.selectId1 : $scope.list.Parent.Id;
        $http.put(SETTING.ZergWcApiUrl + '/CommunityArea/Put', $scope.submit, {
            'withCredentials': true
        }).success(function (data) {
            if (data.Status) {
                //$scope.state = "成功修改数据！";
                $scope.state = "成功修改数据！";
                //window.setInterval(function () {
                //        $scope.state = "";

                //}, 2000);
            } else {
                $scope.state = "数据修改失败！";
            }
        })
    }


}


])