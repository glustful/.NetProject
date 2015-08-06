using System;

namespace CRM.Entity.Model
{
	public class MessageConfigSearchCondition
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


		public string Name { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public EnumMessageConfigSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumMessageConfigSearchOrderBy
	{
		OrderById,
        OrderByName,
        OrderByTemplate
	}
}