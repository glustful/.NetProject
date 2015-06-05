/**
 * Created by gaofengming on 2015/5/28.
 */
app.controller('SecuritySettingController',function($scope,$http){
    $scope.oldpw ={
        Id:11,
        oldPassword:'000000',
        newPassword:'11111'
    } ;
    $http.post(SETTING.ApiUrl+'User/ChangePassword',$scope.oldPassword,{'withCredentials':true}).success(function(data){ })
})






app.directive('pwCheck', [function () {
     return {
           require: 'ngModel',
             link: function (scope, elem, attrs, ctrl) {
               var firstPassword = '#' + attrs.pwCheck;
               elem.add(firstPassword).on('keyup', function () {
             scope.$apply(function () {
                              var v = elem.val()===$(firstPassword).val();
                         ctrl.$setValidity('pwmatch', v);
                 });
                  });
          }
     }
   }]);