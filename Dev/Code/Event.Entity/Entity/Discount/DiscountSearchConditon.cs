







using System;

namespace Event.Entity.Model
{
	public class DiscountSearchCondition
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





		public CrowdEntity[] Crowds { get; set; }



		public int[] Numbers { get; set; }



		public int[] Discounts { get; set; }



		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }




		public EnumDiscountSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumDiscountSearchOrderBy
	{

		OrderById,

	}

}