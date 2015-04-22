using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BankCardEntity : IBaseEntity
	{
		/// <summary>
		/// 银行卡ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// Ban_银行ID
		/// </summary>
		public virtual BankEntity Bank { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 卡号
		/// </summary>
		public virtual int Num { get; set; }
		/// <summary>
		/// 卡种
		/// </summary>
		public virtual bool Type { get; set; }
		/// <summary>
		/// 到期时间
		/// </summary>
		public virtual DateTime Deadline { get; set; }
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