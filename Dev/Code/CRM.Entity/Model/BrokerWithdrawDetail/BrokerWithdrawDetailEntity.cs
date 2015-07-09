using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
    /// <summary>
    /// 提现附表
    /// </summary>
	public class BrokerWithdrawDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 提现ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 银行卡ID
		/// </summary>
		public virtual BankCardEntity BankCard { get; set; }
        /// <summary>
        /// 经纪人提现 ID
        /// </summary>
        public virtual BrokerWithdrawEntity BrokerWithdraw { get; set; }

		/// <summary>
		/// 提现时间
		/// </summary>
		public virtual DateTime Withdrawtime { get; set; }
		/// <summary>
		/// 提现金额
		/// </summary>
		public virtual decimal Withdrawnum { get; set; }
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

        /// <summary>
        /// 类型（0带客，1推
        /// </summary>
        public virtual string Type { get; set; }
	}
}