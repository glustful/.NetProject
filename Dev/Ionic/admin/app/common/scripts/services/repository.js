/**
 * Created by Yunjoy on 2015/9/10.
 */
app.service("repository",["$http","$q",function(http,q){
    this.get = function(resource,params){
        var deferred = q.defer();
        http.get(SETTING.ZergWcApiUrl + '/'+resource +'/get',{
            params:params,
            withcredential:true})
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (data) {
                deferred.reject(data);
            });
        return deferred.promise;
    };
    this.post = function(resource,data){
        var deferred = q.defer();
        http.post(SETTING.ZergWcApiUrl + '/'+resource +'/post',data,{
            withcredential:true})
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (data) {
                deferred.reject(data);
            });
        return deferred.promise;
    };
    this.delete = function(resource,id){
        var deferred = q.defer();
        http.delete(SETTING.ZergWcApiUrl + '/'+resource +'/delete',{
            params:{id:id},
            withcredential:true})
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (data) {
                deferred.reject(data);
            });

        return deferred.promise;
    };
    this.put = function(resource,data){
        var deferred = q.defer();
        http.put(SETTING.ZergWcApiUrl + '/'+resource +'/put',data,{
            withcredential:true})
            .success(function (data) {
                deferred.resolve(data);
            }).error(function (data) {
                deferred.reject(data);
            });
        return deferred.promise;
    }
}]);