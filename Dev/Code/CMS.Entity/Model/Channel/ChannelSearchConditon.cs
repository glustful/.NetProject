using System;

namespace CMS.Entity.Model
{
	public class ChannelSearchCondition
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


		public EnumChannelStatus? Status { get; set; }


		public EnumChannelSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumChannelSearchOrderBy
	{
		OrderById,
		OrderByName,
		OrderByStatus,
	}
}