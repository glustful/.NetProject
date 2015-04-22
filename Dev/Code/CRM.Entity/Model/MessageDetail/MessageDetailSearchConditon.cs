using System;

namespace CRM.Entity.Model
{
	public class MessageDetailSearchCondition
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


		public string Title { get; set; }

		public string Content { get; set; }

		public int[] Senders { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public EnumMessageDetailSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumMessageDetailSearchOrderBy
	{
		OrderById,
	}
}