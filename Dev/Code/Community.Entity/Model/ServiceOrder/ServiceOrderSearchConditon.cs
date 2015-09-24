using System;

namespace Community.Entity.Model.ServiceOrder
{
	public class ServiceOrderSearchCondition
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





		public string OrderNo { get; set; }





		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] AddUsers { get; set; }



		public decimal? FleeBegin { get; set; }

		public decimal? FleeEnd { get; set; }



		public string Address { get; set; }



		public DateTime? ServicetimeBegin { get; set; }

		public DateTime? ServicetimeEnd { get; set; }



		public string Remark { get; set; }

        public EnumServiceOrderStatus? Status { get; set; }


		public EnumServiceOrderSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumServiceOrderSearchOrderBy
	{

		OrderById,

		OrderByOrderNo,

	}

}