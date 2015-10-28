

app.controller('tabservice',['$http','$scope',function($http,$scope){
	$scope.go = function (state) {
		window.location.href = state;
	};

	//    搜索功能
	$scope.showSelect = false;
	$scope.isShow = false;
	$scope.showInput = function () {
		$scope.showSelect = true;
		$scope.isShow = true;
	};
//  下拉框效果
	$scope.selectBox=false;
	$scope.showSelectBox=function(){
		$scope.selectBox=true;
	}
//	关闭下拉框
	$scope.closeSelectBox=function(){
		$scope.selectBox=false;
	}

	//region地址获取
	$scope.Condition = {
		Page: 1,
		father: true,
		Parent_Id: ''
	};
	$scope.pare = [];

	var getAddress = function () {
		$http.get(SETTING.ApiUrl + '/CommunityArea/Get', {
			params: $scope.Condition,
			'withCredentials': true
		}).success(function (data3) {
			if (data3.List != "") {
				$scope.addrss = data3.List;
				$scope.selected = data3.List[0].Id;//如果想要第一个值
				//for( i=0;i<data3.List.length;i++){
				//    if(data3.List[i].Parent=null)
				//    {
				//        $scope.pare.push (data3.List[i].Parent);
				//    }}
				//alert($scope.pare);
			}
		});
	}
	getAddress();
	$scope.SCondition = {

		Parent_Id: ''
	};

	$scope.area="云南省"
	$scope.click = function (area,id) {
		$scope.area=area;
		//$scope.selectBox=false;
		$scope.SCondition.Parent_Id =id
		$http.get(SETTING.ApiUrl + '/CommunityArea/Get', {
			params: $scope.SCondition,
			'withCredentials': true
		}).success(function (data) {
			$scope.zilei = data.List;
		})

	}
	$scope.huan=function(area){
		$scope.area=area;
		$scope.selectBox=false;
	}


//region 图片轮播
	$scope.channelName = 'banner';
	$http.get(SETTING.ApiUrl+'/Channel/GetTitleImg', {
		params: {ChannelName: $scope.channelName},
		'withCredentials': true
	}).success(function (data) {
		$scope.content = data;
	});
	//endregion


}]);



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
				Status: '1',
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
				Status: '2',
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

app.controller('clearservice',['$http','$scope', '$state','$stateParams',function($http,$scope, $state,$stateParams){
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


	$scope.selected = "";
    //    选择清洗服务

    $scope.selectService = function(sel) {
		$scope.selected =sel;
		$("#li"+sel).attr("class","distance border-css");
//		$("#li"+sel:even).attr("class","distance border-css specialStyle");

	for(i=0;i<	$scope.items.length;i++)
	{
		if($scope.items[i].Id!=sel)
		{
			$("#li"+$scope.items[i].Id).attr("class","distance");
		}
	}

    }

	$scope.buysome=function(){
if($scope.selected!=undefined && $scope.selected!="") {
	$http.get(SETTING.ApiUrl + '/CommunityProduct/Get?id=' + $scope.selected, {
		'withCredentials': true
	}).success(function (data) {
		$scope.itema = data.ProductModel;
		$state.go("page.order", {productcount: $scope.itema, pricecount: $scope.itema.Price})
	});
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
