







using System;

namespace Trading.Entity.Model
{
	public class ProductParameterSearchCondition
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





		public ParameterValueEntity[] ParameterValues { get; set; }



		public ParameterEntity[] Parameters { get; set; }



		public ProductEntity[] Products { get; set; }



		public int[] Sorts { get; set; }



		public string Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpdtimeBegin { get; set; }

		public DateTime? UpdtimeEnd { get; set; }




		public EnumProductParameterSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductParameterSearchOrderBy
	{

		OrderById,

	}

}