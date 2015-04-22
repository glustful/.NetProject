using System;

namespace CRM.Entity.Model
{
	public class BrokerBillSearchCondition
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


		public BrokerEntity[] Brokers { get; set; }

		public int[] BillIds { get; set; }

		public bool? Type { get; set; }

		public decimal? Billamount { get; set; }

		public decimal? Paidinamount { get; set; }

		public string Cardnum { get; set; }

		public int[] Merchantids { get; set; }

		public string Merchantname { get; set; }

		public string Payeeuser { get; set; }

		public string Payeenum { get; set; }

		public DateTime? PaytimeBegin { get; set; }

		public DateTime? PaytimeEnd { get; set; }

		public string Customername { get; set; }

		public string Note { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokerBillSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumBrokerBillSearchOrderBy
	{
		OrderById,
	}
}