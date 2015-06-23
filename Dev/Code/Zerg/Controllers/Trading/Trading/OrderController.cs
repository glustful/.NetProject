using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Trading.Entity.Model;
using Trading.Service.Product;
using Trading.Service.Classify;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using Trading.Service.CFBBill;
using Trading.Service.LandAgentBill;
using Trading.Service.AgentBill;
using System.Web.Http;
using Zerg.Common;
using Newtonsoft.Json.Linq;
using Zerg.Models.Trading.Product;
using Zerg.Models.Trading.Trading;
using System.Web.Http.Cors;
using System.ComponentModel;


namespace Zerg.Controllers.Trading.Trading.Order
{
    [System.Web.Http.AllowAnonymous]
    //订单推送（Status）：0：未审核；
    //                    1：审核未通过；
    //                    2: 审核已通过；
    //订单类型（OrderType）：0：推荐订单；
    //                      1：成交订单；
    //订单状态(Shipstatus)：0：未看房（推荐）；
    //                      1：已预约（推荐）；
    //                      3：已看房（推荐）；
    //                      4：已付佣金（推荐）；
    //                      5：已买房（成交）；
    //                      6：已付佣金（成交）
    [Description("订单管理类")]
    public class OrderController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        public OrderController(
                IProductService productService,
                IOrderDetailService orderDetailService,
                IOrderService orderService)
        {
            _productService = productService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
        }
        #region 订单管理
        /// <summary>
        /// 生成订单；
        /// </summary>
        /// <param name="jOject">订单参数</param>
        /// <returns>生成结果</returns>
        [Description("生成订单")]
        [System.Web.Http.HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public string AddRecommonOrder([FromBody]JObject jOject)
        {
            try
            {
                dynamic json = jOject;
                JObject jOrder = json.order;
                JObject jOrderDetail = json.orderDetail;
                var orderModel = jOrder.ToObject<OrderModel>();
                var orderDetailModel = jOrderDetail.ToObject<OrderDetailModel>();
                ProductEntity PE = _productService.GetProductById(orderDetailModel.ProductId);
                OrderDetailEntity ODE = new OrderDetailEntity()
                {
                    Adddate = DateTime.Now,
                    Adduser = orderDetailModel.Adduser,
                    Commission = PE.Commission,
                    Dealcommission = PE.Dealcommission,
                    Price = PE.Price,
                    Product = PE,
                    Productname = PE.Productname,
                    Remark = orderDetailModel.Remark,
                    Snapshoturl = orderDetailModel.Snapshoturl,
                    Upddate = DateTime.Now,
                    Upduser = orderDetailModel.Upduser
                };
                OrderDetailEntity ODEResult = _orderDetailService.Create(ODE);//创建订单详情；
                //生成当前订单编号
                Random rd = new Random();
                string OrderNumber = "YJYDD" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(100, 999).ToString();
                OrderEntity OE = new OrderEntity()
                {
                    Adddate = DateTime.Now,
                    Adduser = orderModel.Adduser,
                    AgentId = orderModel.AgentId,
                    Agentname = orderModel.Agentname,
                    Agenttel = orderModel.Agenttel,
                    BusId = PE.Bussnessid,
                    Busname = "YooPoon",
                    Customname = orderModel.Customname,
                    Ordercode = OrderNumber,
                    OrderDetail = ODEResult,
                    //该条字段属性变更，因方法无使用，故注释
                    //Ordertype = orderModel.Ordertype,
                    Remark = orderModel.Remark,
                    Shipstatus = orderModel.Shipstatus,
                    Status = orderModel.Status,
                    Upddate = DateTime.Now,
                    Upduser = orderModel.Adduser
                };
                _orderService.Create(OE);//添加订单；
                return "生成订单成功";
            }
            catch (Exception e)
            {
                return "生成订单失败";
            }
        }
        #endregion

        #region 订单修改
        /// <summary>
        /// 修改订单流程状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="shipStatus">流程状态</param>
        /// <returns>订单流程状态</returns>
        [Description("修改订单流程状态")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public string EditOrderShipstatus(int orderId, int shipStatus)
        {
            try
            {
                OrderEntity OE = _orderService.GetOrderById(orderId);
                OE.Shipstatus = shipStatus;
                _orderService.Update(OE);
                return "修改流程状态成功";
            }
            catch (Exception e)
            {
                return "修改流程状态失败";
            }

        }
        /// <summary>
        /// 修改订单推送状态；
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="status">订单状态</param>
        /// <returns>推送结果</returns>
        [Description("修改订单推送状态")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public string EditOrderStatus(int orderId, int status)
        {
            try
            {
                OrderEntity OE = _orderService.GetOrderById(orderId);
                OE.Status = status;
                _orderService.Update(OE);
                return "修改推送状态成功";
            }
            catch (Exception e)
            {
                return "修改推送状态失败";
            }

        }
        #endregion

        #region 订单查询

        /// <summary>
        /// 查询所有推荐订单；       //原方法变更为根据传入类型返回订单列表——杨定鹏——2015年6月10日10:42:49
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="type">类型</param>
        /// <returns>推荐订单列表</returns>
        [Description("查询所有推荐订单")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAllRecommonOrders(EnumOrderType type)
        {
            OrderSearchCondition OSC = new OrderSearchCondition()
            {
                Ordertype = type,
                OrderBy = EnumOrderSearchOrderBy.OrderById
            };
            var list = _orderService.GetOrdersByCondition(OSC).Select(p => new
            {
                p.Ordercode,
                p.Ordertype,
                p.Status,
                p.Shipstatus,
                p.Agentname,
                p.Agenttel,
                p.Busname,
                p.OrderDetail.Price,
                p.OrderDetail.RecCommission,
                p.OrderDetail.Commission,
                p.OrderDetail.Dealcommission
            }).ToList();
            return PageHelper.toJson(list);
        }
        /// <summary>
        /// 获取洽谈后的订单
        /// </summary>
        /// <returns>订单列表</returns>
        [Description("获取洽谈后的订单")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetOrders()
        {
            var shipStatus = 3;
            var status = 1;
            OrderSearchCondition OSC = new OrderSearchCondition()
            {
                Shipstatus = shipStatus,
                Status = status
            };
            var OrderList = _orderService.GetOrdersByCondition(OSC).Select(a => new
            {
                a.Ordercode,
                a.Ordertype,
                a.Status,
                a.Shipstatus,
                a.Agentname,
                a.Agenttel,
                a.Busname,
                a.OrderDetail.Price,
                a.OrderDetail.RecCommission,
                a.OrderDetail.Commission,
                a.OrderDetail.Dealcommission
            }).ToList();
            return PageHelper.toJson(OrderList);

        }

        /// <summary>
        /// 获取洽谈后的订单
        /// </summary>
        /// <returns>成交订单列表</returns>
        [Description("获取洽谈后的订单")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAllDealOrders()
        {
            OrderSearchCondition OSC = new OrderSearchCondition()
            {
                Ordertype = EnumOrderType.成交订单,
                OrderBy = EnumOrderSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_orderService.GetOrdersByCondition(OSC).ToList());
        }
        /// <summary>
        /// 按照审核的结果查找订单
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>订单列表</returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetOrdersByStatus(int status)
        {
            OrderSearchCondition OSC = new OrderSearchCondition()
            {
                Status = status,
                OrderBy = EnumOrderSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_orderService.GetOrdersByCondition(OSC).ToList());
        }
        #endregion
    }
}
