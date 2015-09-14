using System;
using Community.Entity.Model.Category;

namespace Community.Entity.Model.Parameter
{
	public class ParameterSearchCondition
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



		public string Name { get; set; }





		public int? AddUser { get; set; }



		public DateTime? AddTimeBegin { get; set; }

		public DateTime? AddTimeEnd { get; set; }



		public int? UpdUser { get; set; }



		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }

        public CategoryEntity Category { get; set; }


		public EnumParameterSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumParameterSearchOrderBy
	{

		OrderById,

		OrderBySort,

	}

}