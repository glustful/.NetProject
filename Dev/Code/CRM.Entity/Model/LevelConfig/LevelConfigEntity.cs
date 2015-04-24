using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class ChannelEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public EnumChannelStatus Status { get; set; }
		/// <summary>
		/// 父级
		/// </summary>
		public virtual ChannelEntity Parent { get; set; }
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
        /// 频道内内容
        /// </summary>
        public virtual IList<ContentEntity> Contents { get; set; } 
	}
}