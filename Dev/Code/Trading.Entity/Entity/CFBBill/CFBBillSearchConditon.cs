







using System;

namespace Trading.Entity.Model
{
	public class CFBBillSearchCondition
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





		public OrderEntity[] Orders { get; set; }



		public int? AgentId { get; set; }



		public string Agentname { get; set; }



		public int[] LandagentIds { get; set; }



		public string Landagentname { get; set; }



		public decimal? AmountBegin { get; set; }

		public decimal? AmountEnd { get; set; }



		public decimal? ActualamountBegin { get; set; }

		public decimal? ActualamountEnd { get; set; }



		public string Cardnumber { get; set; }



		public bool? Isinvoice { get; set; }



		public string Remark { get; set; }



		public string Beneficiary { get; set; }



		public string Beneficiarynumber { get; set; }



		public DateTime? CheckoutdateBegin { get; set; }

		public DateTime? CheckoutdateEnd { get; set; }



		public string Customname { get; set; }



		public string Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpdtimeBegin { get; set; }

		public DateTime? UpdtimeEnd { get; set; }




		public EnumCFBBillSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumCFBBillSearchOrderBy
	{

		OrderById,
        OrderByCheckoutdate,
        OrderByAmount

	}

}