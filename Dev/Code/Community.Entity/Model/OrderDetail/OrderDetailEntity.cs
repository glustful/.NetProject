using System;
using Community.Entity.Model.Order;
using Community.Entity.Model.Product;
using YooPoon.Core.Data;

namespace Community.Entity.Model.OrderDetail
{
	public class OrderDetailEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual ProductEntity Product { get; set; }
		/// <summary>
		/// 商品名字
		/// </summary>
		public virtual string ProductName { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public virtual decimal UnitPrice { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public virtual decimal Count { get; set; }
		/// <summary>
		/// 商品页面快照
		/// </summary>
		public virtual string Snapshoturl { get; set; }
		/// <summary>
		/// 商品备注
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 添加人员
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public virtual DateTime Adddate { get; set; }
		/// <summary>
		/// 修改人
		/// </summary>
		public virtual int Upduser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime Upddate { get; set; }
		/// <summary>
		/// 总价
		/// </summary>
		public virtual decimal Totalprice { get; set; }
		/// <summary>
		/// 订单主体
		/// </summary>
		public virtual OrderEntity Order { get; set; }
	}
}