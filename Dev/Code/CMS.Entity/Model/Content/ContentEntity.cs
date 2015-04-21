using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class ContentEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public virtual string Content { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public virtual string Title { get; set; }
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
		/// 状态
		/// </summary>
		public virtual EnumContentStatus Status { get; set; }
		/// <summary>
		/// 点赞数
		/// </summary>
		public virtual int Praise { get; set; }
		/// <summary>
		/// 点踩数
		/// </summary>
		public virtual int Unpraise { get; set; }
		/// <summary>
		/// 点击数
		/// </summary>
		public virtual int Viewcount { get; set; }
		/// <summary>
		/// 内容所带多媒体资源
		/// </summary>
		public virtual IList<ResourceEntity> Resources { get; set; }
		/// <summary>
		/// 标签
		/// </summary>
		public virtual IList<TagEntity> Tags { get; set; }
		/// <summary>
		/// 所属频道
		/// </summary>
		public virtual IList<ChannelEntity> Channels { get; set; }
	}
}