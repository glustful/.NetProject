using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.CommissionRatio
{
    public class CommissionRatio
    {
        public int Id { get; set; }
        /// <summary>
        /// 推荐平台所占比例
        /// </summary>
        public virtual decimal RecCfbScale { get; set; }
        /// <summary>
        /// 推荐经纪人所占比例
        /// </summary>
        public virtual decimal RecAgentScale { get; set; }
        /// <summary>
        /// 推荐成交合伙人所占比例
        /// </summary>
        public virtual decimal RecPartnerScale { get; set; }
        /// <summary>
        /// 带客平台所占比例
        /// </summary>
        public virtual decimal TakeCfbScale { get; set; }
        /// <summary>
        /// 带客经纪人所占比例
        /// </summary>
        public virtual decimal TakeAgentScale { get; set; }
        /// <summary>
        /// 带客成交合伙人所占比例
        /// </summary>
        public virtual decimal TakePartnerScale { get; set; }
    }
}