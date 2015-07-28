using System;

namespace CRM.Entity.Model
{
    public class EventSearchCondition
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
        public bool IsDescending { get; set; }
        public int Id { get; set; }

        public BrokerEntity Brokers { get; set; }

        public DateTime? Starttime { get; set; }

        public DateTime? Endtime { get; set; }

        public string EventContent { get; set; }
        public string ActionControllers { get; set; }


        public EnumEventSearchOrderBy? OrderBy { get; set; }

    }
    public enum EnumEventSearchOrderBy
    {
        OrderById,
    }
}