/**
 * Created by gaofengming on 2015/8/11.
 */
app.factory('httpRequestInterceptor', ['$q','$location', function($q, $location) {
    return {
        'responseError': function(rejection) {
            console.log(rejection);
             //do something on error
            if(rejection.status === 0){
             $location.path('/404');
            }
            if (rejection.status === 401) {
                console.log("Response Error 401",rejection);
                $location.path('');
            }
            if (rejection.status === 500) {
                console.log("Response Error 500",rejection);
                $location.path('/505');
            }
            if (rejection.status === 403) {
                console.log("Response Error 403",rejection);
                $location.path('/');
            }

            return $q.reject(rejection);
        }
    };
}])

