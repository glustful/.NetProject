using System;

namespace CMS.Entity.Model
{
	public class SettingSearchCondition
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


		public string Key { get; set; }

		public string Value { get; set; }

		public EnumSettingSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumSettingSearchOrderBy
	{
		OrderById,
	}
}