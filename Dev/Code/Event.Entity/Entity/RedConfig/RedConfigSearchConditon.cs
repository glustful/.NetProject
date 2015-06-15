







using System;

namespace Event.Entity.Model
{
	public class RedConfigSearchCondition
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



		public int[] 商家关联ids { get; set; }



		public string Ttitle { get; set; }



		public string Intro { get; set; }



		public DateTime? StarttimeBegin { get; set; }

		public DateTime? StarttimeEnd { get; set; }





		public DateTime? EndtimeBegin { get; set; }

		public DateTime? EndtimeEnd { get; set; }





		public int? StatusBegin { get; set; }

		public int? StatusEnd { get; set; }





		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }




		public EnumRedConfigSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumRedConfigSearchOrderBy
	{

		OrderByStarttime,

		OrderByEndtime,

		OrderByStatus,

	}

}