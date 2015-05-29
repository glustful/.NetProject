/**
 * Created by chenda on 2015/5/27.
 */

app.controller('tuijianController',['$http','$scope','$stateParams',function($http,$scope,$stateParams) {
    $scope.BrokerRECClientEntity={
        Id:'1',
        Broker:'1',
        NickName:'aaa',
       Sex:'v',
        ClientInfo:'1',
        Qq:'4444',
        Type:'',
        Brokername:'',
        Brokerlevel:'',
        ProjectName:'',
        Projectid:'2',

        Houses:'JSJY',
        HouseType:'SSYT',
        Clientname:'nike',
        Phone:13888888888,
        Note:'hello'
    };
    //$scope.BrokerRECClient.Broker_Id=$stateParams.Broker_Id;
    //$scope.BrokerRECClient.Brokername=$stateParams.Brokername;
    //$scope.BrokerRECClient.Client_Id=$stateParams.ClientName;
    ////$scope.BrokerRECClient.Phone=1;
    ////$scope.BrokerRECClient.QQ=1;
    ////$scope.BrokerRECClient.Brokerlevel="";
    //$scope.BrokerRECClient.Projectid=$stateParams.Projectid;
    //$scope.BrokerRECClient.Projectname=$stateParams.Projectname;


    //增加一条推荐信息
    var getBrokerResult  = function() {
        //alert("sssssssssssssssssssssbbbbbbbbbbbbbbbbbbb");
        alert($scope.BrokerRECClientEntity.Clientname);
        var postData = {text:'long blob of text'};
        var config = {params:$scope.BrokerRECClientEntity};

        var p=JSON.stringify( $scope.BrokerRECClientEntity);



        //$http.post(SETTING.ApiUrl+'/BrokerRECClient/Add',postData, $scope.BrokerRECClientEntity).success(function(data){
        //    if(data.Status){
        //        alert(data.Msg);
        //    }
        //});

        alert(p);

        $.ajax({
            url:SETTING.ApiUrl+'/BrokerRECClient/Add',
            type:'post',
            data:p,
            contentType:'application/json',
            success:function(data){
                alert('yes');
            }
        });

        //$http({
        //    method: 'POST',
        //    url:SETTING.ApiUrl+'/BrokerRECClient/Add',
        //    data:   $scope.BrokerRECClientEntity,  // pass in data as strings
        //    headers : { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8','Accept':'application/json, text/javascript, */*; q=0.01','X-Requested-With':'XMLHttpRequest' }
        //    //   transformRequest: formDataObject
        //}).success(function(data){
        //    alert("chenggong");
        //}).error(function(){
        //    alert("shibai");
        //})




    };
    $scope.add=getBrokerResult;
}])