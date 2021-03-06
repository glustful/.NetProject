using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BrokeAccountEntity : IBaseEntity
	{
		/// <summary>
		/// 账户ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }

        /// <summary>
        /// 金额变动描述
        /// </summary>
        public virtual string MoneyDesc { get; set; }

		/// <summary>
		/// 新增金额
		/// </summary>
		public virtual decimal Balancenum { get; set; }


        /// <summary>
        /// 金额类型（0 带客，1 推荐,2 奖励完整信息钱）
        /// </summary>
        public virtual int Type { get; set; }
        /// <summary>
        /// 状态（0 可用，1 已使用,-1提现中）
        /// </summary>
        public virtual int State { get; set; }

		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpUser
		/// </summary>
		public virtual int Upuser { get; set; }
		/// <summary>
		/// UpTime
		/// </summary>
		public virtual DateTime Uptime { get; set; }

      

        
	}
}