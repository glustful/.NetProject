/**
 * Created by Yunjoy on 2015/5/6.
 * authentication service
 */
angular.module("app").service("AuthService",["$http",'$localStorage',function($http,$localStorage){
    var _isAuthenticated = false;
    var _currentUser;

    //????cookies?????????(????????????????????)
    var xmlhttp=new XMLHttpRequest();
    xmlhttp.open("get",SETTING.ApiUrl+"/user/GetCurrentUser",false);
    xmlhttp.withCredentials = true;
    xmlhttp.send();
    var data = angular.fromJson(xmlhttp.response);
    if(data.Status){
        _isAuthenticated = true;
        _currentUser = {
            UserName:data.Object.UserName
        };
        $localStorage.UserRoles=data.Object.Roles;
    }

    /**
     * 是否拥有权限
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthorized = function(access){
        if(!access || !access instanceof  Array || !$localStorage.UserRoles)
            return false;
        var hasRole = false;
        for(var i = 0;i<$localStorage.UserRoles.length;i++){
            hasRole = access.indexOf($localStorage.UserRoles[i].RoleName) >-1;
            if(hasRole)
                break;
        }
        return hasRole;
    };
    /**
     * 是否登录
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthenticated = function(){
        //if(_isAuthenticated)
        //    return _isAuthenticated;
        //    $http.get(SETTING.ApiUrl+"/user/GetCurrentUser",{withCredentials:true}).success(function(data){
        //        if(data.Status){
        //            _isAuthenticated = true;
        //            _currentUser ={
        //                UserName:data.Object.UserName
        //            };
        //        }
        //    });
        return _isAuthenticated;
    };
    /**
     * 当前用户
     * @returns CurrentUser
     * @constructor
     */
    this.CurrentUser = function(){
        //if(_currentUser !== undefined)
            return _currentUser;
        //return $cookieStore.get("CurrentUser");
    };
    /**
     * 登录
     * @param userName
     * @param password
     */
    this.doLogin = function(userName,password,callback,faildCallback){
        $http.post(SETTING.ApiUrl+"/user/login",
            {
                UserName:userName,
                Password:password
            },{
                'withCredentials':true
            })
            .success(function(data){
                if(data.Status){
                    _currentUser ={
                        UserName:userName
                    };
                    _isAuthenticated = true;
                    $localStorage.UserRoles=data.Object.Roles;
                    if(typeof(callback) === 'function')
                        callback();
                }else{
                    if(typeof(faildCallback) === 'function')
                        faildCallback();
                }
            });
    };

    this.doLogout=function(callback,faildCallback){
        $http.post(SETTING.ApiUrl+"/user/Logout",
            {'withCredentials':true}
        ).success(function(data){
                //登出成功
                if(data.Status){
                    _currentUser=null;
                    _isAuthenticated=false;
                    $localStorage.UserRoles=null;

                    if(typeof(callback) === 'function')
                        callback();
                }else{
                    if(typeof(faildCallback) === 'function')
                        faildCallback();
                }
            });
    };

}]);