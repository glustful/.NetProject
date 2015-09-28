app.controller('TabServiceCtrl', function($scope,$http, $ionicSlideBoxDelegate, $timeout,AuthService, $ionicHistory,$stateParams) {
	// With the new view caching in Ionic, Controllers are only called
	// when they are recreated or on app start, instead of every page change.
	// To listen for when this page is active (for example, to refresh data),
	// listen for the $ionicView.enter event:
	//
	//$scope.$on('$ionicView.enter', function(e) {
	//});

    $scope.$on('$ionicView.enter',function(){
        $ionicSlideBoxDelegate.start();
    });

	$scope.model = {
		activeIndex: 0
	};
	al


	// 遮罩层
    //
	//var tip1 = document.getElementById("tiphidden1");
	//var tip2 = document.getElementById("tiphidden2");
	//$scope.hide=true;
	//$scope.closetips = function() {
	//	tip1.style.display = "none";
	//	tip2.style.display = "none";
	//	localStorage.x1 = "none";
	//	$scope.hide=false;
	//};
	//$scope.hide=true;
	//function save() {
	//	if (localStorage.x1) {
	//		tip1.style.display = "none";
	//		tip2.style.display = "none";
	//		$scope.hide=false;
	//	}
	//}
	//save();

	$scope.pageClick = function(index) {
		//alert(index);
		//alert($scope.delegateHandler.currentIndex());
		$scope.model.activeIndex = 2;
	};

	$scope.slideHasChanged = function($index) {
		//alert($index);
		//alert($scope.model.activeIndex);
	};
	$scope.delegateHandler = $ionicSlideBoxDelegate;

	//    页面跳转
	$scope.go = function(state) {
		window.location.href = state;
	};

	//    页面跳转到页脚导航
	$scope.goes = function(state) {
		$ionicHistory.clearHistory();
		window.location.href = state;
	}
	//获取当前用户信息
	$scope.currentuser= AuthService.CurrentUser();
	//我的服务
	function tab() {
		//待接件
		    if ($stateParams.tabIndex == 5) {
			$scope.tabIndex = 5;
			$scope.condition = {
				Status: '4',
				Addusers: $scope.currentuser.UserId
			};
			var getRec = function () {
				$http.get(SETTING.ApiUrl+'/ServiceOrderDetail/Get',{params:$scope.condition,'withCredentials':true})
					.success(function(data) {
						//alert("recive ");
						$scope.list = data.List;
					});
			};
			getRec();
		}
		//办理中
		if ($stateParams.tabIndex == 6) {
			$scope.tabIndex = 6;
			$scope.condition = {
				Status: '5',
				Addusers: $scope.currentuser.UserId
			};
			var getHandle = function () {
				$http.get(SETTING.ApiUrl+'/ServiceOrderDetail/Get',{params:$scope.condition,'withCredentials':true})
					.success(function(data) {
						$scope.list = data.List;
					});
			};
			getHandle();
		}
	}
	tab();
	//$scope.tabIndex = 5;
	$scope.getServiceList = function (tabIndex) {
		$scope.tabIndex = tabIndex;
		if(	$scope.tabIndex == 5){
			$scope.condition = {
				Status: '4',
				Addusers: $scope.currentuser.UserId
			};
			var getList = function () {
				$http.get(SETTING.ApiUrl+'/ServiceOrderDetail/Get',{params:$scope.condition,'withCredentials':true})
					.success(function(data) {
					$scope.list = data.List;
				});
			};
			getList();

		}
		if($scope.tabIndex == 6){
			$scope.condition = {
				Status: '5',
				Addusers: $scope.currentuser.UserId
			};
			var getList1 = function () {
				$http.get(SETTING.ApiUrl+'/ServiceOrderDetail/Get',{params:$scope.condition,'withCredentials':true})
					.success(function(data) {
						$scope.list = data.List;
					});
			};
			getList1();
		}
	};


	//    滚动刷新
	$scope.items = [];
	var base = 0;
	$scope.load_more = function() {
		$timeout(function() {
			for (var i = 0; i < 10; i++, base++)
				$scope.items.push(["item ", base].join(""));
			$scope.$broadcast("scroll.infiniteScrollComplete");
		}, 500);

	};


});

app.controller('clearservice',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
// money
	if( $stateParams.name==undefined ||  $stateParams.name=="" )
	{
		$scope.go("page.service");
	}



	$scope.loadmore=true;
	$scope.Name=$stateParams.name;
	$scope.items = [];
	$scope.searchCondition = {

		CategoryName:$stateParams.name,
		IsDescending: true,
		OrderBy: 'OrderByAddtime',
		Page: 1,
		PageCount: 10
		//ProductId:''
	};

	var getList = function () {
		$http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
			params: $scope.searchCondition,
			'withCredentials': true
		}).success(function (data) {
			if (data.List != "") {
				$scope.items = data.List;
				if(((($scope.searchCondition.Page-1)*$scope.searchCondition.PageCount )+data.List.length)==data.TotalCount)
				{
					$scope.loadmore=false;
				}
			}
		});
	};
	getList();
//endregion
	//region 商品加载

	$scope.load_more = function () {
		$timeout(function () {
			$scope.searchCondition.Page += 1;
			$http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
				params: $scope.searchCondition,
				'withCredentials': true
			}).success(function (data) {

				if (data.List != "") {
					for (var i = 0; i < data.List.length; i++) {
						$scope.items.push(data.List[i]);
					}
					if($scope.items.length==data.TotalCount)
					{
						$scope.loadmore=false;
					}
				}
				$scope.$broadcast("scroll.infiniteScrollComplete");
			});
		}, 1000)
	};



    //    选择清洗服务
    $scope.selected1 = false;
    $scope.selected2 = false;
    $scope.selected3 = false;
    $scope.selected4 = false;
    $scope.selected5 = false;
    $scope.selectService = function(sel) {
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

}]);

app.controller('safeservice',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
// no money

	if( $stateParams.name==undefined ||  $stateParams.name=="" )
	{
		$scope.go("page.service");
	}

	$scope.loadmore=true;
    $scope.Name=$stateParams.name;
	$scope.items = [];
	$scope.searchCondition = {

		CategoryName:$stateParams.name,
		IsDescending: true,
		OrderBy: 'OrderByAddtime',
		Page: 1,
		PageCount: 10
		//ProductId:''
	};

	var getList = function () {
		$http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
			params: $scope.searchCondition,
			'withCredentials': true
		}).success(function (data) {
			if (data.List != "") {
				$scope.items = data.List;
				if(((($scope.searchCondition.Page-1)*$scope.searchCondition.PageCount )+data.List.length)==data.TotalCount)
				{
					$scope.loadmore=false;
				}
			}
		});
	};
	getList();
//endregion
	//region 商品加载

	$scope.load_more = function () {
		$timeout(function () {
			$scope.searchCondition.Page += 1;
			$http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
				params: $scope.searchCondition,
				'withCredentials': true
			}).success(function (data) {

				if (data.List != "") {
					for (var i = 0; i < data.List.length; i++) {
						$scope.items.push(data.List[i]);
					}
					if($scope.items.length==data.TotalCount)
					{
						$scope.loadmore=false;
					}
				}
				$scope.$broadcast("scroll.infiniteScrollComplete");
			});
		}, 1000)
	};

}]);

app.controller('safedetailservice',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
// no money

	if( $stateParams.name==undefined ||  $stateParams.name=="" ||  $stateParams.id==undefined ||  $stateParams.id=="" )
	{
		$scope.go("page.service");
	}
	$scope.Name=$stateParams.name;
	$scope.searchCondition = {
		id:$stateParams.id
	};

	var getList = function () {
		$http.get(SETTING.ApiUrl + '/CommunityProduct/Get', {
			params: $scope.searchCondition,
			'withCredentials': true
		}).success(function (data) {
			if (data.ProductModel != "" && data.ProductModel !=undefined) {
				$scope.item = data.ProductModel;
			}
		});
	};
	getList();


}]);
