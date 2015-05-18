







using System;

namespace Trading.Entity.Model
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





		public string Productname { get; set; }



		public string Productdetail { get; set; }



		public string Productimg { get; set; }



		public string Productimg1 { get; set; }



		public string Productimg2 { get; set; }



		public string Productimg3 { get; set; }



		public string Productimg4 { get; set; }



		public string Sericeinstruction { get; set; }



		public string Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpdtimeBegin { get; set; }

		public DateTime? UpdtimeEnd { get; set; }




		public EnumProductDetailSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductDetailSearchOrderBy
	{

		OrderById,

	}

}