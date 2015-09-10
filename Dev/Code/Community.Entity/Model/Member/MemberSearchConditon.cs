namespace Community.Entity.Model.Member
{
	public class MemberSearchCondition
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





		public string  RealName { get; set; }





		public string  IdentityNo { get; set; }



		public EnumGender? Gender { get; set; }





		public string  Phone { get; set; }






		public EnumMemberSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumMemberSearchOrderBy
	{

		OrderById,

		OrderByRealName,

		OrderByGender,

		OrderByPhone,

	}

}