using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zerg.Common
{

    /// <summary>
    /// 短信发送帮助类
    /// </summary>
    public class SMSHelper
    {


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
            return ReturnMessage(GetHtmlFromUrl(smsAddress));
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
            switch (message)
            {
                case "-1":
                    returnValue = "没有该用户账户";
                    break;

                case "-2":
                    returnValue = "接口密钥不正确,不是账户登陆密码";
                    break;

                case "-21":
                    returnValue = "MD5接口密钥加密不正确";
                    break;

                case "-3":
                    returnValue = "短信数量不足";
                    break;

                case "-11":
                    returnValue = "该用户被禁用";
                    break;

                case "-14":
                    returnValue = "短信内容出现非法字符";
                    break;

                case "-4":
                    returnValue = "手机号格式不正确";
                    break;

                case "-41":
                    returnValue = "手机号码为空";
                    break;

                case "-42":
                    returnValue = "短信内容为空";
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


        #region 长沙沃动通讯科技有限公司 SMS短信发送平台

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="contexts"></param>
        /// <returns></returns>
        //public static string Sending(string mobile, string contexts)
        //{
        //    GetSMSCounts();
        //    发送短信
        //    string param = "action=send&userid=95&account=ypkj&password=123456&content=" + contexts + "&mobile=" + mobile + "&sendtime=";
        //      param = param + "0";//不需要定时发送，也需要带上0            
        //    byte[] bs = Encoding.UTF8.GetBytes(param);
        //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://115.29.242.32:8888/sms.aspx");
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    req.ContentLength = bs.Length;

        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(bs, 0, bs.Length);
        //    }
        //    using (WebResponse wr = req.GetResponse())
        //    {
        //        StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
        //         return sr.ReadToEnd().Trim();
        //        returnsms returns = XmlSerialize.DeserializeXML<returnsms>(sr.ReadToEnd().Trim());

        //        if (returns.returnstatus == "Success" && returns.successCounts == "1")
        //        {
        //            return "1";
        //        }
        //        return "短信发送失败";
        //    }
        //    return "";
        //}

        /// <summary>
        /// 获取短信剩余量
        /// </summary>
        /// <returns></returns>
        //public static string GetSMSCounts()
        //{
        //    //查询余额
        //    string param = "userid=95&account=ypkj&password=123456&action=overage";
        //    byte[] bs = Encoding.UTF8.GetBytes(param);

        //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://115.29.242.32:8888/sms.aspx");
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    req.ContentLength = bs.Length;

        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(bs, 0, bs.Length);
        //    }
        //    using (WebResponse wr = req.GetResponse())
        //    {
        //        StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
        //        string str = sr.ReadToEnd().Trim();
        //        returnsms returns = XmlSerialize.DeserializeXML<returnsms>(str);




        //        return "短信发送失败";
        //    }
        //    return "";
        //}

        #endregion
    }


    /// <summary>
    /// XML Help
    /// </summary>
    public class XmlSerialize
    {
        /// <summary>  
        /// 反序列化XML为类实例  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="xmlObj"></param>  
        /// <returns></returns>  
        public static T DeserializeXML<T>(string xmlObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlObj))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>  
        /// 序列化类实例为XML  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string SerializeXML<T>(T obj)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize((TextWriter)writer, obj);
                return writer.ToString();
            }
        }
    }

    /// <summary>
    /// 短信接口 返回值 类
    /// </summary>
    [Serializable]
    public class returnsms
    {
        /// <summary>
        /// 返回状态值：成功返回Success 失败返回：Faild
        /// </summary>
        public string returnstatus { get; set; }

        /// <summary>
        /// 返回信息（）
        /// </summary>
        public string message { get; set; }





        /// <summary>
        /// 返回余额
        /// </summary>
        public string remainpoint { get; set; }

        /// <summary>
        /// 返回本次任务的序列ID
        /// </summary>
        public string taskID { get; set; }

        /// <summary>
        /// 成功短信数：当成功后返回提交成功短信
        /// </summary>
        public string successCounts { get; set; }




        /// <summary>
        /// 返回支付方式  后付费，预付费
        /// </summary>
        public string payinfo { get; set; }

        /// <summary>
        /// 返回余额
        /// </summary>
        public string overage { get; set; }

        /// <summary>
        /// 返回总点数  当支付方式为预付费是返回总充值点数
        /// </summary>
        public string sendTotal { get; set; }

    }


}
