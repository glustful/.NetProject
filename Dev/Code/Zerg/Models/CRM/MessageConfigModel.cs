using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class MessageConfigModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string  Id { get; set; }
        /// <summary>
        /// 短信配置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 短信配置模版
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public  int Adduser { get; set; }
        /// <summary>
        /// UpTime
        /// </summary>
        public  DateTime Uptime { get; set; }
        /// <summary>
        /// UpUser
        /// </summary>
        public  int Upuser { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageCount { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool isDescending { get; set; }
    }
}