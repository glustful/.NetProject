using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BrokerOrderEntity : IBaseEntity
	{
		/// <summary>
		/// 订单关联表ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 订单ID
		/// </summary>
		public virtual int OrderId { get; set; }
		/// <summary>
		/// 商家名称
		/// </summary>
		public virtual string Merchantname { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public virtual bool Status { get; set; }
		/// <summary>
		/// 订单备注
		/// </summary>
		public virtual string Note { get; set; }
		/// <summary>
		/// 开单时间
		/// </summary>
		public virtual DateTime Ordertime { get; set; }
		/// <summary>
		/// 开单人员
		/// </summary>
		public virtual string Orderuser { get; set; }
		/// <summary>
		/// 修改人员
		/// </summary>
		public virtual string Modifyuser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime Modifytime { get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public virtual string Customername { get; set; }
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