using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class LevelEntity : IBaseEntity
	{
		/// <summary>
		/// 等级ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 等级名称
		/// </summary>
		public virtual string CodeId { get; set; }
		/// <summary>
		/// 等级描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 图片SRC
		/// </summary>
		public virtual string Url { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpUser
		/// </summary>
		public virtual int Upuser { get; set; }
		/// <summary>
		/// UpTime
		/// </summary>
		public virtual DateTime Uptime { get; set; }
	}
}