using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class AgentBillEntity : IBaseEntity
	{
		/// <summary>
		/// 账单ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 订单ID
		/// </summary>
		public virtual OrderEntity Order { get; set; }
        /// <summary>
        /// 活动订单表
        /// </summary>
	    public virtual int? EventOrderId { get; set; }
	    /// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual int AgentId { get; set; }
		/// <summary>
		/// 经纪人昵称
		/// </summary>
		public virtual string Agentname { get; set; }
		/// <summary>
		/// 地产商ID
		/// </summary>
		public virtual int LandagentId { get; set; }
		/// <summary>
		/// 地产商名字
		/// </summary>
		public virtual string Landagentname { get; set; }
		/// <summary>
		/// 账单金额
		/// </summary>
		public virtual decimal  Amount { get; set; }
		/// <summary>
		/// 实收金额
		/// </summary>
		public virtual decimal? Actualamount { get; set; }
		/// <summary>
		/// 卡号
		/// </summary>
		public virtual string Cardnumber { get; set; }
		/// <summary>
		/// 是否出具发票
		/// </summary>
		public virtual bool Isinvoice { get; set; }
		/// <summary>
		/// 结账备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 收款人
		/// </summary>
		public virtual string Beneficiary { get; set; }
		/// <summary>
		/// 收款账号
		/// </summary>
		public virtual string Beneficiarynumber { get; set; }
		/// <summary>
		/// 付款日期
		/// </summary>
		public virtual DateTime Checkoutdate { get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public virtual string Customname { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual string Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual string Upduser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime Updtime { get; set; }
	}
}