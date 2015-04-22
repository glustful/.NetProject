using System;

namespace CRM.Entity.Model
{
	public class ClientInfoSearchCondition
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


		public string Clientname { get; set; }

		public string Phone { get; set; }

		public string Housetype { get; set; }

		public string Houses { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumClientInfoSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumClientInfoSearchOrderBy
	{
		OrderById,
	}
}