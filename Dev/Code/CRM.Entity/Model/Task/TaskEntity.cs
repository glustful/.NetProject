using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class TaskEntity : IBaseEntity
	{
		/// <summary>
		/// 任务属性ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 失败惩罚ID
		/// </summary>
		public virtual TaskPunishmentEntity TaskPunishment { get; set; }
		/// <summary>
		/// 发放奖励ID
		/// </summary>
		public virtual TaskAwardEntity TaskAward { get; set; }
		/// <summary>
		/// 任务目标ID
		/// </summary>
		public virtual TaskTagEntity TaskTag { get; set; }
		/// <summary>
		/// 任务类型ID
		/// </summary>
		public virtual TaskTypeEntity TaskType { get; set; }
		/// <summary>
		/// 任务名称
		/// </summary>
		public virtual string Taskname { get; set; }
		/// <summary>
		/// 任务描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public virtual DateTime Endtime { get; set; }
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