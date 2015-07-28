using System;

namespace CRM.Entity.Model
{
    public class EventOrderSearchCondition
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
        /// 状态 0 为可用 1 为不可用
        /// </summary>
        public int? State { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public bool isDescending { get; set; }
        public int Id { get; set; }
      
        public BrokerEntity Brokers { get; set; }

        public DateTime? AddtimeBegin { get; set; }

        public DateTime? AddtimeEnd { get; set; }

        public decimal ?MoneyCount { get; set; }
        public EventEntity Event { get; set; }

        public string AcDetail { get; set; }


        public EnumEventOrderSearchOrderBy? OrderBy { get; set; }

    }
    public enum EnumEventOrderSearchOrderBy
    {
        OrderById,
    }
}