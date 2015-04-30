using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskPunishmentEntity : IBaseEntity
	{
		/// <summary>
		/// 失败惩罚ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 惩罚名
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 惩罚描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 目标值
		/// </summary>
		public virtual string Value { get; set; }
	}
}