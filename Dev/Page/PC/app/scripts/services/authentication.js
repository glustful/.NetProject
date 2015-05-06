/**
 * Created by Yunjoy on 2015/5/6.
 * 用户验证登陆service
 */
angular.module("Zerg.Service").service("AuthenticationService",["$http",function($http){
    var userName; //用户名
    var IsAuthorised = false;
    this.doLogin = function(userName,password){
        $http.post("",
            {
                UserName:userName,
                Password:password
            })
            .success(function(data){
                if(data.Status){
                    return true;
                }else{
                    return false;
                }
            })
            .error(function(data){
                return false;
            })
    };
}]);