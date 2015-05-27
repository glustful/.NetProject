using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class CreateMessageConfigModel
    {
        /// <summary>
        /// 短信配置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 短信配置模版
        /// </summary>
        public string Template { get; set; }
    }
}