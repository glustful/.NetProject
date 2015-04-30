using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskListEntity : IBaseEntity
	{
		/// <summary>
		/// 任务列表ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 任务表ID
		/// </summary>
		public virtual TaskEntity Task { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 任务进度
		/// </summary>
		public string Taskschedule { get; set; }
	}
}