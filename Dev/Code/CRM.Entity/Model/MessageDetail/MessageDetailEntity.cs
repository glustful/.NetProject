using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class MessageDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 发送主题
		/// </summary>
		public virtual string Title { get; set; }
		/// <summary>
		/// 发送内容
		/// </summary>
		public virtual string Content { get; set; }
		/// <summary>
		/// 发送人
		/// </summary>
		public virtual int Sender { get; set; }
		/// <summary>
		/// 发送时间
		/// </summary>
		public virtual DateTime Addtime { get; set; }
	}
}