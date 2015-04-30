using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class MessageConfigEntity : IBaseEntity
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 短信配置名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 短信配置模版
		/// </summary>
		public virtual string Template { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// UpTime
		/// </summary>
		public virtual DateTime Uptime { get; set; }
		/// <summary>
		/// UpUser
		/// </summary>
		public virtual int Upuser { get; set; }
	}
}