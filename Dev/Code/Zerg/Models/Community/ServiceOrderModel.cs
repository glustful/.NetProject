using System;
using System.Collections.Generic;
using Community.Entity.Model.ServiceOrder;

namespace Zerg.Models.Community
{
	public class ServiceOrderModel
	{

		/// <summary>
        /// Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 订单号
        /// </summary>
		public string OrderNo {get;set;}


		/// <summary>
        /// 添加时间
        /// </summary>
		public DateTime Addtime {get;set;}


		/// <summary>
        /// 添加人
        /// </summary>
		public int AddUser {get;set;}

        public int UpdUser { get; set; }

        public DateTime UpdTime { get; set; }


		/// <summary>
        /// 费用
        /// </summary>
		public decimal Flee {get;set;}


		/// <summary>
        /// 地址
        /// </summary>
		public string Address {get;set;}


		/// <summary>
        /// 服务时间
        /// </summary>
		public DateTime Servicetime {get;set;}


		/// <summary>
        /// 备注
        /// </summary>
		public string Remark {get;set;}

        /// <summary>
        /// 状态
        /// </summary>
        public EnumServiceOrderStatus Status { get; set; }

        public string StatusString
        {
            get
            {
                switch (Status)
                {

                    case EnumServiceOrderStatus.Created:
                        return "新建";

                    case EnumServiceOrderStatus.Payed:
                        return "已付款";

                    //case EnumOrderStatus.Delivering:
                    //    return "配送中";

                    case EnumServiceOrderStatus.Successed:
                        return "订单完成";

                    case EnumServiceOrderStatus.Canceled:
                        return "订单关闭";

                    default:
                        return "";
                }
            }
        }


        public List<ServiceOrderDetailModel> Details { get; set; }

        public string UserName { get; set; }

        public int MemberAddressId { get; set; }
	}
}