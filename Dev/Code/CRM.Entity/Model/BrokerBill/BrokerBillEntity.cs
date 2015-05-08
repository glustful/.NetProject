using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BrokerBillEntity : IBaseEntity
	{
		/// <summary>
		/// 账单关联表ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 账单ID
		/// </summary>
		public virtual int BillId { get; set; }
		/// <summary>
		/// 结账类型
		/// </summary>
		public virtual bool Type { get; set; }
		/// <summary>
		/// 账单金额
		/// </summary>
		public virtual decimal Billamount { get; set; }
		/// <summary>
		/// 实收金额
		/// </summary>
		public virtual decimal Paidinamount { get; set; }
		/// <summary>
		/// 卡号
		/// </summary>
		public virtual string Cardnum { get; set; }
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual int Merchantid { get; set; }
		/// <summary>
		/// 商家名字
		/// </summary>
		public virtual string Merchantname { get; set; }
		/// <summary>
		/// 收款人
		/// </summary>
		public virtual string Payeeuser { get; set; }
		/// <summary>
		/// 收款账号
		/// </summary>
		public virtual string Payeenum { get; set; }
		/// <summary>
		/// 付款日期
		/// </summary>
		public virtual DateTime Paytime { get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public virtual string Customername { get; set; }
		/// <summary>
		/// 账单备注
		/// </summary>
		public virtual string Note { get; set; }
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