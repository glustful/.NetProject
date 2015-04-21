using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class ResourceEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// GUID
		/// </summary>
		public virtual Guid Guid { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 类型
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public virtual Int64 Length { get; set; }
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
	}
}