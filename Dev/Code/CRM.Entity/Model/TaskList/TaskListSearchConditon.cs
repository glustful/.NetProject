using System;
using System.Linq;

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
		public bool isDescending { get; set; }

		public int[] Ids { get; set; }

    
		public TaskEntity[] Tasks { get; set; }
        public TaskEntity Task{ get; set; }

		public BrokerEntity[] Brokers { get; set; }
        public IQueryable<BrokerEntity> Broker { get; set; }

		public string Taskschedule { get; set; }

		public EnumTaskListSearchOrderBy? OrderBy { get; set; }

        public string BrokerName { get; set; }
        /// <summary>
        /// 任务Id
        /// </summary>
        public int TaskId { get; set; }
	}

	public enum EnumTaskListSearchOrderBy
	{
		OrderById,
	}
}