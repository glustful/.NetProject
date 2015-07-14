/**
 * Created by 黄秀宇 on 2015/7/10.
 */

//-------------------------众筹主页 start------------------------------
angular.module("app").controller('chipController', [
    '$http','$scope','$modal',function($http,$scope,$modal) {
        $scope.img=SETTING.ImgUrl;
        $scope.searchCondition = {
            page: 1,
            pageSize: 10
        };
        $scope.list=[];
        //获取众筹列表
        $scope.getList  = function() {
            var state=-1;
            $http.get(SETTING.eventApiUrl+'/CrowdApi/GetCrowdInfoPC?status='+state, {'withCredentials': true})
                .success(function(data) {
                    if(data.list.length>0) {
                        $scope.list = data.list;
                        $scope.tips="";
                    }
                    else{
                        $scope.tips="没有数据";
                    }
            });
        };
        $scope.getList();

        //---------------------------------------------删除众筹----------------------------------------------//
        $scope.Delete=function(index,id){
            $scope.index=index;
            $scope.id=id;
            var modalInstance=$modal.open({
                templateUrl:'myModalContent.html',
                controller:'ModalInstanceCtrl',
                resolve:{
                    msg:function(){
                        return "你确定要删除吗？";
                    }
                }
            });

            modalInstance.result.then(function () {
                $http.post(SETTING.eventApiUrl + '/CrowdApi/DeleteCrowdInfo?id='+$scope.id,{
                    'withCredentials':true
                }).success(function(data){
                    if(data.Status){
                        $scope.list.splice(index,1);
                        $scope.alerts=[{type:'danger',msg:data.Msg}];
                    }else{
                        $scope.alerts=[{type:'danger',msg:data.Msg}];

                    }
                });
            });
        };
        $scope.closeAlert = function(Brand) {
            $scope.alerts.splice(Brand, 1);
        };
    }

]);

//----------------------------------------新增众筹----------------------------------------------//
angular.module("app").controller('creatChipController', [
    '$http', '$scope', '$state','FileUploader', function ($http, $scope, $state,FileUploader) {

        //--------添加项目 start---------//
        $scope.crowdModel={
            Ttitle:"",
            ImgList1:new Array(),
            SubTitle:"",
            Content:"",
            ClassId:"",
            crowdUrl:""

        };

        $scope.Save = function(){
            document.getElementById("btnok").setAttribute("disabled", true);
            $scope.crowdModel.ImgList1=$scope.image;
            console.log( $scope.crowdModel);
           $http.post(SETTING.eventApiUrl + '/CrowdApi/AddCrowdInfo',$scope.crowdModel,{
                    'withCredentials':true
            }).success(function(data){
               console.log(data);
               //alert(data.Status);
                if(data.Status){
                    document.getElementById("btnok").removeAttribute("disabled");
                    $scope.alerts=[{type:'danger',msg:data.Msg}];

                }else{
                    document.getElementById("btnok").removeAttribute("disabled");
                    $scope.alerts=[{type:'danger',msg:data.Msg}];


                }
            });
        }
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
            $scope.crowdModel.Bname=''
        };


        //---------------------------------------------图片上传 start------------------------------------//
        $scope.image=[];//保存图片名称
        $scope.SImg=SETTING.ImgUrl;//图片服务器基础路径
        function completeHandler(e) {
           // $scope.showImage=$scope.showImage+'<img height="100" width="100" src="'+SETTING.ImgUrl+e+'" title="图片" />';
            //$scope.showImage=$scope.showImage+"<img height='100' width='100' src='"+SETTING.ImgUrl+e+"' title='图片' />";

            $scope.image.push(e);
               }

        function errorHandler(e) {
            // console.log(e);
        }

        var uploader = $scope.uploader = new FileUploader({
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        });
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            completeHandler(response.Msg);

        };

        //-------------------------------------------图片上传 end------------------------------------------//



    }




]);

//----------------------------------------修改众筹----------------------------------------------//
angular.module("app").controller('upChipController', [
    '$http', '$scope', '$state','FileUploader','$stateParams', function ($http, $scope, $state,FileUploader,$stateParams) {

        //--------添加项目 start---------//
        $scope.crowdModel={
            Ttitle:"",
            ImgList1:new Array(),
            SubTitle:"",
            Content:"",
            ClassId:"",
            crowdUrl:"",
            Id:""

        };
        $scope.image=[];//保存图片名称
        $http.get(SETTING.eventApiUrl+'/CrowdApi/GetCrowdByCrowdId?id='+$stateParams.crowId, {'withCredentials': true})
            .success(function(data) {
                $scope.list = data.list;
                $scope.crowdModel.Ttitle=data.list[0].Ttitle;
                $scope.crowdModel.crowdUrl=data.list[0].crowdUrl;
                $scope.crowdModel.Intro=data.list[0].Intro;
                for(var i=0;i< data.list[0].ImgList.length;i++){
                $scope.image.push(data.list[0].ImgList[i].Imgurl);

                }
                console.log($scope.image);
            });
        //删除图片
        $scope.del=function(index){

            $scope.image.splice(index,1);

            console.log($scope.image);
        }
        //保存

        $scope.Save = function(){
            document.getElementById("btnok").setAttribute("disabled", true);
           for(var i=0;i<$scope.image.length;i++){

           }
            $scope.crowdModel.ImgList1=$scope.image;
            $scope.crowdModel.Id=$stateParams.crowId;
            console.log( $scope.crowdModel);
            $http.post(SETTING.eventApiUrl + '/CrowdApi/AddCrowdInfo',$scope.crowdModel,{
                'withCredentials':true
            }).success(function(data){
                if(data.Status){
                    console.log(data.Msg);
                    document.getElementById("btnok").removeAttribute("disabled");
                    $scope.alerts=[{type:'danger',msg:data.Msg}];
                    //$state.go('page.event.chip.chip');

                }else{
                    document.getElementById("btnok").removeAttribute("disabled");
                    $scope.alerts=[{type:'danger',msg:data.Msg}];


                }
            });
        }
        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
            $scope.crowdModel.Bname=''
        };


        //---------------------------------------------图片上传 start------------------------------------//

        $scope.SImg=SETTING.ImgUrl;
        //保存上传的图片
        function completeHandler(e) {
            $scope.image.push(e);
            console.log($scope.image);
        }

        function errorHandler(e) {
            // console.log(e);
        }

        var uploader = $scope.uploader = new FileUploader({
            url: SETTING.ApiUrl+'/Resource/Upload',
            'withCredentials':true
        });
        uploader.onSuccessItem = function(fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            completeHandler(response.Msg);
        };

        //-------------------------------------------图片上传 end------------------------------------------//



    }




]);