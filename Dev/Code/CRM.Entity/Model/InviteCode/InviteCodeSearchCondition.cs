using System;

namespace CRM.Entity.Model
{
    public class InviteCodeSearchCondition
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
        public int ?NumUser { get; set; }
        public BrokerEntity Brokers { get; set; }

        public int? BrokerId { get; set; }

        public DateTime? CreatTime { get; set; }

        public DateTime? UseTime { get; set; }

        public string Number { get; set; }


        public EnumInviteCodeSearchOrderBy? OrderBy { get; set; }

    }
    public enum EnumInviteCodeSearchOrderBy
    {
        OrderById,
    }
}