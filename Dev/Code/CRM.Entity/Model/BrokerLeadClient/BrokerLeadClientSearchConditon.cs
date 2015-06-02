using System;

namespace CRM.Entity.Model
{
	public class BrokerLeadClientSearchCondition
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

		public ClientInfoEntity[] ClientInfos { get; set; }

		public DateTime? AppointmenttimeBegin { get; set; }

		public DateTime? AppointmenttimeEnd { get; set; }

		public string Appointmentstatus { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokerLeadClientSearchOrderBy? OrderBy { get; set; }

	    public EnumBLeadType? Status { get; set; }
	}

	public enum EnumBrokerLeadClientSearchOrderBy
	{
		OrderById,
	}
}