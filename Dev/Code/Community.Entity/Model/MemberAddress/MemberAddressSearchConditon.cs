using System;

namespace Community.Entity.Model.MemberAddress
{
	public class MemberAddressSearchCondition
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







		public string Address { get; set; }



		public string Zip { get; set; }



		public string Linkman { get; set; }



		public string Tel { get; set; }



		public int? Adduser { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int? Upduser { get; set; }



		public DateTime? UpdtimeBegin { get; set; }

		public DateTime? UpdtimeEnd { get; set; }

        public string UserName { get; set; }

        public int? MemberId { get; set; }

        public int? UserId { get; set; }

		public EnumMemberAddressSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumMemberAddressSearchOrderBy
	{

		OrderById,

		OrderByMember,

	}

}