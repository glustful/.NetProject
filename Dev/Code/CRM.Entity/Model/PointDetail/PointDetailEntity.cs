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
		public int Id { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 明细
		/// </summary>
		public string Detail { get; set; }
		/// <summary>
		/// 持续时间
		/// </summary>
		public DateTime Continue { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public virtual ContentEntity Content { get; set; }
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
	}
}