using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Trading
{
    public class OrderModel
    {
        public  int OrderDetailId { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>
        public  string Ordercode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public  int Ordertype { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        public  int Shipstatus { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public  int Status { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        public  int BusId { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public  string Busname { get; set; }
        /// <summary>
        /// 经纪人ID
        /// </summary>
        public  int AgentId { get; set; }
        /// <summary>
        /// 经纪人名称
        /// </summary>
        public  string Agentname { get; set; }
        /// <summary>
        /// 经纪人tel
        /// </summary>
        public  string Agenttel { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public  string Customname { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        public  string Remark { get; set; }
        /// <summary>
        /// 开单时间
        /// </summary>
        public  DateTime Adddate { get; set; }
        /// <summary>
        /// 开单人员
        /// </summary>
        public  string Adduser { get; set; }
        /// <summary>
        /// 修改人员
        /// </summary>
        public  string Upduser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public  DateTime Upddate { get; set; }
    }
}