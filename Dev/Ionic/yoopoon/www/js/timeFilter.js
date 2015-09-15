/**
 * Created by Administrator on 2015/9/10.
 */
//Ê±¼ä¹ýÂËÆ÷
app.filter('dateFilter',function(){
    return function(date){
        if(!date)
            return "";
        var jsondate = date.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        }
        else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var newDate = new Date(parseInt(jsondate, 10));
        var month = newDate.getMonth() + 1 < 10 ? "0" + (newDate.getMonth() + 1) : newDate.getMonth() + 1;
        var currentDate = newDate.getDate() < 10 ? "0" + newDate.getDate() : newDate.getDate();

        return newDate.getFullYear()
            + "-"
            + month
            + "-"
            + currentDate
            + "  "
            + newDate.getHours()
            + ":"
            + newDate.getMinutes()
            + ":"
            + newDate.getSeconds()
            ;
    }
});
