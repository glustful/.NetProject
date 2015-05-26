/**
 * Created by Yunjoy on 2015/5/16.
 */
angular.module('app').directive('access',['AuthService',function(authService){
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            //var makeVisible = function () {
            //        element.removeClass('hidden');
            //    },
             var  makeHidden = function () {
                     element.remove();
                },
                determineVisibility = function (resetFirst) {
                    var result;
                    //if (resetFirst) {
                    //    makeVisible();
                    //}

                    result = authService.IsAuthorized(roles);
                    if (!result) {
                        makeHidden();
                    }
                    //else {
                    //    makeHidden();
                    //}
                },
                roles = attrs.access.split(',');


            if (roles.length > 0) {
                determineVisibility(true);
            }
        }
    };
}]);