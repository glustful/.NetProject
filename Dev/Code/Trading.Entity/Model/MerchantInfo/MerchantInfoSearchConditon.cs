using System;

namespace CRM.Entity.Model
{
	public class MerchantInfoSearchCondition
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


		public int[] UserIds { get; set; }

		public string Merchantname { get; set; }

		public string Mail { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public string License { get; set; }

		public string Legalhuman { get; set; }

		public string Legalsfz { get; set; }

		public string Orgnum { get; set; }

		public string Taxnum { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumMerchantInfoSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumMerchantInfoSearchOrderBy
	{
		OrderById,
	}
}