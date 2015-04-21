using System;

namespace CMS.Entity.Model
{
	public class AdvertisementSearchCondition
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


		public string Title { get; set; }


		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }


		public int[] UpdUsers { get; set; }

		public DateTime? UpdTimeBegin { get; set; }

		public DateTime? UpdTimeEnd { get; set; }


		public EnumAdvertisementSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumAdvertisementSearchOrderBy
	{
		OrderById,
		OrderByTitle,
		OrderByAddtime,
		OrderByUpdTime,
	}
}