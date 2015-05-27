using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class PublishProductEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Product_Id
		/// </summary>
		public int ProductId { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public string ProductName { get; set; }
		/// <summary>
		/// 详细
		/// </summary>
		public string Detail { get; set; }
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime Publishtime { get; set; }
		/// <summary>
		/// 发布人
		/// </summary>
		public int PublishUser { get; set; }
		/// <summary>
		/// 标签
		/// </summary>
		public virtual IList<TagEntity> Tags { get; set; }
	}
}