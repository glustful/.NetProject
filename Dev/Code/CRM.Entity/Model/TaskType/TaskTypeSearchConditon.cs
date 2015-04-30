







using System;

namespace CRM.Entity.Model
{
	public class TaskTypeSearchCondition
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



		public string Describe { get; set; }




		public EnumTaskTypeSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumTaskTypeSearchOrderBy
	{

		OrderById,

	}

}