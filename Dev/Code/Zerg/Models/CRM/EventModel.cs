using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class EventModel
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// 活动内容
        /// </summary>
        public virtual string EventContent { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public virtual DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndTime { get; set; }
        /// <summary>
        /// 所属控制器
        /// </summary>
        public virtual string ActionControllers { get; set; }
        /// <summary>
        /// 活动状态  0关闭  1开启
        /// </summary>
        public virtual bool State { get; set; }
    }
}