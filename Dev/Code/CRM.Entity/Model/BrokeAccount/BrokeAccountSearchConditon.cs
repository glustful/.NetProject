using System;

namespace CRM.Entity.Model
{
	public class BrokeAccountSearchCondition
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
        /// 状态 0 可用，1 已使用,-1提现中
        /// </summary>
        public int? State { get; set; }
		/// <summary>
		/// 是否降序
		/// </summary>
		public bool isDescending { get; set; }

		public int[] Ids { get; set; }


		public BrokerEntity Brokers { get; set; }

		public decimal? Balancenum { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokeAccountSearchOrderBy? OrderBy { get; set; }
	    public int? BrokerId { get; set; }

        /// <summary>
        /// 金额类型（0 带客，1 推荐,2 奖励完整信息钱）
        /// </summary>
        public int? Type { get; set; }
	}

	public enum EnumBrokeAccountSearchOrderBy
	{
		OrderById,
	}
}