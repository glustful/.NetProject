







using System;

namespace Community.Entity.Model.ProductDetail
{
	public class ProductDetailSearchCondition
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





		public string Name { get; set; }





		public int? AddUser { get; set; }



		public DateTime? AddTimeBegin { get; set; }

		public DateTime? AddTimeEnd { get; set; }



		public int? UpdUser { get; set; }



		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }




		public EnumProductDetailSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductDetailSearchOrderBy
	{

		OrderById,

		OrderByName,

	}

}