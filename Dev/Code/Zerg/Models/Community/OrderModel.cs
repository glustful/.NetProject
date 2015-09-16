using System;
using System.Collections.Generic;
using Community.Entity.Model.Order;

namespace Zerg.Models.Community
{
	public class OrderModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 订单编码
        /// </summary>
		public string No {get;set;}


		/// <summary>
        /// 状态
        /// </summary>
		public EnumOrderStatus Status {get;set;}

		public string StatusString
		{
			get
			{
				switch(Status)
				{

					case EnumOrderStatus.Created:
						return "新建";

					case EnumOrderStatus.Payed:
						return "已付款";

					case EnumOrderStatus.Delivering:
						return "配送中";

					case EnumOrderStatus.Successed:
						return "订单完成";

					case EnumOrderStatus.Canceled:
						return "订单关闭";

					default:
						return "";
				}
			}
		}


		/// <summary>
        /// 客户名称
        /// </summary>
		public string CustomerName {get;set;}


		/// <summary>
        /// 订单备注
        /// </summary>
		public string Remark {get;set;}


		/// <summary>
        /// 开单时间
        /// </summary>
		public DateTime Adddate {get;set;}


		/// <summary>
        /// 开单人员
        /// </summary>
		public int Adduser {get;set;}


		/// <summary>
        /// 修改人员
        /// </summary>
		public int Upduser {get;set;}


		/// <summary>
        /// 修改时间
        /// </summary>
		public DateTime Upddate {get;set;}


		/// <summary>
        /// 订单总价
        /// </summary>
		public decimal Totalprice {get;set;}


		/// <summary>
        /// 订单实价
        /// </summary>
		public decimal Actualprice {get;set;}


		/// <summary>
        /// 订单明细
        /// </summary>
		public List<OrderDetailModel> Details {get;set;}


        public string UserName { get; set; }

        public int MemberAddressId { get; set; }
	}
}