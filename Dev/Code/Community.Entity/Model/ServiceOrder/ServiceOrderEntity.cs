using System;
using System.Collections.Generic;
using Community.Entity.Model.Member;
using Community.Entity.Model.MemberAddress;
using Community.Entity.Model.Order;
using Community.Entity.Model.ServiceOrderDetail;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ServiceOrder
{
	public class ServiceOrderEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 订单号
		/// </summary>
		public virtual string OrderNo { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public virtual DateTime AddTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public virtual int UpdUser { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdTime { get; set; }

		/// <summary>
		/// 添加人
		/// </summary>
		public virtual int AddUser { get; set; }
		/// <summary>
		/// 费用
		/// </summary>
		public virtual decimal Flee { get; set; }
//		/// <summary>
//		/// 地址
//		/// </summary>
//		public virtual string Address { get; set; }
		/// <summary>
		/// 服务时间
		/// </summary>
		public virtual DateTime Servicetime { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public virtual string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual EnumOrderStatus Status { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public virtual List<ServiceOrderDetailEntity> Details { get; set; }

        /// <summary>
        /// 下单人实体
        /// </summary>
        public virtual MemberEntity AddMember { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual MemberAddressEntity Address { get; set; }
	}
}