using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace Zerg.Common
{

    /// <summary>
    ///页面处理帮助类
    /// </summary>
    public class PageHelper
    {


        /// <summary>
        /// 将返回数据转换成Json格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        /// <summary>
        /// 返回数据封装
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResultModel  ReturnValue(bool status,string msg)
        {
            return new ResultModel { Status=status,Msg=msg };
        }

        /// <summary>
        /// 验证输入是否是数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool ValidateNumber(string number)
        {  
           int num;
           return Int32.TryParse(number, out num);             
        }

     

    }
}
