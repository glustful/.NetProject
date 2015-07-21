using System;
using System.Collections.Generic;
using YooPoon.Core.Data;
//by yangyue   2015/7/20 -------活动表实体//
namespace CRM.Entity.Model
{
    public class EventEntity : IBaseEntity
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 活动内容
        /// </summary>
        public virtual string EventContent{ get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public virtual DateTime Starttime { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        public virtual DateTime Endtime { get; set; }

        /// <summary>
        /// 作用的Controllers
        /// </summary>
        public virtual string ActionControllers { get; set; }

        /// <summary>
        /// 正在使用   结束
        /// </summary>
        public virtual bool State { get; set; }

    }
}
