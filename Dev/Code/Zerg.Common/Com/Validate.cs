﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YooPoon.Common.Encryption;
namespace Zerg.Common.Com
{
 public static  class ValidateMessage
 {
     #region 黄秀宇 2015.06.05
   
     /// <summary>
     /// 验证六位短信验证码
     /// </summary>
        /// <param name="sourc">加密后的字符串</param>
        /// <param name="messa">验证码</param>
        /// <param name="userid">用户密钥,DES加密用</param>
     /// <returns></returns>
     public static HttpResponseMessage validate(string sourc, string messa, string salt)
        {
            if ((!string.IsNullOrEmpty(sourc)) && (!string.IsNullOrEmpty(messa)))
            {

                string sou = EncrypHelper.Decrypt(sourc, salt);//解密
             string[] str = sou.Split('$');
             string source = str[0];//获取验证码
             DateTime  date =Convert .ToDateTime (str[1]);//获取发送验证码的时间
             DateTime dateNow =Convert .ToDateTime ( DateTime.Now.ToLongTimeString());//获取当前时间
             TimeSpan ts = dateNow.Subtract(date);
             double  secMinu =  ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
                if (secMinu >30)
                {
                   // SMSHelper.vali.Remove(userid + messa);
                    return PageHelper.toJson(PageHelper.ReturnValue(false , "你已超过时间验证，请重新发送验证码！"));
                }
                else if(messa ==source){
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "成功验证！"));
                }
                else 
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "验证失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "验证失败！"));
        }
     /// <summary>
     /// 发送短信验证码
     /// </summary>
     /// <param name="phone">手机号码</param>
     /// <param name="salt">用户密钥，DES加密用</param>
     /// 
     /// <returns></returns>
     public static  HttpResponseMessage SendMessage6( string phone, string salt)
     {
         string messa = new Random().Next(100000, 1000000).ToString();//生成大于等于100000，小于等于999999的随机数，也就是六位随机数
         if (!string.IsNullOrEmpty(phone))
         {
             var p = SMSHelper.Sending(phone, messa);//给用户发送验证码
             string time = DateTime.Now.ToLongTimeString();
             var source = EncrypHelper.Encrypt(messa + "$" + time, salt);//EMS 加密短信验证码
             return PageHelper.toJson(new { sou = source, messP = p });
         }
         return PageHelper.toJson(0);
     }
     #endregion
 }
}
