app.service("cartservice", ['$rootScope',

	function($rootScope) { 
		$rootScope.cartProductCount = selectcount();
//	    utils = {
//			setParam: function(name, value) {
//				localStorage.setItem(name, value)
//			},
//			getParam: function(name) {
//				return localStorage.getItem(name)
//			}
//		};

		cartinfo = {
			id: null,
			name: null,
			count: null,
            mainimg:null,
            price:null,
            newprice:null,
		    parameterValue:[]
		};

		///添加商品和更改已有的商品sevice
		//data.id(商品id)
		//data.name(商品名)
		//data.count(商品数量)
		this.add = function(data) {
			cartinfo.id = data.id;
			cartinfo.name = data.name;
			cartinfo.count = data.count;
            cartinfo.mainimg=data.mainimg;
            cartinfo.price=data.price;
            cartinfo.newprice=data.newprice;
			cartinfo.parameterValue=data.parameterValue
			var storage = localStorage.getItem("ShoppingCart");
			//第一次加入商品 
			if (storage == null || storage == "") {
				var jsonstr = {
					"productlist": [{
						"id": cartinfo.id,
						"name": cartinfo.name,
						"count": cartinfo.count,
                        "mainimg":cartinfo.mainimg,
                        "price":cartinfo.price,
                        "newprice":cartinfo.newprice,
						"parameterValue":cartinfo.parameterValue
					}]
				};
				localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));
				$rootScope.cartProductCount += 1;
			} else {
				var jsonstr = JSON.parse(storage.substr(1, storage.length));
				var productlist = jsonstr.productlist;
				var result = false;
				for (var i in productlist) {
					if (productlist[i].id == cartinfo.id) {
						productlist[i].count = parseInt(productlist[i].count) + parseInt(cartinfo.count);
						result = true;
					}
				}
				if (!result) {
					//没有该商品就直接加进去  
					productlist.push({
						"id": cartinfo.id,
						"name": cartinfo.name,
						"count": cartinfo.count,
                        "mainimg":cartinfo.mainimg,
                        "price":cartinfo.price,
                        "newprice":cartinfo.newprice,
						"parameterValue":cartinfo.parameterValue
					});
				}
				$rootScope.cartProductCount += 1;
				//保存购物车  
				localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));

			}
		};
		//商品减少数量，默认是减少1件，减少到为0时候这件商品就删除
		this.delete = function(id) {
			var storage = localStorage.getItem("ShoppingCart");
			var jsonstr = JSON.parse(storage.substr(1, storage.length));
			var productlist = jsonstr.productlist;
			for (var i in productlist) {
				if (productlist[i].id == id) {
					//商品数量为0时候,删除这件商品
					if(productlist[i].count==1)
					{
						//删除这个商品json
						productlist.splice(i,1)
						if(productlist.length==0){
							localStorage.removeItem("ShoppingCart");
							return;	
						}
						jsonstr.productlist = productlist;
						localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));
						//总数+1
						$rootScope.cartProductCount -= 1;
						return;
					}
					productlist[i].count = parseInt(productlist[i].count) - 1;
					//jsonstr.totalAmount = parseFloat(jsonstr.totalAmount) - parseInt(productlist[i].num) * parseFloat(productlist[i].price);
				} else {
					console.log("删除失败,没有这个商品的ID");
					
				}
			}
			jsonstr.productlist = productlist;
			localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));
		};
		//直接删除这件商品(无视商品个数)
		this.deletethis = function  (id) {
			var storage = localStorage.getItem("ShoppingCart");
			var jsonstr = JSON.parse(storage.substr(1, storage.length));
			var productlist = jsonstr.productlist;
			for (var i in productlist) {
				if (productlist[i].id == id) {
						//删除这个商品json
						productlist.splice(i,1)
						jsonstr.productlist = productlist;
						if(productlist.length==0){
							localStorage.removeItem("ShoppingCart");
							$rootScope.cartProductCount = 0;
							return;	
						}
						localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));
						//总数+1
						$rootScope.cartProductCount -= 1;
						return;
					productlist[i].count = parseInt(productlist[i].count) - 1;
					//jsonstr.totalAmount = parseFloat(jsonstr.totalAmount) - parseInt(productlist[i].num) * parseFloat(productlist[i].price);
				} else {
					console.log("删除失败,没有这个商品的ID");
					
				}
			}
			jsonstr.productlist = productlist;
			localStorage.setItem("ShoppingCart", "'" + JSON.stringify(jsonstr));
		};
		//清空购物车
		this.deleteall = function () {
			localStorage.removeItem("ShoppingCart");
			$rootScope.cartProductCount = 0;
		};
        //获取总数的service
        this.GetAllcart= function()
        {
            var a =  selectcount();
            $rootScope.cartProductCount = a;
            return a;
        };
		//查询购物车商品个数(请用变量接收)
		function selectcount ()
		{
			var storage = localStorage.getItem("ShoppingCart");
			if (storage == null || storage == "") {
				$rootScope.cartProductCount = 0;
				return 0;
			}
			var jsonstr =  JSON.parse(storage.substr(1, storage.length));
			var count = jsonstr.productlist.length;
			$rootScope.cartProductCount = count;
			return count;
		};
		//获取购物车内所有的商品(返回的是json)
		this.GetAllcartToJson = function () {
			var storage = localStorage.getItem("ShoppingCart");
			if (storage == null || storage == "") {
				return '购物车为空!';
			}
			var jsonstr =  JSON.parse(storage.substr(1, storage.length));
			return jsonstr.productlist;
		}

	}
]);