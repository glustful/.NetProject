/**
 * Created by 黄秀宇 on 2015/5/13.
 * 自定义json日期格式转换过滤器toDate
 */
angular.module('app').filter('toDate',function(){
    return function(jsonDate){
        //获取毫秒数
        var oldtime= jsonDate.replace(/[^0-9]/ig,"");

       return oldtime;
       /* //转成日期
>>>>>>> 4af4dcba0a34a626067e81c22df3a71fdae1d511
        var newtime = new Date();
        newtime.setTime (oldtime);
        //拼凑日期
        var nntime=newtime.getFullYear()+'-'+(newtime.getMonth()+ 1)+'-'+newtime.getDate()
            +' '+newtime.getHours ()+':'+newtime.getMinutes ();
<<<<<<< HEAD
        return nntime;
=======
        return nntime;*/

    }
});
