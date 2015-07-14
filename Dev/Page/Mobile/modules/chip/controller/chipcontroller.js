/**
 * Created by 黄秀宇 on 2015/7/9.
 */
app.controller("chipcontroller",function ($scope,$http,$timeout,$sce) {

    $scope.corwdlist={
        Ttitle:'',
        Intro:'',
        Status:'',
        Starttime:'',
        Endtime:'',
        Addtime:'',
        Adduser:'',
        Uptime:'',
        Upuser:''
    };
    $scope.corwdImglist={
        //图片序号
        Orderby:'',
        Imgurl:'',
        Addtime:'',
        Adduser:'',
        Uptime:'',
        Upuser:''
    };
    var len=0;
    var timer=null; //定时器
    //项目列表的数据
    $scope.crowdlist = [];
    //图片地址
    $scope.Img=SETTING.ImgUrl;
    var crowClick=function(state){
        //改变选定众筹状态的颜色
       if(typeof(state)=="undefined"||state==-1)
{   state=-1;
    $scope.allCla="all";
    $scope.crowdCla="";
    $scope.houCla="";
}
       else if(state==1){
           $scope.allCla="";
           $scope.crowdCla="all";
           $scope.houCla="";
       }
        else {
           $scope.allCla="";
           $scope.crowdCla="";
           $scope.houCla="all";}
//----------------------------查询众筹列表 start----------------------------
    $http.get(SETTING.eventApiUrl+'/CrowdApi/GetCrowdInfo?status='+state, {'withCredentials': true})
        .success(function(response) {
            if(response.list.length>0){$scope.tips="";
            $scope.crowdlist = [];
            //项目的图片序列
            $scope.i='';
            //当前项目的图片序列
            $scope.j=0;
            len=response.list.length;
            //获取到项目列表中的所有数据
            for (var i = 0; i < response.list.length; i++)
            {
                $scope.crowdlist[i]=response.list[i];
                //获取到项目的图片List
                $scope.crowdlist[i].i= i+1;
                $scope.crowdlist[i].Intro=$sce.trustAsHtml($scope.crowdlist[i].Intro) ;
                if($scope.crowdlist[i].Status==1)
                {
                    $scope.crowdlist[i].Status="众筹中";
                }
                if($scope.crowdlist[i].Status==0)
                {
                    $scope.crowdlist[i].Status="选房中";
                }

          for(var j=0;j<$scope.crowdlist[i].ImgList.length;j++)
       {
                $scope.crowdlist[i].ImgList[j].Id=j+1;
                if(j===0)
                 { $scope.crowdlist[i].ImgList[j].Uptime= "display:block;";}
                else
                 { $scope.crowdlist[i].ImgList[j].Uptime= "display:none;";}
      }
            }
            }
            else{
                $scope.tips="没有数据";
            }
            });
}
    crowClick();
    $scope.crowClickByState=crowClick;
    // 循环切换每一栏图片
    function refrech() {
        for (var num = 0; num < len ; num++) {
          //  alert($scope.crowdlist.length);
            //循环获取每一栏内的图片类名，然后隐藏该图片，显示下一图片
            for (var pnum = 0; pnum < $scope.crowdlist[num].ImgList.length; pnum++) {

                if ($scope.crowdlist[num].ImgList[pnum].Uptime=="display:block;") {
                    $scope.crowdlist[num].ImgList[pnum].Uptime="display:none;";
                    //document.getElementById("idIma/" + h + pnum).style.backgroundColor = "#FDF0E8";
                    pnum++;//下一个图片
                    if ($scope.crowdlist[num].ImgList.length==pnum)//循环到最后一个图片，回到第一个
                    {
                        $scope.crowdlist[num].ImgList[0].Uptime="display:block;"
                        //document.getElementById("idIma/" + h + '1').style.backgroundColor = "#7c89e2";
                        break;
                    }
                    else {

                        $scope.crowdlist[num].ImgList[pnum].Uptime="display:block;"
                        // document.getElementById("idIma/" + h + pnum).style.backgroundColor = "#7c89e2";
                        break;
                    }

                }
            }

        }
        $timeout(function(){
            refrech();
        },3000);//3秒执行轮播

    };

refrech();

    $scope.crowdadd={
        Adduser:2,
        Upuser:2,
        Ttitle:"添加标题",
        Intro:"添加简介",
        Starttime:'2015-06-20',
        Endtime:'2015-06-20',
        Status:3
    }

    $scope.info={
        Openid:'sadsadas',
        Nickname:'abc',
        Sex:'s',
        City:'sadsad',
        Country:'asdsad',
        Private:'asdasd',
        Language:'asdsad',
        Headimgurl:'asdsad',
        Subscribetime:'2015-06-20',
        Unioid:'asd',
        Remark:'asdsad',
        Groupid:'asdsad',
        Adduser:2,
        Addtime:'2015-06-20',
        Upuser:2,
        Uptime:'2015-06-20',
        Phone:15911330000
    }
//    $http.post('http://localhost:16857/API/CrowdApi/AddFollower',$scope.info)
//        .success(function (response) {
//
//        });
});