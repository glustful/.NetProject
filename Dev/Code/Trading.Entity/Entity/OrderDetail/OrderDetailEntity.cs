using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class OrderDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 订单明细ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual ProductEntity Product { get; set; }
		/// <summary>
		/// 商品名字
		/// </summary>
		public virtual string Productname { get; set; }
		/// <summary>
		/// 价格
		/// </summary>
		public virtual decimal Price { get; set; }
		/// <summary>
		/// 带客佣金
		/// </summary>
        public virtual decimal Commission { get; set; }
        /// <summary>
        /// 成交佣金
        /// </summary>
        public virtual decimal Dealcommission { get; set; }
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
		public virtual string Adduser { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public virtual DateTime Adddate { get; set; }
		/// <summary>
		/// 修改人
		/// </summary>
		public virtual string Upduser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime Upddate { get; set; }
	}
}