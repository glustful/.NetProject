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
		public virtual int Id { get; set; }
		/// <summary>
		/// 标签
		/// </summary>
		public virtual string Tag { get; set; }
		/// <summary>
		/// 添加人
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// 更新人
		/// </summary>
		public virtual int UpdUser { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public virtual DateTime UpdTime { get; set; }
		/// <summary>
		/// 关联内容
		/// </summary>
		public virtual IList<ContentEntity> Content { get; set; }
	}
}