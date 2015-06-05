using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zerg.Common
{

    /// <summary>
    /// 短信发送帮助类
    /// </summary>
    public  class SMSHelper
    {
       public static  string  radValue = new Random().Next(100000, 1000000).ToString ();//生成大于等于100000，小于等于999999的随机数，也就是六位随机数；
      // public   static List<string> vali = new List<string>();//定义一个列表，用于保存短信发送的验证码
        #region 中国网建 SMS短信发送平台

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="contexts">内容</param>
        /// <returns></returns>
        public static string Sending(string mobile, string contexts)
        {
            string smsAddress = "http://utf8.sms.webchinese.cn/?Uid=" + "yunjoy_1009" + "&Key=" + "a5a1ad32761a3a2f1ec2" + "&smsMob=" + mobile + "&smsText=" + contexts;
            return ReturnMessage( GetHtmlFromUrl(smsAddress));
        }



        /// <summary>
        /// 获取短信剩余数量
        /// </summary>
        /// <returns></returns>
        public static string GetSMSCount()
        {
            string smsAddress = "http://sms.webchinese.cn/web_api/SMS/?Action=SMS_Num&Uid=" + "yunjoy_1009" + "&Key=" + "a5a1ad32761a3a2f1ec2";
            return GetHtmlFromUrl(smsAddress);
        }


        /// <summary>
        /// 调用接口
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }

        /// <summary>
        /// 返回提示消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ReturnMessage(string message)
        {
            string returnValue = string.Empty;
            switch(message)
            {
                case "-1":
                returnValue= "没有该用户账户";
                break;

                case "-2":
                returnValue= "接口密钥不正确,不是账户登陆密码";
                break;

                case "-21":
                returnValue= "MD5接口密钥加密不正确";
                break;

                case "-3":
                returnValue= "短信数量不足";
                break;

                case "-11":
                returnValue= "该用户被禁用";
                break;

                case "-14":
                returnValue= "短信内容出现非法字符";
                break;

                case "-4":
                returnValue= "手机号格式不正确";
                break;

                case "-41":
                returnValue= "手机号码为空";
                break;

                case "-42":
                returnValue= "短信内容为空";
                break;

                case "-51":
                returnValue = "短信签名格式不正确 接口签名格式为：【签名内容】";
                break;

                case "-6":
                returnValue = "IP限制";
                break;

                //大于0	短信发送数量
                default:
                returnValue = message;
                break;
            }
            return returnValue;
        }

        #endregion

    }
}
