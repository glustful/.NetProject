using System;
using System.Collections.Generic;
using YooPoon.Core.Data;
//by yangyue   2015/7/20 -------活动订单表实体//
namespace CRM.Entity.Model
{
    public class EventOrderEntity : IBaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id { get; set; }


        /// <summary>
        /// 活动
        /// </summary>
        public virtual EventEntity Event { get; set; }
        /// <summary>
        /// 所属经纪人
        /// </summary>
        public virtual BrokerEntity Broker { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public virtual DateTime Addtime { get; set; }
        /// <summary>
        /// 内容明细
        /// </summary>
        public virtual string AcDetail { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal MoneyCount { get; set; }

    }
}
