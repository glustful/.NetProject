/**
 * Created by Craig.Y.Duan on 2015/8/10.
 */
app.service("PortalService", ["$http", '$sessionStorage', '$q','$rootScope' ,
    function ($http, $sessionStorage, $q,$rootscope) {
    //region private member
    //var _activePortalId = 0, _portalList = undefined, _settting = undefined;
    var promise = undefined;

    var getSetting = function (portalId) {
        $http.get(SETTING.ZergWcApiUrl + "/portal/GetSettingDetial?id=" + portalId).success(function (data) {
            $rootscope.settting = data;
        });
    };

    var autoSetPrivateField = function () {
        $rootscope.portalList = $sessionStorage.PortalList;
        if (!$sessionStorage.ActivedPortalId && $rootscope.portalList.length > 0) {
            $sessionStorage.ActivedPortalId = $rootscope.portalList[0].Id;
            //$sessionStorage.activePortalName = _portalList[0].Name;
        }
        $rootscope.activePortalId = $sessionStorage.ActivedPortalId;
        //_activePortalName = $sessionStorage.activePortalName;
        //if(_activePortalId >0){
        //    getSetting(_activePortalId);
        //}
        for (var i = 0; i < $rootscope.portalList.length; i++) {
            if ($rootscope.portalList[i].Id == $rootscope.activePortalId) {
                $rootscope.activedPortal = $rootscope.portalList[i];
                break;
            }
        }
    };

    /**
     * 获取公众号列表方法承诺
     * @returns {promise}
     */
    var getPortalList = function () {
        var deferred = $q.defer();
        $http.get(SETTING.ZergWcApiUrl + "/portal/GetSettingList").success(function (data) {
            deferred.resolve(data);
        }).error(function (data) {
            deferred.reject(data);
        });
        return deferred.promise;
    };

    /**
     * 同步获取公众号列表
     */
    var getAsyncPortalList = function () {
        promise = getPortalList().then(function (data) {
            $sessionStorage.PortalList = data.List;
            autoSetPrivateField()
        });
    };
    //endregion

    //region public member
    /**
     * 获取激活的门户Id
     * @return {number}
     */
    this.ActivedPortalId = function () {
        return $rootscope.activePortalId;
    };
    /**
     * 设置门户Id
     * @param id
     * @constructor
     */
    this.SetActivePortalId = function (id) {
        if (id && id > 0) {
            //$rootscope.activePortalId = id;
            $sessionStorage.ActivedPortalId = id;
            autoSetPrivateField();
        }
    };

    /**
     * 刷新门户列表
     * @constructor
     */
    this.RefreshPortalList = function () {
        getAsyncPortalList();
    };
    //endregion

    if (!$sessionStorage.PortalList) {
        getAsyncPortalList()
    } else {
        autoSetPrivateField();
    }
}]);