using System;

namespace CRM.Entity.Model
{
	public class PartnerListSearchCondition
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


		public BrokerEntity Brokers { get; set; }

		public int[] PartnerIds { get; set; }

		public string Brokername { get; set; }

		public int? Phone { get; set; }

		public string Agentlevel { get; set; }

		public DateTime? RegtimeBegin { get; set; }

		public DateTime? RegtimeEnd { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumPartnerListSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumPartnerListSearchOrderBy
	{
		OrderById,
	}
}