using System;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ProductDetail
{
	public class ProductDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 商品Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 商品明细
		/// </summary>
		public virtual string Detail { get; set; }
		/// <summary>
		/// 商品图片（主图）
		/// </summary>
		public virtual string Img { get; set; }
		/// <summary>
		/// 商品附图1
		/// </summary>
		public virtual string Img1 { get; set; }
		/// <summary>
		/// 商品附图2
		/// </summary>
		public virtual string Img2 { get; set; }
		/// <summary>
		/// 商品附图3
		/// </summary>
		public virtual string Img3 { get; set; }
		/// <summary>
		/// 商品附图4
		/// </summary>
		public virtual string Img4 { get; set; }
		/// <summary>
		/// 售后说明
		/// </summary>
		public virtual string SericeInstruction { get; set; }
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
		/// 广告1
		/// </summary>
		public virtual string Ad1 { get; set; }
		/// <summary>
		/// 广告2
		/// </summary>
		public virtual string Ad2 { get; set; }
		/// <summary>
		/// 广告3
		/// </summary>
		public virtual string Ad3 { get; set; }
	}
}