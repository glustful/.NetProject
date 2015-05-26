using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class OrderEntity : IBaseEntity
	{
		/// <summary>
		/// 订单ID
		/// </summary>
        /// 	
        public virtual int Id { get; set; }
		public virtual OrderDetailEntity OrderDetail { get; set; }
		/// <summary>
		/// 订单编码
		/// </summary>
		public virtual string Ordercode { get; set; }
		/// <summary>
		/// 订单类型
		/// </summary>
		public virtual int Ordertype { get; set; }
		/// <summary>
		/// 流程状态
		/// </summary>
		public virtual int Shipstatus { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public virtual int Status { get; set; }
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual int BusId { get; set; }
		/// <summary>
		/// 商家名称
		/// </summary>
		public virtual string Busname { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual int AgentId { get; set; }
		/// <summary>
		/// 经纪人名称
		/// </summary>
		public virtual string Agentname { get; set; }
		/// <summary>
		/// 经纪人tel
		/// </summary>
		public virtual string Agenttel { get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public virtual string Customname { get; set; }
		/// <summary>
		/// 订单备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 开单时间
		/// </summary>
		public virtual DateTime Adddate { get; set; }
		/// <summary>
		/// 开单人员
		/// </summary>
		public virtual string Adduser { get; set; }
		/// <summary>
		/// 修改人员
		/// </summary>
		public virtual string Upduser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime Upddate { get; set; }
	}
}