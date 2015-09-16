app.controller('TabServiceCtrl', function($scope, $ionicSlideBoxDelegate,$timeout,$ionicHistory) {
  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //
  //$scope.$on('$ionicView.enter', function(e) {
  //});
    var tip1=document.getElementById("tiphidden1");
    var tip2=document.getElementById("tiphidden2");
    function getCookie(cookie_name)
    {
        var allcookies = document.cookie;
        var cookie_pos = allcookies.indexOf(cookie_name);
        if (cookie_pos != -1)
        {
            cookie_pos += cookie_name.length + 1;
            var cookie_end = allcookies.indexOf(";", cookie_pos);
            if (cookie_end == -1)
            {
                cookie_end = allcookies.length;
            }
            var value = unescape(allcookies.substring(cookie_pos, cookie_end));
        }
        return value;
    }
    var tipcookietext = getCookie('tipcookie');
    //如果有cookie
    if(tipcookietext) {
        tip1.style.display = "none";
        tip2.style.display = "none";
    }

  $scope.model = {
    activeIndex:0
  };


  $scope.pageClick = function(index){
    //alert(index);
    //alert($scope.delegateHandler.currentIndex());
    $scope.model.activeIndex = 2;
  };

  $scope.slideHasChanged = function($index){
    //alert($index);
    //alert($scope.model.activeIndex);
  };
  $scope.delegateHandler = $ionicSlideBoxDelegate;

    //    页面跳转
    $scope.go=function(state){
        window.location.href=state;
    };

//    页面跳转到页脚导航
    $scope.goes=function(state) {
        $ionicHistory.clearHistory();
        window.location.href = state;
    }
//    搜索功能
        $scope.showSelect = false;
        $scope.isShow = false;
        $scope.showInput = function () {
            $scope.showSelect = true;
            $scope.isShow = true;
        };
// 遮罩层
        $scope.closetips = function () {
            tip1.style.display = "none";
            tip2.style.display = "none";
            writeCookie('tipcookie', 1000, '999999');

        };
        ///Savecookie
        function writeCookie(name, value, hours) {
            var expire = "";
            if (hours != null) {
                expire = new Date((new Date()).getTime() + hours * 3600000);
                expire = "; expires=" + expire.toGMTString();
            }
            document.cookie = name + "=" + escape(value) + expire;
        }

//    滚动刷新
        $scope.items = [];
            var base = 0;
            $scope.load_more = function () {
                $timeout(function () {
                    for (var i = 0; i < 10; i++, base++)
                        $scope.items.push(["item ", base].join(""));
                    $scope.$broadcast("scroll.infiniteScrollComplete");
                }, 500);
        };

//    选择清洗服务
        $scope.selected1 = false;
        $scope.selected2 = false;
        $scope.selected3 = false;
        $scope.selected4 = false;
        $scope.selected5 = false;
        $scope.selectService = function (sel) {
            switch (sel) {
                case 1:
                    if ($scope.selected1 == false) {
                        $scope.selected1 = true;
                        return;
                    }
                    $scope.selected1 = false;
                    break;
                case 2:
                    if ($scope.selected2 == false) {
                        $scope.selected2 = true;
                        return;
                    }
                    $scope.selected2 = false;
                    break;
                case 3:
                    if ($scope.selected3 == false) {
                        $scope.selected3 = true;
                        return;
                    }
                    $scope.selected3 = false;
                    break;
                case 4:
                    if ($scope.selected4 == false) {
                        $scope.selected4 = true;
                        return;
                    }
                    $scope.selected4 = false;
                    break;
                case 5:
                    if ($scope.selected5 == false) {
                        $scope.selected5 = true;
                        return;
                    }
                    $scope.selected5 = false;
                    break;
            }
        }

})