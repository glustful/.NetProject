using System;

namespace CRM.Entity.Model
{
	public class BankCardSearchCondition
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


		public BankEntity Banks { get; set; }

		public BrokerEntity Brokers { get; set; }

		public int[] Nums { get; set; }

		public string Type { get; set; }

		public DateTime? DeadlineBegin { get; set; }

		public DateTime? DeadlineEnd { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBankCardSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumBankCardSearchOrderBy
	{
		OrderById,
	}
}