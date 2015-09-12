using System;
using Community.Entity.Model.Parameter;
using Community.Entity.Model.ParameterValue;
using Community.Entity.Model.Product;

namespace Community.Entity.Model.ProductParameter
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





		public ParameterValueEntity ParameterValue { get; set; }



		public ParameterEntity Parameter { get; set; }



		public ProductEntity Product { get; set; }





		public int? AddUser { get; set; }



		public DateTime? AddTimeBegin { get; set; }

		public DateTime? AddTimeEnd { get; set; }



		public int? UpdUser { get; set; }



		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }




		public EnumProductParameterSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductParameterSearchOrderBy
	{

		OrderById,

		OrderBySort,

	}

}