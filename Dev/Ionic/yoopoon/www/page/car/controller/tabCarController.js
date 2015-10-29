app.controller('TabCarCtrl', function($scope,$state,$ionicHistory,AuthService,  $ionicSlideBoxDelegate,cartservice) {
  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //
  //  window.onload=function(){
  //
  //  }
    //

    $ionicHistory.clearHistory();
    //$ionicConfigProvider.views.maxCache(0);
var carlistcount=0;

    var getcar =function (){
        //window.location.reload();
        var storage=window.localStorage.ShoppingCart;
        if(storage!=undefined)
        {
            var jsonstr = JSON.parse(storage.substr(1, storage.length));
            $scope.productlist = jsonstr.productlist;
            carlistcount=$scope.productlist.length;
        }

    };
    $scope.$on('$viewContentLoaded', function() {
        getcar();
    });
    getcar();


    $scope.chats = [{
    id: 0,
    name: 'Ben Sparrow',
    lastText: 'You on your way?',
    face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
  }, {
    id: 1,
    name: 'Max Lynx',
    lastText: 'Hey, it\'s me',
    face: 'https://avatars3.githubusercontent.com/u/11214?v=3&s=460'
  }, {
    id: 2,
    name: 'Adam Bradleyson',
    lastText: 'I should buy a boat',
    face: 'https://pbs.twimg.com/profile_images/479090794058379264/84TKj_qa.jpeg'
  }, {
    id: 3,
    name: 'Perry Governor',
    lastText: 'Look at my mukluks!',
    face: 'https://pbs.twimg.com/profile_images/598205061232103424/3j5HUXMY.png'
  }, {
    id: 4,
    name: 'Mike Harrington',
    lastText: 'This is wicked good ice cream.',
    face: 'https://pbs.twimg.com/profile_images/578237281384841216/R3ae1n61.png'
  }];

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




    //region 全选 单选
    $scope.start=false;//默认未选中
    $scope.allButton=false;//全选
    $scope.choseArr=[];//定义数组用于存放前端显示
    $scope.all=function(c,v){

        if(c==true){
            $scope.choseArr=[""];
            cleanchoseArr();
            for( j=0;j<$scope.productlist.length;j++)
            {
                cleanchoseArr();
                $("#check"+$scope.productlist[j].id).find(":checkbox")[0].checked=true;
                $scope.choseArr.push($scope.productlist[j].id);
            }
           // document.getElementById('ccsum').removeAttribute('disabled');
           // var valuss=v;
           // $scope.choseArr=valuss;
        }else{
            for( i=0;i<$scope.productlist.length;i++)
            {
                $("#check"+$scope.productlist[i].id).find(":checkbox").eq(0).prop('checked', false);
            }

            $scope.choseArr=[""];
            cleanchoseArr();
           // document.getElementById('ccsum').setAttribute('disabled');
        }
        allprice();

    }
    var cleanchoseArr=function() {
        for(i=0;i<$scope.choseArr.length;i++)
        {
            if($scope.choseArr[i]=="")
            {
                $scope.choseArr.splice(i,1);
            }
        }
    }
    $scope.chanage = function(t) {
        // 修改折扣后触发事件
        var node = t.item;//当前值
        var isExit=false;//是否存在 默认不存在
        cleanchoseArr();
        if($("#check"+node.id).find(":checkbox")[0].checked)//如果当前这个值选中
        {
            //判断当前Id是否在choseArr存在
            for(i=0;i<$scope.choseArr.length;i++) {
                if($scope.choseArr[i]==node.id)
                {
                    isExit=true;//列表中存在
                }
            }
            if(!isExit)//列表中不存在当前值
            {
                $scope.choseArr.push(node.id);
            }
            //判断当前这些在值 和列表中值是否一样
            cleanchoseArr();

            //再次遍历所有checkbox是否都选中
            $scope.allButton=true;
        for(i=0;i<$scope.productlist.length;i++)
        {
           if( !$("#check"+$scope.productlist[i].id).find(":checkbox")[0].checked)//判断是否选中
           {

               $scope.allButton=false;
           }
        }
        }else{
            $scope.allButton=false;
            $("#check"+node.id).find(":checkbox")[0].checked=false;
            cleanchoseArr();

            for(i=0;i<$scope.choseArr.length;i++) {

                if($scope.choseArr[i]==node.id)
                {
                    //$scope.choseArr.remove(i);
                   $scope.choseArr.splice(i,1);
                }
            }
        }
        allprice();
    }
    //endregion

    //region 数量增加减

    $scope.adding=function(id){
        cartservice.addone(id);
        for(j=0;j<$scope.productlist.length;j++){
            if($scope.productlist[j].id==id){
                $scope.productlist[j].count=  $scope.productlist[j].count+1;
            }
        }
        allprice();


        }


    $scope.decrease=function(id){

        for(j=0;j<$scope.productlist.length;j++){
            if($scope.productlist[j].id==id){
                if(  $scope.productlist[j].count>1){
                    $scope.productlist[j].count=  $scope.productlist[j].count-1;
                    cartservice.delete(id);
                }
            }

        }
        allprice();

}

    //endregion


    //region 计算总价
    $scope.dprice=0;
    $scope.sum=0;


   var allprice=function(){
        var prices=0;
       $scope.sum=$scope.choseArr.length;

       for(i=0;i< $scope.choseArr.length;i++)
       {
           for(j=0;j<$scope.productlist.length;j++){
               if($scope.choseArr[i]==$scope.productlist[j].id){
                   prices+= parseFloat($scope.productlist[j].price * $scope.productlist[j].count);
                 //  $scope.sum+=1;

               }

           }
       }
       if( $scope.sum>0){
           document.getElementById('ccsum').removeAttribute('disabled');
       }
       else{
           // document.getElementById('ccsum').setAttribute('disabled');
           //  document.getElementById('ccsum').disabled="disabled";
           $("#ccsum").attr("disabled",true);
           // $("#ccsum").css("disabled","disabled");
       }
        $scope.dprice=prices;
    }
    //endregion

    //region 删除编辑

    $scope.flag={showDelete:false,showReorder:false};
    $scope.items=["Chinese","English","German","Italian","Janapese","Sweden","Koeran","Russian","French"];
    $scope.delete_item=function(id){
       cartservice.deletethis(id)
        getcar();
        $scope.all();
    };
    $scope.move_item = function(item, fromIndex, toIndex) {
        $scope.items.splice(fromIndex, 1);
        $scope.items.splice(toIndex, 0, item);
    };
    //endregion

    //region 结算
    $scope.jiesuan=function(){

        $scope.currentuser= AuthService.CurrentUser(); //调用service服务来获取当前登陆信息
        if( $scope.currentuser==undefined ||  $scope.currentuser=="")
        {
            $state.go("page.login");//调到登录页面
        }
        else
        {
            $scope.productcount=[];
            for(i=0;i< $scope.choseArr.length;i++)
            {
                for(j=0;j<$scope.productlist.length;j++){
                    if($scope.choseArr[i]==$scope.productlist[j].id){
                        $scope.productcount.push($scope.productlist[j])
                    }
                }
            }
            $state.go("page.order",{productcount:$scope.productcount,pricecount:$scope.dprice})
        }

    }

    //endregion

});




app.controller('CarDetailCtrl', function($scope, $stateParams) {
  
  $scope.chat = {
    id: 0,
    name: 'Ben Sparrow',
    lastText: 'You on your way?',
    face: 'https://pbs.twimg.com/profile_images/514549811765211136/9SgAuHeY.png'
  };
  

  $scope.goBack = function(){
    window.history.go(-1);
  };

//    我的订单
    $scope.tabIndex=1;
    $scope.getOrderList1=function(){
        $scope.tabIndex=1;
    };
    $scope.getOrderList2=function(){
        $scope.tabIndex=2;
    };
    $scope.getOrderList3=function(){
        $scope.tabIndex=3;
    };
    $scope.getOrderList4=function(){
        $scope.tabIndex=4;
    };

});