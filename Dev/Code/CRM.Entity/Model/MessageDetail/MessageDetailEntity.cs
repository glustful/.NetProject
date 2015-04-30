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
		/// 接收人
		/// </summary>
        public virtual string Sender { get; set; }

        /// <summary>
		/// 手机号
		/// </summary>
		public virtual string Mobile { get; set; }

		/// <summary>
		/// 发送时间
		/// </summary>
		public virtual DateTime Addtime { get; set; }
	}
}