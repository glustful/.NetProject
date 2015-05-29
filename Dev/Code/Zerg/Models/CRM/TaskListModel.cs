using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class TaskListModel
    {
        /// <summary>
        /// 任务列表ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 任务表ID
        /// </summary>
        public virtual int TaskId { get; set; }
        /// <summary>
        /// 经纪人ID
        /// </summary>
        public virtual int  BrokerId { get; set; }
        /// <summary>
        /// 任务进度
        /// </summary>
        public string Taskschedule { get; set; }
        /// <summary>
        /// 增加或删除
        /// </summary>
        public string Type { get; set; }
    }
}