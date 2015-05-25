using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Entity.Model;

namespace Zerg.Models.CRM
{
    public class AdminPayModel
    {
        /// <summary>
        /// 流程列表ID
        /// </summary>
        public virtual int BRECCId { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// BrokerRECClientId
        /// </summary>
        public virtual int BrokerRECClientId { get; set; }
        /// <summary>
        /// 款项名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 打款阶段
        /// </summary>
        public virtual EnumBRECP Statusname { get; set; }
        /// <summary>
        /// 款项描述
        /// </summary>
        public virtual string Describe { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 财务ID
        /// </summary>
        public virtual int Accountantid { get; set; }
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
        /// 银行卡号
        /// </summary>
        public virtual int BankCard { get; set; }
    }
}