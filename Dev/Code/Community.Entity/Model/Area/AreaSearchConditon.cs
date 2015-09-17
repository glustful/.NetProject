using System;

namespace Community.Entity.Model.Area
{
	public class AreaSearchCondition
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

        public string Parent_Id { get; set; }



        public string Codeid { get; set; }



		public DateTime? AdddateBegin { get; set; }

		public DateTime? AdddateEnd { get; set; }



		public string Name { get; set; }




		public EnumAreaSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumAreaSearchOrderBy
	{

		OrderById,

	}

}