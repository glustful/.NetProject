using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskTagEntity : IBaseEntity
	{
		/// <summary>
		/// 任务目标ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 目标名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 目标描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 目标值
		/// </summary>
		public virtual string Value { get; set; }
	}
}