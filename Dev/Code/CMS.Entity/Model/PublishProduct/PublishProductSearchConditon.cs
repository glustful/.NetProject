using System;
using System.Collections.Generic;

namespace CMS.Entity.Model
{
	public class PublishProductSearchCondition
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


		public int[] ProductIds { get; set; }

		public string ProductName { get; set; }

		public DateTime? PublishtimeBegin { get; set; }

		public DateTime? PublishtimeEnd { get; set; }


		public int[] PublishUsers { get; set; }

		public IList<TagEntity>[] Tagss { get; set; }

		public EnumPublishProductSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumPublishProductSearchOrderBy
	{
		OrderById,
		OrderByPublishtime,
	}
}