using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BrokeAccount;
using CRM.Service.Broker;
using CRM.Service.PartnerList;
using Trading.Entity.Entity.CommissionRatio;
using Trading.Entity.Model;
using Trading.Service.AgentBill;
using Trading.Service.CFBBill;
using Trading.Service.CommissionRatio;
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
        private readonly ICommissionRatioService _commissionRatioService;
        private readonly IBrokeAccountService _brokeAccountService;

        /// <summary>
        /// 账单管理初始化
        /// </summary>
        /// <param name="productService">productService</param>
        /// <param name="orderDetailService">orderDetailService</param>
        /// <param name="orderService">orderService</param>
        /// <param name="brokerService"></param>
        /// <param name="agentBillService">agentBillService</param>
        /// <param name="cfbBillService">CFBBillService</param>
        /// <param name="partnerlistService"></param>
        /// <param name="workContext"></param>
        /// <param name="landAgentBillService">landAgentBillService</param>
        /// <param name="commissionRatioService"></param>
        /// <param name="brokeAccountService"></param>
        public BillController(
                IProductService productService,
                IOrderDetailService orderDetailService,
                IOrderService orderService,
            IBrokerService brokerService,
            IAgentBillService agentBillService,
            ICFBBillService cfbBillService,
            IPartnerListService partnerlistService,
            IWorkContext workContext,
            ILandAgentBillService landAgentBillService,
            ICommissionRatioService commissionRatioService, IBrokeAccountService brokeAccountService)
        {
            _productService = productService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _brokerService = brokerService;
            _agentBillService = agentBillService;
            _CFBBillService = cfbBillService;
            _partnerlistService = partnerlistService;
            _workContext = workContext;
            _landAgentBillService = landAgentBillService;
            _commissionRatioService = commissionRatioService;
            _brokeAccountService = brokeAccountService;

        }
        #region 账单创建管理；
        /// <summary>
        /// 创建三个账单（zerg、经纪人、地产商）及账户明细；
        /// </summary>
        /// <param name="model">账单对象</param>
        /// <returns>账单创建结果状态信息</returns>
        [Description("创建三个账单（zerg、经纪人、地产商")]
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        //===================================================================pengguifei start========================================================================//
        public HttpResponseMessage CreateBill(BillModel model)
        {
            OrderEntity oe = _orderService.GetOrderById(model.orderId);
            var broker = _brokerService.GetBrokerById(oe.AgentId);
            var newAmount = GetCommission(oe);
            if (newAmount == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "不存在佣金分成，账单无法生成"));
            }         
            var partner = GetPartner(broker.UserId);
            //创富宝平台账单
            var CBE = new CFBBillEntity
            {
                Actualamount = model.Actualamount,
                Amount = newAmount.CFBamount,
                AgentId = oe.AgentId, //经纪人Id；
                Agentname = oe.Agentname, //经纪人名字；
                LandagentId = oe.BusId, //地产商Id；
                Landagentname = oe.Busname, //地产商名字；
                Beneficiary = oe.Agentname,
                Beneficiarynumber = model.beneficiarynumber,
                Cardnumber = model.beneficiarynumber,
                Checkoutdate = DateTime.Now,
                Customname = oe.Agentname,
                Isinvoice = false,
                Order = oe,
                Remark = model.remark,
                Addtime = DateTime.Now,
                Adduser = _workContext.CurrentUser.Id.ToString(),
                Updtime = DateTime.Now,
                Upduser = _workContext.CurrentUser.Id.ToString()
            };
            //地产商账单
            var LABE = new LandAgentBillEntity
            {
                Actualamount = null,
                Amount = newAmount.LandAgentamount,
                AgentId = oe.AgentId, //经纪人Id；
                Agentname = oe.Agentname, //经纪人名字；
                LandagentId = oe.BusId, //地产商Id；
                Landagentname = oe.Busname, //地产商名字；
                Beneficiary = oe.Agentname,
                Beneficiarynumber = null,
                Cardnumber = null,
                Checkoutdate = DateTime.Now,
                Customname = oe.Agentname,
                Isinvoice = false,
                Order = oe,
                Remark = model.remark,
                Addtime = DateTime.Now,
                Adduser = _workContext.CurrentUser.Id.ToString(),
                Updtime = DateTime.Now,
                Upduser = _workContext.CurrentUser.Id.ToString()
            };
            //经济人账单                           
            var ABE = new AgentBillEntity
            {
                Actualamount = null,
                Amount = newAmount.Agentamount,
                AgentId = oe.AgentId, //经纪人Id；
                Agentname = oe.Agentname, //经纪人名字；
                LandagentId = oe.BusId, //地产商Id；
                Landagentname = oe.Busname, //地产商名字；
                Beneficiary = oe.Agentname,
                Beneficiarynumber = null,
                Cardnumber = null,
                Checkoutdate = DateTime.Now,
                Customname = oe.Agentname,
                Isinvoice = false,
                Order = oe,
                Remark = model.remark,
                Addtime = DateTime.Now,
                Adduser = _workContext.CurrentUser.Id.ToString(),
                Updtime = DateTime.Now,
                Upduser = _workContext.CurrentUser.Id.ToString()
            };
            AgentBillEntity PBE = null;          
            BrokeAccountEntity BAE = null, PAE = null;
            //经济人账户明细
            switch (oe.Ordertype)
            {
                case EnumOrderType.带客订单:
                    BAE = new BrokeAccountEntity
                    {
                        Balancenum = newAmount.Agentamount,
                        Broker = broker,
                        Type = 0,
                        MoneyDesc = model.MoneyDesc,
                        Adduser = _workContext.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        Upuser = _workContext.CurrentUser.Id,
                        Uptime = DateTime.Now
                    };
                    break;
                case EnumOrderType.推荐订单:
                    BAE = new BrokeAccountEntity
                    {
                        Balancenum = newAmount.Agentamount,
                        Broker = broker,
                        Type = 1,
                        MoneyDesc = model.MoneyDesc,
                        Adduser = _workContext.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        Upuser = _workContext.CurrentUser.Id,
                        Uptime = DateTime.Now
                    };
                    break;
            }
            //if (oe.Ordertype == EnumOrderType.带客订单)
            //{
            //    BAE = new BrokeAccountEntity
            //    {
            //        Balancenum = newAmount.Agentamount,
            //        Broker = broker,
            //        Type = 0,
            //        MoneyDesc = model.MoneyDesc,
            //        Adduser = _workContext.CurrentUser.Id,
            //        Addtime = DateTime.Now,
            //        Upuser = _workContext.CurrentUser.Id,
            //        Uptime = DateTime.Now
            //    };
            //}
            //else
            //{
            //    BAE = new BrokeAccountEntity
            //    {
            //        Balancenum = newAmount.Agentamount,
            //        Broker = broker,
            //        Type = 1,
            //        MoneyDesc = model.MoneyDesc,
            //        Adduser = _workContext.CurrentUser.Id,
            //        Addtime = DateTime.Now,
            //        Upuser = _workContext.CurrentUser.Id,
            //        Uptime = DateTime.Now
            //    };
            //}
            //成交并且有合伙人时创建合伙人账单和账户明细
            if (oe.Shipstatus == 3 &&partner != null)
            {
                //合伙人账单
                PBE = new AgentBillEntity
                {
                    Actualamount = null,
                    Amount = newAmount.Partneramount,
                    AgentId = partner.PartnersId, //经纪人Id；
                    Agentname = partner.Brokername, //经纪人名字；
                    LandagentId = oe.BusId, //地产商Id；
                    Landagentname = oe.Busname, //地产商名字；
                    Beneficiary = partner.Brokername,
                    Beneficiarynumber = null,
                    Cardnumber = null,
                    Checkoutdate = DateTime.Now,
                    Customname = oe.Agentname,
                    Isinvoice = false,
                    Order = oe,
                    Remark = model.remark,
                    Addtime = DateTime.Now,
                    Adduser = _workContext.CurrentUser.Id.ToString(),
                    Updtime = DateTime.Now,
                    Upduser = _workContext.CurrentUser.Id.ToString()
                };
                //合伙人账户明细
                switch (oe.Ordertype)
                {
                        case EnumOrderType.带客订单:
                        PAE = new BrokeAccountEntity
                        {
                            Balancenum = newAmount.Partneramount,
                            Broker = _brokerService.GetBrokerById(partner.PartnersId),
                            Type = 0,
                            MoneyDesc = model.MoneyDesc,
                            Adduser = _workContext.CurrentUser.Id,
                            Addtime = DateTime.Now,
                            Upuser = _workContext.CurrentUser.Id,
                            Uptime = DateTime.Now
                        };
                        break;
                        case EnumOrderType.推荐订单:
                        PAE = new BrokeAccountEntity
                        {
                            Balancenum = newAmount.Partneramount,
                            Broker = _brokerService.GetBrokerById(partner.PartnersId),
                            Type = 1,
                            MoneyDesc = model.MoneyDesc,
                            Adduser = _workContext.CurrentUser.Id,
                            Addtime = DateTime.Now,
                            Upuser = _workContext.CurrentUser.Id,
                            Uptime = DateTime.Now
                        };
                        break;
                }
                _agentBillService.Create(PBE);
                _brokeAccountService.Create(PAE);
                //if (oe.Ordertype == EnumOrderType.带客订单)
                //{
                //    PAE = new BrokeAccountEntity
                //    {
                //        Balancenum = newAmount.Partneramount,
                //        Broker = _brokerService.GetBrokerById(partner.PartnersId),
                //        Type = 0,
                //        MoneyDesc = model.MoneyDesc,
                //        Adduser = _workContext.CurrentUser.Id,
                //        Addtime = DateTime.Now,
                //        Upuser = _workContext.CurrentUser.Id,
                //        Uptime = DateTime.Now
                //    };
                //}
                //else
                //{                  
                //    PAE = new BrokeAccountEntity
                //    {
                //        Balancenum = newAmount.Partneramount,
                //        Broker = _brokerService.GetBrokerById(partner.PartnersId),
                //        Type = 1,
                //        MoneyDesc = model.MoneyDesc,
                //        Adduser = _workContext.CurrentUser.Id,
                //        Addtime = DateTime.Now,
                //        Upuser = _workContext.CurrentUser.Id,
                //        Uptime = DateTime.Now
                //    };
                //}
            }
            //broker.Amount = broker.Amount + BAE.Balancenum;
            //broker.Uptime=DateTime.Now;
            //broker.Upuser = _workContext.CurrentUser.Id;
            //_brokerService.Update(broker);
            _CFBBillService.Create(CBE);
            _landAgentBillService.Create(LABE);
            _agentBillService.Create(ABE);
            _brokeAccountService.Create(BAE);
            //成交并且有合伙人时创建合伙人账单和账户明细
            //if (oe.Shipstatus == 3 && partner != null)
            //{
            //    _agentBillService.Create(PBE);
            //    _brokeAccountService.Create(PAE);
            //}
            return PageHelper.toJson(PageHelper.ReturnValue(true, "账单生成成功"));
        }
        /// <summary>
        /// 获取合伙人
        /// </summary>
        /// <param name="brokerUserId">经济人UserId</param>
        /// <returns></returns>
        public BrokerModel GetPartner(int brokerUserId)
        {
            var partnerlistsearchcon = new PartnerListSearchCondition
            {
                Brokers = _brokerService.GetBrokerByUserId(brokerUserId),
                Status = EnumPartnerType.同意
            };
            var partner =
                _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Select(p => new BrokerModel
                {
                    Id = p.Id,
                    Brokername = p.Brokername,
                    PartnersId = p.PartnerId
                }).FirstOrDefault();
            return partner;
        }
        /// <summary>
        /// 计算佣金
        /// </summary>
        /// <param name="model">订单对象</param>
        /// <returns></returns>
        public AmountModel GetCommission(OrderEntity model)
        {
            var con = new CommissionRatioSearchCondition
            {
                Page = 1,
                PageSize = 1
            };
            //获取佣金分成比例
            var commissionRatio =
                _commissionRatioService.GetCommissionRatioCondition(con)
                    .Select(p => new Models.Trading.CommissionRatio.CommissionRatio
                    {
                        Id = p.Id,
                        RecCfbScale = p.RecCfbScale,
                        RecAgentScale = p.RecAgentScale,
                        TakeCfbScale = p.TakeCfbScale,
                        TakeAgentScale = p.TakeAgentScale,
                        RecPartnerScale = p.RecPartnerScale,
                        TakePartnerScale = p.TakePartnerScale
                    }).FirstOrDefault();
            if (commissionRatio == null) return null;
            var ode = model.OrderDetail;
            var amount = new AmountModel();
            var broker = _brokerService.GetBrokerById(model.AgentId);              
            if (GetPartner(broker.UserId) == null)
            {
                switch (model.Ordertype)
                {
                    case EnumOrderType.推荐订单:
                        //如果是推荐订单；                   
                        if (model.Shipstatus == 3)
                        {
                            amount.CFBamount = ode.Dealcommission * commissionRatio.RecCfbScale;
                            amount.Agentamount = ode.Dealcommission * commissionRatio.RecAgentScale + ode.RecCommission;
                        }
                        else
                        {
                            amount.Agentamount = ode.RecCommission;
                        }
                        break;
                    case EnumOrderType.带客订单:
                        if (model.Shipstatus == 3)
                        {
                            amount.CFBamount = ode.Dealcommission * commissionRatio.TakeCfbScale;
                            amount.Agentamount = ode.Dealcommission * commissionRatio.TakeAgentScale + ode.Commission;
                        }
                        else
                        {
                            amount.Agentamount = ode.Commission;
                        }
                        break;
                }
            }
            else
            {
                switch (model.Ordertype)
                {
                    case EnumOrderType.推荐订单:
                        //如果是推荐订单；                   
                        if (model.Shipstatus == 3)
                        {
                            amount.CFBamount = ode.Dealcommission * commissionRatio.RecCfbScale;
                            amount.Agentamount = (ode.Dealcommission * commissionRatio.RecAgentScale - ode.Dealcommission * commissionRatio.RecAgentScale *
                                                  commissionRatio.RecPartnerScale) + ode.RecCommission;
                            amount.Partneramount = ode.Dealcommission * commissionRatio.RecAgentScale *
                                                   commissionRatio.RecPartnerScale;
                        }
                        else
                        {
                            amount.Agentamount = ode.RecCommission;
                        }
                        break;
                    case EnumOrderType.带客订单:
                        if (model.Shipstatus == 3)
                        {
                            amount.CFBamount = ode.Dealcommission * commissionRatio.TakeCfbScale;
                            amount.Agentamount = (ode.Dealcommission * commissionRatio.TakeAgentScale - ode.Dealcommission * commissionRatio.TakeAgentScale *
                                                  commissionRatio.TakePartnerScale) + ode.Commission;
                            amount.Partneramount = ode.Dealcommission * commissionRatio.TakeAgentScale *
                                                   commissionRatio.TakePartnerScale;
                        }
                        else
                        {
                            amount.Agentamount = ode.Commission;
                        }
                        break;
                }
            }
            return amount;
        }
        //========================================================================================end=====================================================================//
        /*public HttpResponseMessage CreateBillsByOrder(BillModel model)
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
         }*/
        #endregion

        #region 查询账单
        /// <summary>
        /// 查询所有Admin账单；
        /// </summary>
        /// <returns>账单详细信息</returns>
        [Description("查询账单")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAdminBill(int page, int pageSize)
        {
            CFBBillSearchCondition CFBSC = new CFBBillSearchCondition
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
            return PageHelper.toJson(new { AdminBill = adminBill, BillCount = billCount, Condition = CFBSC });
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
            AgentBillSearchCondition CFBSC = new AgentBillSearchCondition
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
            LandAgentBillSearchCondition LABSC = new LandAgentBillSearchCondition
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
            AgentBillSearchCondition ABSC = new AgentBillSearchCondition
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
            LandAgentBillSearchCondition LABSC = new LandAgentBillSearchCondition
            {
                LandagentId = LandagentId,
                OrderBy = EnumLandAgentBillSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_landAgentBillService.GetLandAgentBillsByCondition(LABSC).ToList());
        }
        #endregion
    }
}
