using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//任务目标数据模型
namespace Zerg.Models.CRM
{
    public class TaskTagModel
    {
        /// <summary>
        /// 任务目标ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 目标名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 目标描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 目标值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        public string Type { get; set; }
    }
}