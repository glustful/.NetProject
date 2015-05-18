/**
 * Created by Yunjoy on 2015/5/6.
 * 用户验证登陆service
 */
angular.module("app").service("AuthService",["$http",function($http){
    var _isAuthenticated = false;
    var _currentUser;
    /**
     * 是否拥有授权
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthorized = function(){
        return false;
    };
    /**
     * 是否已经登陆
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthenticated = function(){
        return _isAuthenticated
    };
    /**
     * 当前用户
     * @returns CurrentUser
     * @constructor
     */
    this.CurrentUser = function(){
        return _currentUser;
    };
    /**
     * 登陆操作
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
                    if(typeof(callback) === 'function')
                        callback();
                }else{
                    if(typeof(faildCallback) === 'function')
                        faildCallback();
                }
            });
    };
}]);