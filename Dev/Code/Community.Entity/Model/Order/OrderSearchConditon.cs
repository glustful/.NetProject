using System;

namespace Community.Entity.Model.Order
{
	public class OrderSearchCondition
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





		public string No { get; set; }



		public EnumOrderStatus? Status { get; set; }



		public string CustomerName { get; set; }



		public string Remark { get; set; }



		public DateTime? AdddateBegin { get; set; }

		public DateTime? AdddateEnd { get; set; }



		public int[] Addusers { get; set; }



		public int[] Updusers { get; set; }



		public DateTime? UpddateBegin { get; set; }

		public DateTime? UpddateEnd { get; set; }



		public decimal? TotalpriceBegin { get; set; }

		public decimal? TotalpriceEnd { get; set; }



		public decimal? ActualpriceBegin { get; set; }

		public decimal? ActualpriceEnd { get; set; }




		public EnumOrderSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumOrderSearchOrderBy
	{

		OrderById,

	}

}