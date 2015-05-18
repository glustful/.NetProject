







using System;

namespace CRM.Entity.Model
{
	public class TaskSearchCondition
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



        public int Id { get; set; }
      

		public TaskPunishmentEntity[] TaskPunishments { get; set; }



		public TaskAwardEntity[] TaskAwards { get; set; }



		public TaskTagEntity[] TaskTags { get; set; }



		public TaskTypeEntity[] TaskTypes { get; set; }



		public string Taskname { get; set; }



		public DateTime? EndtimeBegin { get; set; }

		public DateTime? EndtimeEnd { get; set; }



		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }
        /// <summary>
        /// 类型id
        /// </summary>
        public int typeId { get; set; }
        /// <summary>
        /// 奖励id
        /// </summary>
        public int awardId { get; set; }
        /// <summary>
        /// 惩罚id
        /// </summary>
        public int punishId { get; set; }
        /// <summary>
        /// 目标id
        /// </summary>
        public int tagId { get; set; }


		public EnumTaskSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumTaskSearchOrderBy
	{

		OrderById,

	}

}