app.service("cartservice", ['$rootScope',

	function($rootScope) {
		//$rootScope.cartProductCount = selectcount();
		utils = {
			setParam: function(name, value) {
				localStorage.setItem(name, value)
			},
			getParam: function(name) {
				return localStorage.getItem(name)
			}
		}

		cartinfo = {
			id: null,
			name: null,
			count: null
		}

		///添加商品和更改已有的商品sevice
		//data.id(商品id)
		//data.name(商品名)
		//data.count(商品数量)
		this.add = function(data) {
			cartinfo.id = data.id;
			cartinfo.name = data.name;
			cartinfo.count = data.count;
			var storage = utils.getParam("ShoppingCart");
			//第一次加入商品 
			if (storage == null || storage == "") {
				var jsonstr = {
					"productlist": [{
						"id": cartinfo.id,
						"name": cartinfo.name,
						"count": cartinfo.count
					}]
				};
				utils.setParam("ShoppingCart", "'" + JSON.stringify(jsonstr));
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
					});
				}
				//保存购物车  
				utils.setParam("ShoppingCart", "'" + JSON.stringify(jsonstr));

			}
		};
		//单独删除商品id
		this.delete = function(id) {
			var storage = utils.getParam("ShoppingCart");
			var jsonstr = JSON.parse(storage.substr(1, storage.length));
			var productlist = jsonstr.productlist;
			var list = [];
			for (var i in productlist) {
				if (productlist[i].id == id) {
					//商品数量为0时候,删除这件商品
					if(productlist[i].count==1)
					{
						//删除这个商品json
						productlist.splice(i,1)
						jsonstr.productlist = productlist;
						utils.setParam("ShoppingCart", "'" + JSON.stringify(jsonstr));
						//总数+1
						$rootScope.cartProductCount -= 1;
						return;
					}
					productlist[i].count = parseInt(productlist[i].count) - 1;
					//jsonstr.totalAmount = parseFloat(jsonstr.totalAmount) - parseInt(productlist[i].num) * parseFloat(productlist[i].price);
				} else {
					//list.push(productlist[i]);
					console.log("删除失败,没有这个商品的ID");
					
				}
			}
			jsonstr.productlist = productlist;
			utils.setParam("ShoppingCart", "'" + JSON.stringify(jsonstr));
		};
		//清空购物车
		this.deleteall = function () {
			localStorage.removeItem("ShoppingCart");
		};
		//查询购物车商品个数(请用变量接收)
		function selectcount ()
		{
			var storage = utils.getParam("ShoppingCart");
			if (storage == null || storage == "") {
				return 0;
			}
			var jsonstr =  JSON.parse(storage.substr(1, storage.length));
			var count = jsonstr.productlist.length;
			return count;
		}

	}
]);