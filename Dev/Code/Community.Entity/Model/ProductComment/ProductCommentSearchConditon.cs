using System;
using Community.Entity.Model.Product;

namespace Community.Entity.Model.ProductComment
{
	public class ProductCommentSearchCondition
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



	    public int ?Stars { get; set; }

	    public ProductEntity[] Products { get; set; }

	    public int? ProductId { get; set; }

	    public int? AddUser { get; set; }



		public DateTime? AddTimeBegin { get; set; }

		public DateTime? AddTimeEnd { get; set; }





		public string Content { get; set; }



		public int? StarsBegin { get; set; }

		public int? StarsEnd { get; set; }






		public EnumProductCommentSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumProductCommentSearchOrderBy
	{

		OrderById,

		OrderByAddTime,

		OrderByStars,

	}

}