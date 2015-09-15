using System;
using Community.Entity.Model.Category;

namespace Community.Entity.Model.Product
{
	public class ProductSearchCondition
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





		public CategoryEntity[] Categorys { get; set; }
        public int? CategoryId { get; set; }



		public decimal? PriceBegin { get; set; }

		public decimal? PriceEnd { get; set; }





		public string Name { get; set; }



        public EnumProductStatus? Status { get; set; }



		public int? IsRecommend { get; set; }





		public int? StockBegin { get; set; }

		public int? StockEnd { get; set; }



		public int? Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }





		public int? UpdUser { get; set; }



		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }



		public string Subtitte { get; set; }



		public EnumProductType? Type { get; set; }




		public EnumProductSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductSearchOrderBy
	{

		OrderById,

		OrderByPrice,

		OrderBySort,

		OrderByAddtime,

        OrderByOwner

	}

}