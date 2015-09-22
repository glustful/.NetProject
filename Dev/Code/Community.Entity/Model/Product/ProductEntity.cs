using System;
using System.Collections.Generic;
using Community.Entity.Model.Category;
using Community.Entity.Model.ProductComment;
using Community.Entity.Model.ProductDetail;
using Community.Entity.Model.ProductParameter;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Product
{
	public class ProductEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 分类排序
		/// </summary>
		public virtual CategoryEntity Category { get; set; }
		/// <summary>
		/// 商家Id
		/// </summary>
		public virtual int BussnessId { get; set; }
		/// <summary>
		/// 商家名称
		/// </summary>
		public virtual string BussnessName { get; set; }
		/// <summary>
		/// 价格
		/// </summary>
		public virtual decimal Price { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 商品状态
		/// </summary>
		public virtual EnumProductStatus Status { get; set; }
		/// <summary>
		/// 商品主图
		/// </summary>
		public virtual string MainImg { get; set; }
		/// <summary>
		/// 商家推荐标识
		/// </summary>
		public virtual int IsRecommend { get; set; }
		/// <summary>
		/// 分类排序
		/// </summary>
		public virtual int Sort { get; set; }
		/// <summary>
		/// 库存计数
		/// </summary>
		public virtual int Stock { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int AddUser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime AddTime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual int UpdUser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime UpdTime { get; set; }
		/// <summary>
		/// 副标题
		/// </summary>
		public virtual string Subtitte { get; set; }
		/// <summary>
		/// 联系电话
		/// </summary>
		public virtual string Contactphone { get; set; }
		/// <summary>
		/// 商品类型
		/// </summary>
		public virtual EnumProductType Type { get; set; }
		/// <summary>
		/// 商品明细
		/// </summary>
		public virtual ProductDetailEntity Detail { get; set; }
		/// <summary>
		/// 商品评论
		/// </summary>
		public virtual List<ProductCommentEntity> Comments { get; set; }
		/// <summary>
		/// 商品属性
		/// </summary>
		public virtual List<ProductParameterEntity> Parameters { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public virtual decimal? OldPrice { get; set; }
        /// <summary>
        /// 有多少人抢购
        /// </summary>
        public virtual int? Owner { get; set; }
	}
}