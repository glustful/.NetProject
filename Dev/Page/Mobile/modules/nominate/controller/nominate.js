/**
 * Created by Administrator on 2015/5/29.
 */
app.controller('StormRoomController',['$http','$scope','$timeout',function($http,$scope,$timeout){

    /*----------------------------------------------��̬����-------------------------------------------*/
    $scope.searchCondition={
        AreaName:'',
        TypeId:'',
        PriceBegin:'',
        PriceEnd:'',
        Page:0,
        PageCount:10,
        OrderBy:'OrderByAddtime',
        IsDescending:true
    };
    $scope.tipp="���ڼ���......";
    var loading = false
        ,pages=2;                      //�ж��Ƿ����ڶ�ȡ���ݵı���
    $scope.List = [];//����ӷ��������������񣬿��ۼ�
    var pushContent= function() {                    //�����������������$scope.posts
        //�������
//        $scope.List=[];
//        $scope.searchCondition.Page=0;
        // pages=2;
        if (!loading && $scope.searchCondition.Page < pages) {                         //���ҳ��û�����ڶ�ȡ
            loading = true;                     //��֪���ڶ�ȡ
            $http.get(SETTING.ApiUrl+'/Product/GetSearchProduct',{params:$scope.searchCondition,'withCredentials':true}).success(function(data) {
                pages =Math.ceil(data.TotalCount /$scope.searchCondition.PageCount);
                for (var i = 0; i <= data.List.length - 1; i++) {
                    $scope.List.push(data.List[i]);
                }
                loading = false;            //��֪��ȡ����
                $scope.tipp="���ظ���......";
                if ($scope.List.length == data.TotalCount) {//������������Ѳ����
                    $scope.tipp = "�Ѿ������һҳ��";
                }
                $scope.Count=data.TotalCount;
            });
            $scope.searchCondition.Page++;                             //��ҳ
        }
//        else {
//            $scope.tipp = "�Ѿ������һҳ��";
//        }
    };
    pushContent();
    //$scope.more=pushContent;
    function pushContentMore(){

        if ($(document).scrollTop()+5 >= $(document).height() - $(window).height())
        {
            pushContent();//if�ж���û�л������ײ������˼���
        }
        $timeout(pushContentMore, 2500);//��ʱ����ÿ��һ��ѭ������������
    }
    pushContentMore();//��������Ķ�ʱ��
    /*----------------------------------------------��̬����-------------------------------------------*/

    $scope.type = true;
    $scope.province=true;
    $scope.city=true;
    $scope.county=true;
    $scope.price=true;
    $scope.selectCity=false;
    $scope.selectCounty=false;
    $scope.selectArea='����';
    $scope.selectType='����';
    $scope.selectPrice='�۸�';
    //��������
    $scope.Reset=function(){
        $scope.searchCondition.AreaName='';
        $scope.searchCondition.TypeId='';
        $scope.searchCondition.PriceBegin='';
        $scope.searchCondition.PriceEnd='';
        $scope.List=[];
        $scope.searchCondition.Page=0;
        $scope.selectArea='����';
        $scope.selectType='����';
        $scope.selectPrice='�۸�';
        $scope.tipp="���ڼ���......";
        pages=2;
        pushContent();
    }
    //��ȡ��������
    $scope.getTypeCondition= function(value,typeName){
        $scope.searchCondition.TypeId = value;
        $scope.selectType=typeName;
        if($scope.type==false)
        {
            $scope.type = !$scope.type;
        }
        //$scope.searchProduct();
        $scope.List=[];
        $scope.searchCondition.Page=0;
        $scope.tipp="���ڼ���......";
        pages=2;
        pushContent();
    }
    //��ȡ��������
    $scope.getAreaCondition= function(value){
        $scope.searchCondition.AreaName = value;
        $scope.selectArea=value;
        if($scope.province==false)
        {
            $scope.province = !$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city = !$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county = !$scope.county;
        }
        //$scope.searchProduct();
        $scope.tipp="���ڼ���......";
        $scope.List=[];
        $scope.searchCondition.Page=0;
        pages=2;
        pushContent();
    }
    //��ȡ�۸�����
    $scope.getPriceCondition= function(priceBegin,priceEnd){
        $scope.searchCondition.PriceBegin =priceBegin;
        $scope.searchCondition.PriceEnd=priceEnd;
        if(priceBegin==0)
        {
            $scope.selectPrice='4000����';
        }
        else if(priceBegin==10000)
        {
            $scope.selectPrice='10000����'
        }
        else{
            $scope.selectPrice=priceBegin+'-'+priceEnd;
        }

        if($scope.price==false)
        {
            $scope.price = !$scope.price;
        }
        //$scope.searchProduct();
        $scope.tipp="���ڼ���......";
        $scope.List=[];
        $scope.searchCondition.Page=0;
        pages=2;
        pushContent();
    }
    //��ȡʡ�ݺͻ���
    $http.get(SETTING.ApiUrl + '/Condition/GetCondition',{'withCredentials':true}).success(function(data){
        $scope.Area =data.AreaList;
        $scope.Type=data.TypeList;
    });
    //��ȡʡ��Ӧ����
    $scope.getCityList=function(id,row){
        $scope.selectedRow = row;
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCity =data.AreaList;
        });
        if($scope.city) {
            $scope.city = !$scope.city;
        }
        $scope.selectCity=true;
        $scope.selectCounty=false;
        $scope.AreaCity=null;
        $scope.AreaCounty=null;
    };
    //��ȡ�ж�Ӧ������
    $scope.getCountyList=function(id,row){
        $scope.selectedRow1 = row;
        $scope.parentId=id;
        $http.get(SETTING.ApiUrl + '/Condition/GetCondition/',{params:{
            parentId:$scope.parentId
        },'withCredentials':true}).success(function(data){
            $scope.AreaCounty =data.AreaList;
        });
        if($scope.county) {
            $scope.county = !$scope.county;
        }
        $scope.selectCounty=true;
    };
//չ������
    $scope.toggleProvince=function(){
        $scope.province=!$scope.province;
        $scope.city=!$scope.city;
        $scope.county=!$scope.county;
        if($scope.price==false)
        {
            $scope.price=!$scope.price;
        }
        if($scope.type==false)
        {
            $scope.type=!$scope.type;
        }
    }
    //չ������
    $scope.toggleType = function() {
        $scope.type = !$scope.type;
        if($scope.price==false)
        {
            $scope.price=!$scope.price;
        }
        if($scope.province==false)
        {
            $scope.province=!$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city=!$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county=!$scope.county;
        }
    };
    //չ���۸�
    $scope.togglePrice=function(){
        $scope.price=!$scope.price;
        if($scope.type==false)
        {
            $scope.type=!$scope.type;
        }
        if($scope.province==false)
        {
            $scope.province=!$scope.province;
        }
        if($scope.city==false)
        {
            $scope.city=!$scope.city;
        }
        if($scope.county==false)
        {
            $scope.county=!$scope.county;
        }
    };
}])