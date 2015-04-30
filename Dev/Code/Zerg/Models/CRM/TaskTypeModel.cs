using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class TaskTypeModel
    {
        /// <summary>
        /// 任务类型ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>
        public string Describe { get; set; }
    }
}