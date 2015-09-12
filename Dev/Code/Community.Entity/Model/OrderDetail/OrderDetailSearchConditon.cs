using System;

namespace Community.Entity.Model.OrderDetail
{
	public class OrderDetailSearchCondition
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


		public string ProductName { get; set; }



		public string Adduser { get; set; }



		public DateTime? AdddateBegin { get; set; }

		public DateTime? AdddateEnd { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpddateBegin { get; set; }

		public DateTime? UpddateEnd { get; set; }




		public EnumOrderDetailSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumOrderDetailSearchOrderBy
	{

		OrderById,

	}

}