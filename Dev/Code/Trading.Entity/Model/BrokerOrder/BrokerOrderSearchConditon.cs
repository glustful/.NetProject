using System;

namespace CRM.Entity.Model
{
	public class BrokerOrderSearchCondition
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

		public int[] OrderIds { get; set; }

		public string Merchantname { get; set; }

		public bool? Status { get; set; }

		public DateTime? OrdertimeBegin { get; set; }

		public DateTime? OrdertimeEnd { get; set; }

		public string Orderuser { get; set; }

		public string Modifyuser { get; set; }

		public DateTime? ModifytimeBegin { get; set; }

		public DateTime? ModifytimeEnd { get; set; }

		public string Customername { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokerOrderSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumBrokerOrderSearchOrderBy
	{
		OrderById,
	}
}