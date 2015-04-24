using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class TagEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 标签
		/// </summary>
		public  string Tag { get; set; }
		/// <summary>
		/// 添加人
		/// </summary>
		public  int Adduser { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public  DateTime Addtime { get; set; }
		/// <summary>
		/// 更新人
		/// </summary>
		public  int UpdUser { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public  DateTime UpdTime { get; set; }
		/// <summary>
		/// 关联内容
		/// </summary>
		public virtual IList<ContentEntity> Content { get; set; }
        /// <summary>
        /// 发布的商品信息
        /// </summary>
        public virtual IList<PublishProductEntity> PublishProduct { get; set; } 
	}
}