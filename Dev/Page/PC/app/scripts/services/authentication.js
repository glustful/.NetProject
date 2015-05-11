/**
 * Created by Yunjoy on 2015/5/6.
 * �û���֤��½service
 */
angular.module("app").service("AuthService",["$http",function($http){
    var _isAuthenticated = false;
    var _currentUser;
    /**
     * �Ƿ�ӵ����Ȩ
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthorized = function(){
        return false;
    };
    /**
     * �Ƿ��Ѿ���½
     * @returns {boolean}
     * @constructor
     */
    this.IsAuthenticated = function(){
        return _isAuthenticated
    };
    /**
     * ��ǰ�û�
     * @returns CurrentUser
     * @constructor
     */
    this.CurrentUser = function(){
        return _currentUser;
    };
    /**
     * ��½����
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