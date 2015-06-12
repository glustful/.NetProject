using System;

namespace CRM.Entity.Model
{
	public class MessageDetailSearchCondition
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
		public bool isDescending { get; set; }

		public int[] Ids { get; set; }


		public string Title { get; set; }

		public string Content { get; set; }

		public string Senders { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }


        /// <summary>
        /// 邀请码(用于邀请经纪人)
        /// </summary>
        public virtual string InvitationCode { get; set; }

        /// <summary>
        /// 邀请人(用于邀请经纪人) 跟经纪人ID相关
        /// </summary>
        public virtual string InvitationId { get; set; }


		public EnumMessageDetailSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumMessageDetailSearchOrderBy
	{
		OrderById,
	}
}