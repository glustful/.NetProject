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
		public int Id { get; set; }
		/// <summary>
		/// GUID
		/// </summary>
		public Guid Guid { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 类型
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public Int64 Length { get; set; }
		/// <summary>
		/// 添加人
		/// </summary>
		public int Adduser { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime Addtime { get; set; }
		/// <summary>
		/// 更新人
		/// </summary>
		public int UpdUser { get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UpdTime { get; set; }
        /// <summary>
        /// 相关内容
        /// </summary>
        public virtual ContentEntity Content { get; set; }
	}
}