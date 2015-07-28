using System;
using System.Collections.Generic;
using YooPoon.Core.Data;
//by yangyue   2015/7/20 -------邀请码表实体//
namespace CRM.Entity.Model
{
    public class InviteCodeEntity : IBaseEntity
    {

        /// <summary>
        /// 邀请码ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 拥有者ID
        /// </summary>
        public virtual BrokerEntity Broker { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public virtual string Number { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatTime { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public virtual DateTime UseTime { get; set; }
        /// <summary>
        /// 使用者Id
        /// </summary>
        public virtual int? NumUser { get; set; }
        /// <summary>
        /// 使用状态 1是被使用  0是未使用
        /// </summary>
        public virtual int? State { get; set; }


    }
}
