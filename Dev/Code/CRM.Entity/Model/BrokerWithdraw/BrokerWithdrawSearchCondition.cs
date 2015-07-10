using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.Model
{
    public class BrokerWithdrawSearchCondition
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
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool isAescending { get; set; }

		public int[] Ids { get; set; }

        /// <summary>
        /// 提现状态 ｛0，处理中   1完成 ok｝
        /// </summary>
        public int? State { get; set; }

		public BrokerEntity Brokers { get; set; }

		public BankCardEntity[] BankCards { get; set; }

        public DateTime? WithdrawtimeBegin { get; set; }

        public DateTime? WithdrawtimeEnd { get; set; }

        public decimal? WithdrawTotalNum { get; set; }

		public int[] Addusers { get; set; }

		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }

		public int[] Upusers { get; set; }

		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }

		public EnumBrokerWithdrawSearchOrderBy? OrderBy { get; set; }
	}

    public enum EnumBrokerWithdrawSearchOrderBy
	{
		OrderById,
        State,
	}
    
}
