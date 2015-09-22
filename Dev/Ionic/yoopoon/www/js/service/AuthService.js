/**
 * Created by Yunjoy on 2015/5/6.
 * 用户验证登陆service
 */
app.service("AuthService",["$http",'$sessionStorage',function($http,$sessionStorage){
    var _isAuthenticated = false;
    var _currentUser;

    //依据cookies获取当前用户(使用同步获取——仅此一次)
    var xmlhttp=new XMLHttpRequest();
    xmlhttp.open("get",SETTING.ApiUrl+"/user/GetCurrentUser",false);
    xmlhttp.withCredentials = true;
    xmlhttp.send();
    var data = angular.fromJson(xmlhttp.response);
    if (!data.Status) {
    } else {
        _isAuthenticated = true;
        _currentUser = {
            UserName: data.Object.UserName,
            UserId: data.Object.Id
        };
        //$localStorage.UserRoles = data.Object.Roles;
        $sessionStorage.UserRoles=data.Object.Roles;
    }

    /**
     * 是否拥有授权
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthorized = function(access){
        if(!access || !access instanceof  Array || !$sessionStorage.UserRoles)
            return false;
        var hasRole = false;
        for(var i = 0;i<$sessionStorage.UserRoles.length;i++){
            hasRole = access.indexOf($sessionStorage.UserRoles[i].RoleName) >-1;
            if(hasRole)
                break;
        }
        return hasRole;
    };
    /**
     * 是否已经登陆
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
     * 登陆操作
     * @param userName
     * @param password
     * @param callback
     * @param faildCallback
     */
    this.doLogin = function(userName,password,callback,faildCallback){
        $http.post(SETTING.ApiUrl+"/user/LoginEntry",
            {
                UserName:userName,
                Password:password
            },{
                'withCredentials':true
            })
            .success(function(data){
                if(data.Status){
                    _currentUser ={
                        UserName:data.Object.UserName,
                        UserId:data.Object.Id
                    };
                    _isAuthenticated = true;
                    $sessionStorage.UserRoles=data.Object.Roles;
                    if(typeof(callback) === 'function')
                        callback(data);
                }else{
                    if(typeof(faildCallback) === 'function')
                        faildCallback(data);
                }
            });
    };
}]);