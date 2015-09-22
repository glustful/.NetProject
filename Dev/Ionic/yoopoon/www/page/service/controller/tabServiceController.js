app.controller('TabServiceCtrl', function($scope, $ionicSlideBoxDelegate, $timeout,repository,AuthService, $ionicHistory, cartservice,$stateParams) {
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

	//我的服务
	$scope.tabIndex = 5;
	$scope.getServiceList = function (tabIndex) {
		$scope.tabIndex = tabIndex;
	};
	function tab() {
		//获取当前用户信息
		$scope.currentuser= AuthService.CurrentUser();
		//待接件
		if ($stateParams.tabIndex == 5) {
			$scope.tabIndex = 5;
			$scope.condition = {
				Page: 1,
				PageCount: 10,
				Status: '4',
				Addusers: $scope.currentuser.UserId
			};

			//var getList = function () {
			//	repository.get("OrderDetail", $scope.condition).success(function (data) {
			//		$scope.list = data.List;
			//	});
			//};
            //
			//getList();
		}
		//办理中
		if ($stateParams.tabIndex == 6) {
			$scope.tabIndex = 6;
			$scope.condition = {
				Page: 1,
				PageCount: 10,
				Status: '4',
				Addusers: $scope.currentuser.UserId
			};
			//var getList = function () {
			//	repository.get("OrderDetail", $scope.condition).success(function (data) {
			//		$scope.list = data.List;
			//	});
			//};
			//getList();
		}
	}
	tab();


	//    搜索功能
	$scope.showSelect = false;
	$scope.isShow = false;
	$scope.showInput = function() {
		$scope.showSelect = true;
		$scope.isShow = true;
	};
	// 遮罩层

	var tip1 = document.getElementById("tiphidden1");
	var tip2 = document.getElementById("tiphidden2");
    $scope.hide=true;
	$scope.closetips = function() {
		tip1.style.display = "none";
		tip2.style.display = "none";
		localStorage.x1 = "none";
		$scope.hide=false;
	};
    $scope.hide=true;
	function save() {
		if (localStorage.x1) {
			tip1.style.display = "none";
			tip2.style.display = "none";
			$scope.hide=false;
		}
	}
	save();



	//    滚动刷新
	$scope.items = [];
	var base = 0;
	$scope.load_more = function() {
		$timeout(function() {
			for (var i = 0; i < 10; i++, base++)
				$scope.items.push(["item ", base].join(""));
			$scope.$broadcast("scroll.infiniteScrollComplete");
		}, 500);

	}


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
});

app.controller('clearservice',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
// money
	if( $stateParams.name==undefined ||  $stateParams.name=="" )
	{
		$scope.go("page.service");
	}
	$scope.Address={
		AreaName:$stateParams.name,
		AreaId: $stateParams.id,
		Address:'',
		Zip :'',
		Linkman :'',
		Tel:''
	};

	$scope.saves = function () {

		$http.post(SETTING.ApiUrl + '/MemberAddress/Post', $scope.Address, {'withCredentials': true})
			.success(function (data) {
				if (data.Status) {

					$state.go("page.addressAdm");
				}
			});
	}

}]);

app.controller('safeservice',['$http','$scope','$stateParams',function($http,$scope,$stateParams){
// no money

	if( $stateParams.name==undefined ||  $stateParams.name=="" )
	{
		$scope.go("page.service");
	}

	alert($stateParams.name);
    $scope.Name=$stateParams.name;
	$scope.items = [];
	$scope.searchCondition = {

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
			}
		});
	};
	getList();
//endregion
	//region 商品加载
	$scope.loadmore=true;
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

	if( $stateParams.name==undefined ||  $stateParams.name=="" )
	{
		$scope.go("page.service");
	}

	alert($stateParams.name);
	$scope.Name=$stateParams.name;
	$scope.items = [];
	$scope.searchCondition = {

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
			}
		});
	};
	getList();


}]);