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
		public virtual int Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public virtual EnumChannelStatus Status { get; set; }
		/// <summary>
		/// 父级
		/// </summary>
		public virtual ChannelEntity Parent { get; set; }
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