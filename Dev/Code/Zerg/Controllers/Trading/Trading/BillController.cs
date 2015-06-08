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
namespace Zerg.Controllers.Trading.Trading
{
    public class BillController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IAgentBillService _agentBillService;
        private readonly ICFBBillService _CFBBillService;
        private readonly ILandAgentBillService _landAgentBillService;
        public BillController(
                IProductService productService,
                IOrderDetailService orderDetailService,
                IOrderService orderService,
            IAgentBillService agentBillService,
            ICFBBillService CFBBillService,
            ILandAgentBillService landAgentBillService)
        {
            _productService = productService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _agentBillService = agentBillService;
            _CFBBillService = CFBBillService;
            _landAgentBillService = landAgentBillService;


        }
        #region 账单创建管理；
        /// <summary>
        /// 创建三个账单（zerg、经纪人、地产商）；
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="beneficiarynumber">打款账号</param>
        /// <param name="remark">备注</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public string CreateBillsByOrder(int orderId, string beneficiarynumber, string remark, string user)
        {
            try
            {
                OrderEntity OE = _orderService.GetOrderById(orderId);
                OrderDetailEntity ODE = OE.OrderDetail;
                decimal amount = 0;
                if (OE.Ordertype == 0)
                {//如果是推荐订单；
                    amount = ODE.Commission;
                }
                else if (OE.Ordertype == EnumOrderType.成交订单)//如果是成交订单；
                {
                    amount = ODE.Dealcommission;
                }
                CFBBillEntity CBE = new CFBBillEntity()
                {
                    Actualamount = amount,
                    Amount = amount,
                    AgentId = OE.AgentId,//经纪人Id；
                    Agentname = OE.Agentname,//经纪人名字；
                    LandagentId = OE.BusId,//地产商Id；
                    Landagentname = OE.Busname,//地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = beneficiarynumber,
                    Cardnumber = beneficiarynumber,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = remark,
                    Addtime = DateTime.Now,
                    Adduser = user,
                    Updtime = DateTime.Now,
                    Upduser = user
                };
                LandAgentBillEntity LABE = new LandAgentBillEntity()
                {
                    Actualamount = amount,
                    Amount = amount,
                    AgentId = OE.AgentId,//经纪人Id；
                    Agentname = OE.Agentname,//经纪人名字；
                    LandagentId = OE.BusId,//地产商Id；
                    Landagentname = OE.Busname,//地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = beneficiarynumber,
                    Cardnumber = beneficiarynumber,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = remark,
                    Addtime = DateTime.Now,
                    Adduser = user,
                    Updtime = DateTime.Now,
                    Upduser = user
                };
                AgentBillEntity ABE = new AgentBillEntity()
                {

                    Actualamount = amount,
                    Amount = amount,
                    AgentId = OE.AgentId,//经纪人Id；
                    Agentname = OE.Agentname,//经纪人名字；
                    LandagentId = OE.BusId,//地产商Id；
                    Landagentname = OE.Busname,//地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = beneficiarynumber,
                    Cardnumber = beneficiarynumber,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = remark,
                    Addtime = DateTime.Now,
                    Adduser = user,
                    Updtime = DateTime.Now,
                    Upduser = user
                };
                _CFBBillService.Create(CBE);
                _landAgentBillService.Create(LABE);
                _agentBillService.Create(ABE);
                return "创建账单成功";
            }
            catch (Exception e)
            {
                return "创建账单失败";
            }
        }
        #endregion

        #region 查询账单
        /// <summary>
        /// 查询所有Admin账单；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAdminBill()
        {
            CFBBillSearchCondition CFBSC = new CFBBillSearchCondition()
            {
                OrderBy = EnumCFBBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_CFBBillService.GetCFBBillsByCondition(CFBSC).ToList());
        }
        /// <summary>
        /// 查询所有经纪人账单；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAgentBill()
        {
            AgentBillSearchCondition CFBSC = new AgentBillSearchCondition()
            {
                OrderBy = EnumAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_agentBillService.GetAgentBillsByCondition(CFBSC).ToList());
        }
        /// <summary>
        /// 查询所有地产商账单；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetLandAgentBill()
        {
            LandAgentBillSearchCondition LABSC = new LandAgentBillSearchCondition()
            {
                OrderBy = EnumLandAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_landAgentBillService.GetLandAgentBillsByCondition(LABSC).ToList());
        }
        /// <summary>
        /// 查询经纪人Id所有经纪人账单；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetAgentBillById(int agentId)
        {
            AgentBillSearchCondition ABSC = new AgentBillSearchCondition()
            {
                AgentId=agentId,
                OrderBy = EnumAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_agentBillService.GetAgentBillsByCondition(ABSC).ToList());
        }
        /// <summary>
        /// 根据地产商ID查询所有账单；
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)] 
        public HttpResponseMessage GetLandAgentBillById(int LandagentId)
        {
            LandAgentBillSearchCondition LABSC = new LandAgentBillSearchCondition()
            {
                LandagentId=LandagentId,
                OrderBy = EnumLandAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_landAgentBillService.GetLandAgentBillsByCondition(LABSC).ToList());
        }
        #endregion
    }
}
