using System;

namespace CRM.Entity.Model
{
	public class TaskListSearchCondition
	{
		/// <summary>
		/// 页码
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		/// 每页大小
		/// </summary>
		public int? PageCount { get; set; }

		/// <summary>
		/// 是否降序
		/// </summary>
		public bool IsDescending { get; set; }

		public int[] Ids { get; set; }


		public TaskEntity[] Tasks { get; set; }

		public BrokerEntity[] Brokers { get; set; }

		public string Taskschedule { get; set; }

		public EnumTaskListSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumTaskListSearchOrderBy
	{
		OrderById,
	}
}