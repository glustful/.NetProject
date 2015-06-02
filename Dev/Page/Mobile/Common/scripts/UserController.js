/**
 * Created by gaofengming on 2015/6/2.
 */
app.controller('UserController',function($scope,$location){

    var browserurl = $location.Url();
    if(browserurl=="?/user/login")
    {
        $scope.CurrentUrl = "µÇÂ¼"
    }
    if(browserurl=="?/user/login")
    {
        $scope.CurrentUrl="×¢²á"
    }
})