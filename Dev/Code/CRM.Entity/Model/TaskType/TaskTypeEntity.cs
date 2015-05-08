using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskTypeEntity : IBaseEntity
	{
		/// <summary>
		/// 任务类型ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 类型名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 类型描述
		/// </summary>
		public virtual string Describe { get; set; }
	}
}