using System;

namespace Community.Entity.Model.Category
{
	public class CategorySearchCondition
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



		public int[] Sorts { get; set; }



		public string AddUser { get; set; }



		public DateTime? AddTimeBegin { get; set; }

		public DateTime? AddTimeEnd { get; set; }





		public string UpdUser { get; set; }



		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }




		public EnumCategorySearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumCategorySearchOrderBy
	{

		OrderById,

		OrderByAddTime,

	}

}