using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class AdvertisementEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public virtual string Title { get; set; }
		/// <summary>
		/// 明细
		/// </summary>
		public virtual string Detail { get; set; }
		/// <summary>
		/// 持续时间
		/// </summary>
		public virtual DateTime Continue { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public virtual ContentEntity Content { get; set; }
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