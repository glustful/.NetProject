using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class ProductEntity : IBaseEntity
	{
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品基本信息ID
		/// </summary>
		public virtual ProductDetailEntity ProductDetail { get; set; }
		/// <summary>
		/// 平台商品分类ID
		/// </summary>
		public virtual ClassifyEntity Classify { get; set; }
		/// <summary>
		/// 品牌ID
		/// </summary>
		public virtual ProductBrandEntity ProductBrand { get; set; }
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual int Bussnessid { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public virtual string BussnessName { get; set; }
        /// <summary>
        /// 推荐佣金
        /// </summary>
	    public virtual decimal RecCommission { get; set; }
	    /// <summary>
		/// 带客佣金
		/// </summary>
		public virtual decimal Commission { get; set; }
        /// <summary>
		/// 成交佣金
		/// </summary>
        public virtual decimal Dealcommission { get; set; }
		/// <summary>
		/// 价格
		/// </summary>
		public virtual decimal Price { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public virtual string Productname { get; set; }
		/// <summary>
		/// 商品状态
		/// </summary>
		public virtual bool Status { get; set; }
		/// <summary>
		/// 商品主图
		/// </summary>
		public virtual string Productimg { get; set; }
		/// <summary>
		/// 商家推荐标识
		/// </summary>
		public virtual bool Recommend { get; set; }
		/// <summary>
		/// 分类排序
		/// </summary>
		public virtual int Sort { get; set; }
		/// <summary>
		/// 库存计数（拍下、付款）
		/// </summary>
		public virtual int Stockrule { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual string Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual string Upduser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime Updtime { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        public virtual List<ProductParameterEntity> ProductParameter { get; set; } 
	}
}