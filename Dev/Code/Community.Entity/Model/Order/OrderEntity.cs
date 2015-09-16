using System;
using System.Collections.Generic;
using Community.Entity.Model.Member;
using Community.Entity.Model.MemberAddress;
using Community.Entity.Model.OrderDetail;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Order
{
	public class OrderEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 订单编码
		/// </summary>
		public virtual string No { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public virtual EnumOrderStatus Status { get; set; }
		/// <summary>
		/// 客户名称
		/// </summary>
		public virtual string CustomerName { get; set; }
		/// <summary>
		/// 订单备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 开单时间
		/// </summary>
		public virtual DateTime AddDate { get; set; }
		/// <summary>
		/// 开单人员
		/// </summary>
		public virtual int AddUser { get; set; }
		/// <summary>
		/// 修改人员
		/// </summary>
		public virtual int UpdUser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime UpdDate { get; set; }
		/// <summary>
		/// 订单总价
		/// </summary>
		public virtual decimal Totalprice { get; set; }
		/// <summary>
		/// 订单实价
		/// </summary>
		public virtual decimal Actualprice { get; set; }
		/// <summary>
		/// 订单明细
		/// </summary>
		public virtual List<OrderDetailEntity> Details { get; set; }

        /// <summary>
        /// 下单人实体
        /// </summary>
        public virtual MemberEntity AddMember { get; set; }

        /// <summary>
        /// 收获地址
        /// </summary>
        public virtual MemberAddressEntity Address { get; set; }
	}
}