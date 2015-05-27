using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class ProductDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public virtual string Productname { get; set; }
		/// <summary>
		/// 商品明细
		/// </summary>
		public virtual string Productdetail { get; set; }
		/// <summary>
		/// 商品图片（主图）
		/// </summary>
		public virtual string Productimg { get; set; }
		/// <summary>
		/// 商品附图1
		/// </summary>
		public virtual string Productimg1 { get; set; }
		/// <summary>
		/// 商品附图2
		/// </summary>
		public virtual string Productimg2 { get; set; }
		/// <summary>
		/// 商品附图3
		/// </summary>
		public virtual string Productimg3 { get; set; }
		/// <summary>
		/// 商品附图4
		/// </summary>
		public virtual string Productimg4 { get; set; }
		/// <summary>
		/// 售后说明
		/// </summary>
		public virtual string Sericeinstruction { get; set; }
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
	}
}