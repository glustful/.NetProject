/**
 * Created by Yunjoy on 2015/5/6.
 * �û���֤��½service
 */
angular.module("Zerg.Service").service("AuthenticationService",["$http",function($http){
    var userName; //�û���
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