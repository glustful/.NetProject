using System;

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



	}
}