using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
    public class BrokerWithdrawEntity : IBaseEntity
    {
        /// <summary>
        /// 提现ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 经纪人ID
        /// </summary>
        public virtual BrokerEntity Broker { get; set; }
        /// <summary>
        /// 银行卡ID
        /// </summary>
        public virtual BankCardEntity BankCard { get; set; }
        /// <summary>
        /// 财务 ID
        /// </summary>
        public virtual BrokerEntity AccAccountantId { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public virtual DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 提现总金额
        /// </summary>
        public virtual decimal WithdrawTotalNum { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public virtual int Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public virtual DateTime Addtime { get; set; }
        /// <summary>
        /// UpUser
        /// </summary>
        public virtual int Upuser { get; set; }
        /// <summary>
        /// UpTime
        /// </summary>
        public virtual DateTime Uptime { get; set; }

        /// <summary>
        /// 提现状态 ｛0，处理中   1完成 ok｝
        /// </summary>
        public virtual int State { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string WithdrawDesc { get; set; }
    }
}
