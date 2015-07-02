







using System;

namespace Event.Entity.Model
{
	public class FollowerSearchCondition
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





		public string Openid { get; set; }



		public string Nickname { get; set; }



		public string Sex { get; set; }



		public string City { get; set; }



		public string Country { get; set; }



		public string Private { get; set; }



		public string Language { get; set; }



		public string Headimgurl { get; set; }



		public string Subscribetime { get; set; }



		public string Unioid { get; set; }



		public string Remark { get; set; }



		public string Groupid { get; set; }



		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }




		public EnumFollowerSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumFollowerSearchOrderBy
	{

		OrderById,

	}

}