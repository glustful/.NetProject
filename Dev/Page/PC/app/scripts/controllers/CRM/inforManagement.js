/**
 * Created by 黄秀宇 on 2015/7/8.
 */
var keyname="";
key=function () {
    var keynum;
//三元表达式获取不同浏览器的按下键的代码
    keynum = window.event ? event.keyCode : event.which;
    if(keynum==13){btsearch.click()}if(keynum==39){
        if(keyname=="name"){document.getElementById("phone").focus();}
        else  if(keyname=="phone"){document.getElementById("staStatus").focus();}
    }
    if(keynum==37){

        if(keyname=="staStatus"){
            //使HTML元素原来默认的事件失效
            event.returnValue=false;
            document.getElementById("phone").focus();
        }
        else  if(keyname=="phone"){document.getElementById("name").focus();}
    }

}
//保存获得焦点的id
myFunction=function(idif){
    keyname=idif;
}
