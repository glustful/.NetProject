/** create by 杨波 2015.6.5 创富英雄榜**/
app.controller('BrokerTopThreeController',['$scope','$http','AuthService','$state',function($scope,$http,AuthService,$state){
    var coun=3;//1为经纪人
    //判断是否是经纪人
    $http.get(SETTING.ApiUrl+'/ClientInfo/Getbroker/',{'withCredentials':true})
        .success(function(response) {
            coun=response.count;
            action();
        });
    function action() {
        if (coun === 1)//为经纪人状态
        {
            var BrokerTopThree = function () {
                $http.get(SETTING.ApiUrl + '/BrokerInfo/OrderByBrokerTopThree', {'withCredentials': true}).success(function (data) {
//           $scope.ii=0;
//           for(var i=0;i<data.List.length-1;i++)
//            {
//                $scope.list.push(data.List[i]);
//                $scope.ii=i+1;
//            }
//           console.log($scope.li);
                    $scope.list = data.List;
                    console.log($scope.list);
                })

            };
            BrokerTopThree();

            //获取推荐楼房信息
            var getRecProduct = function () {
                $http.get(SETTING.ApiUrl + '/Brand/GetOneBrand', {params: $scope.searchCondition, 'withCredentials': true}).success(function (data) {
                    $scope.List = data.List;
                });
            };
            getRecProduct();


            //经纪人专区图片轮播
            $scope.channelName = 'banner';
            $http.get(SETTING.ApiUrl + '/Channel/GetTitleImg', {params: {ChannelName: $scope.channelName}, 'withCredentials': true}).success(function (data) {
                $scope.content = data;
            });
            //经纪人专区活动图片轮播
            $scope.channelName = '活动';
            $http.get(SETTING.ApiUrl + '/Channel/GetActiveTitleImg', {params: {ChannelName: $scope.channelName}, 'withCredentials': true}).success(function (data) {
                $scope.Actcontent = data;
            });
        }

        else if (coun === 0) {//一般用户
            $state.go("app.person_setting");//调到设置页面
        }
        else if (coun === 2) {//未登录
            $state.go("user.login");//调到登录页面
        }
    } }]);