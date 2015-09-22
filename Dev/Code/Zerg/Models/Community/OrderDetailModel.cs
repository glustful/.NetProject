using Community.Entity.Model.Order;
using System;

namespace Zerg.Models.Community
{
	public class OrderDetailModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}
        /// <summary>
        /// 商品订单号
        /// </summary>
        public string No { get; set; }

		/// <summary>
        /// 商品ID
        /// </summary>
//		public Product Product {get;set;}


		/// <summary>
        /// 商品名字
        /// </summary>
		public string ProductName {get;set;}


		/// <summary>
        /// 购买价格
        /// </summary>
		public decimal UnitPrice {get;set;}
        /// <summary>
        /// 原始价格
        /// </summary>
        public decimal Price { get; set; }

		/// <summary>
        /// 数量
        /// </summary>
		public decimal Count {get;set;}


		/// <summary>
        /// 商品页面快照
        /// </summary>
		public string Snapshoturl {get;set;}

        public string Status{get;set;}
		/// <summary>
        /// 商品备注
        /// </summary>
		public string Remark {get;set;}


		/// <summary>
        /// 添加人员
        /// </summary>
		public int Adduser {get;set;}


		/// <summary>
        /// 添加时间
        /// </summary>
		public DateTime Adddate {get;set;}


		/// <summary>
        /// 修改人
        /// </summary>
		public int Upduser {get;set;}


		/// <summary>
        /// 修改时间
        /// </summary>
		public DateTime Upddate {get;set;}


		/// <summary>
        /// 总价
        /// </summary>
		public decimal Totalprice {get;set;}

       
		/// <summary>
        /// 订单主体
        /// </summary>
//		public Order Order {get;set;}

        public ProductModel Product { get; set; }

	}
}