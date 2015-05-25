







using System;

namespace CRM.Entity.Model
{
	public class BRECPaySearchCondition
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





		public BrokerRECClientEntity[] BrokerRECClients { get; set; }



		public string Name { get; set; }



		public EnumBRECP? Statusname { get; set; }



		public string Describe { get; set; }



		public decimal? AmountBegin { get; set; }

		public decimal? AmountEnd { get; set; }



		public int[] Accountantids { get; set; }



		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }




		public EnumBRECPaySearchOrderBy? OrderBy { get; set; }

	    public int? BankCard { get; set; }

	}


	public enum EnumBRECPaySearchOrderBy
	{

		OrderById,

	}

}