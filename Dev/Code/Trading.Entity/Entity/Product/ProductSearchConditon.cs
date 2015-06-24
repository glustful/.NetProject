using System;

namespace Trading.Entity.Model
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


        public string AreaName { get; set; }
        public int? TypeId { get; set; }


		public ProductDetailEntity[] ProductDetails { get; set; }



		public ClassifyEntity[] Classifys { get; set; }



		public ProductBrandEntity[] ProductBrands { get; set; }

        public int? ProductBrand { get; set; }

		public int? Bussnessid { get; set; }

        public string BussnessName { get; set; }


		public decimal? CommissionBegin { get; set; }

		public decimal? CommissionEnd { get; set; }



		public decimal? PriceBegin { get; set; }

		public decimal? PriceEnd { get; set; }



		public string Productname { get; set; }



		public bool? Status { get; set; }



		public string Productimg { get; set; }



		public bool? Recommend { get; set; }



		public int[] Sorts { get; set; }



		public int[] Stockrules { get; set; }



		public string Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpdtimeBegin { get; set; }

		public DateTime? UpdtimeEnd { get; set; }




		public EnumProductSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductSearchOrderBy
	{

		OrderById,
        OrderByAddtime,
        Price
	}

}