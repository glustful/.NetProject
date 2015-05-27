using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Trading
{
    public class OrderDetailModel
    {
        /// <summary>
        /// 订单明细ID
        /// </summary>
        public  int Id { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public  int ProductId { get; set; }
        /// <summary>
        /// 商品名字
        /// </summary>
        public  string Productname { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public  decimal Price { get; set; }
        /// <summary>
        /// 带客佣金
        /// </summary>
        public  decimal Commission { get; set; }
        /// <summary>
        /// 成交佣金
        /// </summary>
        public decimal Dealcommission { get; set; }
        /// <summary>
        /// 商品页面快照
        /// </summary>
        public  string Snapshoturl { get; set; }
        /// <summary>
        /// 商品备注
        /// </summary>
        public  string Remark { get; set; }
        /// <summary>
        /// 添加人员
        /// </summary>
        public  string Adduser { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public  DateTime Adddate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public  string Upduser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public  DateTime Upddate { get; set; }
    }
}