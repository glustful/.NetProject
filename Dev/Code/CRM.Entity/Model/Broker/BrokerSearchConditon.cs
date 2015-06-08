using System;

namespace CRM.Entity.Model
{
	public class BrokerSearchCondition
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


		public LevelEntity[] Levels { get; set; }

		public int[] UserIds { get; set; }

		public string Brokername { get; set; }

		public string Nickname { get; set; }

		public string Realname { get; set; }

		public string Sfz { get; set; }

		public string Sexy { get; set; }

		public string Phone { get; set; }

		public int[] Qqs { get; set; }

		public int[] Zips { get; set; }

	    public string Email { get; set; }

	    public string Headphoto { get; set; }

		public int[] Totalpointss { get; set; }

		public decimal? Amount { get; set; }

		public string Agentlevel { get; set; }

		//public string Usertype { get; set; }

		public string Address { get; set; }

		public DateTime? RegtimeBegin { get; set; }

		public DateTime? RegtimeEnd { get; set; }

		public int? Delflag { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokerSearchOrderBy? OrderBy { get; set; }

	    public EnumUserType? UserType { get; set; }

	    public EnumPartnerType? Status { get; set; }
	}

	public enum EnumBrokerSearchOrderBy
	{
		OrderById,
	}
}