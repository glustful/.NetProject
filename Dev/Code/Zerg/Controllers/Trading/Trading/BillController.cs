using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PartnerList;
using Trading.Entity.Model;
using Trading.Service.AgentBill;
using Trading.Service.CFBBill;
using Trading.Service.LandAgentBill;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using Trading.Service.Product;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.CRM;
using Zerg.Models.Trading.Trading;

namespace Zerg.Controllers.Trading.Trading
{
     [AllowAnonymous]
    [Description("账单管理类")]
    public class BillController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IAgentBillService _agentBillService;
        private readonly ICFBBillService _CFBBillService;
        private readonly ILandAgentBillService _landAgentBillService;
         private readonly IWorkContext _workContext;
         private readonly IBrokerService _brokerService;
         private readonly IPartnerListService _partnerlistService;

         /// <summary>
        /// 账单管理初始化
        /// </summary>
        /// <param name="productService">productService</param>
        /// <param name="orderDetailService">orderDetailService</param>
        /// <param name="orderService">orderService</param>
        /// <param name="agentBillService">agentBillService</param>
        /// <param name="CFBBillService">CFBBillService</param>
        /// <param name="landAgentBillService">landAgentBillService</param>
        public BillController(
                IProductService productService,
                IOrderDetailService orderDetailService,
                IOrderService orderService,
            IBrokerService brokerService,
            IAgentBillService agentBillService,
            ICFBBillService CFBBillService,
            IPartnerListService partnerlistService,
            IWorkContext workContext,
            ILandAgentBillService landAgentBillService)
        {
            _productService = productService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
             _brokerService = brokerService;
            _agentBillService = agentBillService;
            _CFBBillService = CFBBillService;
             _partnerlistService = partnerlistService;
            _workContext = workContext;
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
        /// <returns>账单创建结果状态信息</returns>
        [Description("创建三个账单（zerg、经纪人、地产商")]
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage CreateBillsByOrder(BillModel model)
        { 
           
           // try
            //{   
            OrderEntity OE = _orderService.GetOrderById(model.orderId);
            var Broker = _brokerService.GetBrokerById(OE.AgentId);
            var partnerlistsearchcon = new PartnerListSearchCondition
            {
                Brokers = _brokerService.GetBrokerByUserId(Broker.UserId),
                Status = EnumPartnerType.同意
            };
            var partner = _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Select(p => new BrokerModel
            {
                Id = p.Id,
                Brokername = p.Brokername
            }).FirstOrDefault();
            if (partner == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "合伙人不存在"));
            }
            else
            {
                OrderDetailEntity ODE = OE.OrderDetail;
                decimal CFBamount = 0, LandAgentamount = 0, Agentamount = 0, Partneramount = 0;
                if (OE.Ordertype == EnumOrderType.推荐订单)
                {
                   //如果是推荐订单；                   
                    if (OE.Shipstatus == 3)
                    {
                        CFBamount = ODE.Dealcommission*(decimal) 0.7;
                        Agentamount = ODE.Dealcommission*(decimal) 0.27 + ODE.RecCommission;
                        Partneramount = ODE.Dealcommission*(decimal) 0.03;
                    }
                    else
                    {
                        Agentamount = ODE.RecCommission;
                    }
                }
                else if (OE.Ordertype == EnumOrderType.带客订单) //如果是带客订单；
                {

                    if (OE.Shipstatus == 3)
                    {
                        CFBamount = ODE.Dealcommission*(decimal) 0.3;
                        Agentamount = ODE.Dealcommission*(decimal) 0.63 + ODE.Commission;
                        Partneramount = ODE.Dealcommission*(decimal) 0.07;
                    }
                    else
                    {
                        Agentamount = ODE.Commission;
                    }
                }
                //创富宝平台账单
                CFBBillEntity CBE = new CFBBillEntity()
                {
                    Actualamount = model.Actualamount,
                    Amount = CFBamount,
                    AgentId = OE.AgentId, //经纪人Id；
                    Agentname = OE.Agentname, //经纪人名字；
                    LandagentId = OE.BusId, //地产商Id；
                    Landagentname = OE.Busname, //地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = model.beneficiarynumber,
                    Cardnumber = model.beneficiarynumber,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = model.remark,
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString()
                };
                //地产商账单
                LandAgentBillEntity LABE = new LandAgentBillEntity()
                {
                    Actualamount = null,
                    Amount = LandAgentamount,
                    AgentId = OE.AgentId, //经纪人Id；
                    Agentname = OE.Agentname, //经纪人名字；
                    LandagentId = OE.BusId, //地产商Id；
                    Landagentname = OE.Busname, //地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = null,
                    Cardnumber = null,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = null,
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString()
                };
                //经济人账单                           
                AgentBillEntity ABE = new AgentBillEntity()
                {
                    Actualamount = null,
                    Amount = Agentamount,
                    AgentId = OE.AgentId, //经纪人Id；
                    Agentname = OE.Agentname, //经纪人名字；
                    LandagentId = OE.BusId, //地产商Id；
                    Landagentname = OE.Busname, //地产商名字；
                    Beneficiary = OE.Agentname,
                    Beneficiarynumber = null,
                    Cardnumber = null,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = null,
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString()
                };
                //合伙人账单
                AgentBillEntity PBE = new AgentBillEntity()
                {
                    Actualamount = null,
                    Amount = Partneramount,
                    AgentId = partner.Id, //经纪人Id；
                    Agentname = partner.Brokername, //经纪人名字；
                    LandagentId = OE.BusId, //地产商Id；
                    Landagentname = OE.Busname, //地产商名字；
                    Beneficiary = partner.Brokername,
                    Beneficiarynumber = null,
                    Cardnumber = null,
                    Checkoutdate = DateTime.Now,
                    Customname = OE.Agentname,
                    Isinvoice = false,
                    Order = OE,
                    Remark = null,
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString()
                };
                if (OE.Shipstatus == 3)
                {
                    _CFBBillService.Create(CBE);
                    _landAgentBillService.Create(LABE);
                    _agentBillService.Create(ABE);
                    _agentBillService.Create(PBE);
                }
                else
                {
                    _agentBillService.Create(ABE);
                }
                return PageHelper.toJson(PageHelper.ReturnValue(true, "账单生成成功"));
            }
            //}
            //catch (Exception e)
            //{
            //    return PageHelper.toJson(PageHelper.ReturnValue(false,"账单生成失败"));
            //}
        }
        #endregion

        #region 查询账单
        /// <summary>
        /// 查询所有Admin账单；
        /// </summary>
        /// <returns>账单详细信息</returns>
        [Description("查询账单")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAdminBill(int page,int pageSize)
        {
            CFBBillSearchCondition CFBSC = new CFBBillSearchCondition()
            {
                Page = page,
                PageCount = pageSize,
                OrderBy = EnumCFBBillSearchOrderBy.OrderById
            };
            var adminBill = _CFBBillService.GetCFBBillsByCondition(CFBSC).Select(p => new
            {
                p.Id,
                p.Agentname,
                p.Landagentname,
                p.Amount,
                p.Isinvoice,
                p.Remark,
                p.Checkoutdate
            }).ToList();
            var billCount = _CFBBillService.GetCFBBillCount(CFBSC);
            return PageHelper.toJson(new{AdminBill=adminBill,BillCount=billCount,Condition=CFBSC});
        }
        /// <summary>
        /// 查询所有经纪人账单；
        /// </summary>
        /// <returns>经纪人账单详情</returns>
        [Description("查询所有经纪人账单")]
        [HttpGet]
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
        /// <returns>地产商账单</returns>
        [Description(" 查询所有地产商账单")]
        [HttpGet]
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
        /// <returns>所有经纪人账单</returns>
        [Description("查询经纪人Id所有经纪人账单")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAgentBillById(int agentId)
        {
            AgentBillSearchCondition ABSC = new AgentBillSearchCondition()
            {
                AgentId = agentId,
                OrderBy = EnumAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_agentBillService.GetAgentBillsByCondition(ABSC).ToList());
        }
        /// <summary>
        /// 根据地产商ID查询所有账单；
        /// </summary>
        /// <returns>地产商Id对应的所有账单</returns>
        [Description(" 根据地产商ID查询所有账单")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetLandAgentBillById(int LandagentId)
        {
            LandAgentBillSearchCondition LABSC = new LandAgentBillSearchCondition()
            {
                LandagentId = LandagentId,
                OrderBy = EnumLandAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_landAgentBillService.GetLandAgentBillsByCondition(LABSC).ToList());
        }
        #endregion
    }
}
