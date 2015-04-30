/**
 * Created by Yunjoy_Dym on 2015/4/13.
 */
$(function () {
    $("#username").blur(function() {
        if ($("#username").val() == "") {
            $("#massage").html("请输入帐号");
            $("#username").focus();
            return false;
        }
    });
    $("#password").blur(function() {
    if ($("#password").val() == "") {
    $("#massage").html("请输入密码");
    $("#password").focus();
    return false;
    }
    });
    $("#verification").blur(function() {
    if ($("#verification").val() == "") {
    $("#massage").html("请输入验证码");
    $("#verification").focus();
    return false;
    }
    });
    });
