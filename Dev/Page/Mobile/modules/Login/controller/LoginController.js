/**
 * Created by gaofengming on 2015/6/1.
 */
    var app = angular.module("zergApp");
app.controller('LoginController',['$scope','$state','AuthService',function($scope,$state,AuthService){
    $scope.user={
        name:"",
        password:''
    }
    $scope.Login = function(){
                AuthService.doLogin($scope.user.name,$scope.user.password,function(){
                    if(window.location.hash=="#/user/login"){
                        $state.go('app.personal');
                        console.log(window.location.hash);
                       // console.log(window.location.search)
                    }
                    else{
                        var urlinfo = window.location.href; //获取当前页面的url
                        var len = urlinfo.length;//获取url的长度
                        var phone = urlinfo.indexOf("?");//设置参数字符串开始的位置
                        var url = urlinfo.substr(phone, len);//取出参数字符串 这里会获得类似“id=1”这样的字符串
                        var str=url.substr(1);
                        strs=str.split("&");
                        var request= new Object();
                        for(var i=0;i<strs.length;i++){
                            request[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]);
                        }
                        var Nurl=request['url'];
                        if(!Nurl){
                            $state.go('app.personal')
                        }
                        else{
                            if(Nurl.indexOf("http")==0){
                                    window.location.href=Nurl;
                            }
                            else{
                                try{
                                    $state.go(Nurl);
                                }
                                catch(error){
                                    console.log("页面不存在！");
                                    $state.go('app.personal')
                                }

                            }
                        }
                    }
                },function(data){
                    $scope.errorTips = data.Msg;
                })
    }
}]);

