using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskAwardEntity : IBaseEntity
	{
		/// <summary>
		/// 发放奖励ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 奖励名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 奖励描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 目标值
		/// </summary>
		public virtual string Value { get; set; }
	}
}