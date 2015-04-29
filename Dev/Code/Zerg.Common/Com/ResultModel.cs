using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Common
{
    /// <summary>
    /// 数据返回类
    /// </summary>
    public class ResultModel
    {
       /// <summary>
       /// 数据返回状态（true 成功，false  失败）
       /// </summary>
        public bool Status { get; set; }
       /// <summary>
       /// 返回状态说明
       /// </summary>
        public string Msg { get; set; }
    }
}