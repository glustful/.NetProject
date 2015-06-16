using System;

namespace CRM.Entity.Model
{
	public class RecommendAgentSearchCondition
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


		public int BrokerId{ get; set; }

		public int[] PresenteebIds { get; set; }

		public string Brokername { get; set; }

		public string Phone { get; set; }

		public string Qq { get; set; }

		public string Agentlevel { get; set; }

		public DateTime? RegtimeBegin { get; set; }

		public DateTime? RegtimeEnd { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumRecommendAgentSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumRecommendAgentSearchOrderBy
	{
		OrderById,
	}
}