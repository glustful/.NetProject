/**
 * Created by gaofengming on 2015/6/15.
 */

app.controller('AddBrokerController',function($scope,$http){
        $scope.YQM={
            Mobile:'',
            SmsType:'6'
        };
        $scope.Invite= function(){
            $http.post(SETTING.ApiUrl+'/SMS/SendSMS',$scope.YQM,{'withCredentials':true}).success(function(data){
                $scope.errorTip=data.Msg;
            })
        }
    }

)