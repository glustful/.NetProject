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
		public int Id { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public string Content { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }
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
		/// 状态
		/// </summary>
		public EnumContentStatus Status { get; set; }
		/// <summary>
		/// 点赞数
		/// </summary>
		public int Praise { get; set; }
		/// <summary>
		/// 点踩数
		/// </summary>
		public int Unpraise { get; set; }
		/// <summary>
		/// 点击数
		/// </summary>
		public int Viewcount { get; set; }
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
		public virtual ChannelEntity Channel { get; set; }
	}
}