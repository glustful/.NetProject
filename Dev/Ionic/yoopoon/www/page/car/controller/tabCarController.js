app.controller('TabCarCtrl', function($scope, $ionicSlideBoxDelegate,cartservice) {
  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //
  //  window.onload=function(){
  //
  //  }
    $scope.$on('$viewContentLoaded', function() {
        getcar();
    });

var carlistcount=0;

    var getcar =function (){
        var storage=window.localStorage.ShoppingCart;
        if(storage!=undefined)
        {
            var jsonstr = JSON.parse(storage.substr(1, storage.length));
            $scope.productlist = jsonstr.productlist;
            carlistcount=$scope.productlist.length;
        }

    };

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

    ////全选按钮功能
//    $scope.start=false;
//    $scope.allButton=false;
//    $scope.all=function(){
//        if($scope.allButton==false){
//            $scope.start=false;
//        }if($scope.allButton==true){
//            $scope.start=true;
//        }
//    }

    var el=document.getElementsByTagName("input");
   var a=el.length;
    console.log(a);
    var len=el.length;
    $scope.allButton=function(){
        for(var i=0;i<len;i++){

            if(el[len-1].checked==true) {
                el[i].checked = true;
            }else{
                el[i].checked=false;
            }
        }


        //for(i=0;i<$scope.productlist.length;i++){
        //    if($scope.start[i]=false)
        //    {
        //        $scope.allButton==false
        //    }
        //    if( $scope.allButton==true){$scope.start=true;}
        //    if( $scope.allButton==false){$scope.start=false;}
        //}
//        if($scope.allButton==false){
//            $scope.start=false;
//            $scope.dprice=0;
////            alert("nihao")
//        }
//          if($scope.allButton==true){
//            $scope.start=true;
//            //总计
//            allprice();
//
////            alert($scope.start)
//
//        }

    }
    var cleanchoseArr=function()
    {
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
        // if(t.start==true)//选中状态

        if($("#check"+node.id).find(":checkbox")[0].checked)
        {
            for(i=0;i<$scope.choseArr.length;i++) {
                if($scope.choseArr[i].id==node.id)
                {
                    isExit=true;//列表中存在
                }
            }
            if(!isExit)//列表中不存在当前值
            {
                $scope.choseArr.push(node);
            }
            //判断当前这些在值 和列表中值是否一样
            cleanchoseArr();

            //再次遍历所有checkbox是否有选中
        for(i=0;i<$scope.productlist.length;i++)
        {
           if( $("#check"+$scope.productlist[i].id).find(":checkbox")[0].checked)
           {
               var novalue=false;
               for(j=0;j<$scope.choseArr.length;j++) {
                   //循环遍历 当前选中的值是否在 choseArr中存在
                   if($scope.productlist[i].id==$scope.choseArr[j].id)
                   {
                       novalue=true;//存在
                   }
               }
               if(!novalue)
               {
                   var vss=$scope.productlist[i];
                   $scope.choseArr.push(vss);
               }
           }
        }

            if($scope.choseArr.length==$scope.productlist.length)
            {
                $scope.allButton=true;
            }else{
                $scope.allButton=false;
            }



        }else{
            $scope.allButton=false;
            $("#check"+node.id).find(":checkbox")[0].checked=false;
            cleanchoseArr();

            //$scope.choseArr.forEach(function(element, index, array){
            //    if(element.id ===node.id){
            //        $scope.choseArr.remove(index);
            //    }
            //});

            for(i=0;i<$scope.choseArr.length;i++) {

                if($scope.choseArr[i].id==node.id)
                {
                    //$scope.choseArr.remove(i);
                   $scope.choseArr.splice(i,1);
                }
            }
            //if($scope.productList.length<=0 || carlistcount!=$scope.productList.length || $scope.productList==undefined || $scope.productList=="")
            //if( $scope.productList==undefined )
            //{
            //    getcar();
            //}else{
            //    if($scope.productList.length<=0 || carlistcount!=$scope.productList.length)
            //    {
            //        getcar();
            //    }
            //}
        }
    }




    //$scope.$watch('$scope.productlist',aa,false);
    //function aa(){
    //    for(i=0;i<$scope.productlist.length;i++){
    //        if($scope.start[i]=false)
    //        {
    //            $scope.allButton==false
    //        }
    //        if( $scope.allButton==true){$scope.start=true;}
    //        if( $scope.allButton==false){$scope.start=false;}
    //    }
    //}
//



///数量增加减

    $scope.adding=function(id){
        cartservice.addone(id);
        getcar();
        allprice();
        }


    $scope.decrease=function(id){
        cartservice.delete(id);
        getcar();
        allprice();
}
//总计
    $scope.dprice=0;

   var allprice=function(){
        var prices=0;

        for(i=0;i<$scope.productlist.length;i++){
            if($scope.start==true){
                prices+= $scope.productlist[i].newprice * $scope.productlist[i].count;
               }
        }
        $scope.dprice=prices;
    }
//allprice();



//编辑
    $scope.flag={showDelete:false,showReorder:false};
    $scope.items=["Chinese","English","German","Italian","Janapese","Sweden","Koeran","Russian","French"];
    $scope.delete_item=function(id){
       cartservice.deletethis(id)
        getcar();
    };
    $scope.move_item = function(item, fromIndex, toIndex) {
        $scope.items.splice(fromIndex, 1);
        $scope.items.splice(toIndex, 0, item);
    };
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