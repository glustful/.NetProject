using System.Globalization;
using System.Web.Mvc;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Service.ClientInfo;
using Newtonsoft.Json.Linq;
using Trading.Entity.Model;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using Trading.Service.Product;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Controllers.Trading.Trading.Order;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [System.Web.Http.AllowAnonymous]
     [EnableCors("*", "*", "*", SupportsCredentials = true)]

     //经纪人推荐客户
    public class BrokerRECClientController : ApiController
    {
         public readonly IBrokerRECClientService BrokerRecClientService;
         private readonly IBrokerService _brokerService;//经纪人
         private readonly IClientInfoService _clientInfoService;
         private readonly IWorkContext _workContext;
         private readonly IProductService _productService;
         private readonly IOrderDetailService _orderDetailService;
         private readonly IOrderService _orderService;

         public BrokerRECClientController(
             IBrokerRECClientService brokerRecClientService, 
             IBrokerService brokerService,
             IClientInfoService clientInfoService,
             IWorkContext workContext,
             IProductService productService,
             IOrderDetailService orderDetailService,
             IOrderService orderService

             )
         {
             BrokerRecClientService = brokerRecClientService;
             _brokerService = brokerService;
             _clientInfoService = clientInfoService;
             _workContext = workContext;
             _productService = productService;
             _orderDetailService = orderDetailService;
             _orderService = orderService;
         }




         /// <summary>
         /// 查询某经纪人推荐的客户
         /// </summary>
         /// <param name="userid"></param>
         /// <returns></returns>
         [System.Web.Http.HttpGet]
         public HttpResponseMessage SearchBrokerRecClient(string userid)
         {
             var p = new BrokerRECClientSearchCondition
             {
                 Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userid))
             };
             var list = BrokerRecClientService.GetBrokerRECClientsByCondition(p).ToList();
             return PageHelper.toJson(list);

         }


         /// <summary>
         /// 推荐一个
         /// </summary>
         /// <param name="brokerrecclient"></param>
         /// <returns></returns>
         [System.Web.Http.HttpPost]
         public HttpResponseMessage Add([FromBody]  BrokerRECClientModel  brokerrecclient)
         {
             EnumBRECCType type;
             //查询客户信息
             var sech = new ClientInfoSearchCondition
             {
                 Clientname = brokerrecclient.Clientname,
                 Phone = brokerrecclient.Phone.ToString(CultureInfo.InvariantCulture)

             };

             var cmodel = _clientInfoService.GetClientInfosByCondition(sech).FirstOrDefault();

             if (cmodel == null)
             {
                 //客户信息
                 var client = new ClientInfoEntity
                 {
                     Clientname = brokerrecclient.Clientname,
                     Phone = brokerrecclient.Phone.ToString(CultureInfo.InvariantCulture),
                     Housetype = brokerrecclient.HouseType,
                     Houses = brokerrecclient.Houses,
                     Note = brokerrecclient.Note,
                     Adduser = brokerrecclient.Broker,
                     Addtime = DateTime.Now,
                     Upuser = brokerrecclient.Broker,
                     Uptime = DateTime.Now
                 };


                 _clientInfoService.Create(client);

                 type = EnumBRECCType.审核中;
             }
             else
             {
                 type = BrokerRecClientService.GetBrokerRECClientById(cmodel.Id).Status;
             }

             //检测
             if (type == EnumBRECCType.等待上访) return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));

             #region 创建订单 杨定鹏 2015年6月3日17:21:39

             //创建订单号
             var num = _orderService.CreateOrderNumber();

             //查询商品详情
             var product = _productService.GetProductById(1);

             //创建订单详情
             OrderDetailEntity ode = new OrderDetailEntity();
             ode.Adddate = DateTime.Now;
             ode.Adduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture),
             ode.Commission = product.Commission;
             ode.RecCommission = product.RecCommission;
             ode.Dealcommission = product.Dealcommission;
             ode.Price = product.Price;
             ode.Product = product;
             ode.Productname = product.Productname;
                 //ode.Remark = product.
                 //ode.Snapshoturl = orderDetailModel.Snapshoturl,
             ode.Upddate = DateTime.Now;
             ode.Upduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture)

             //创建订单
             OrderEntity oe = new OrderEntity
             {
                 Adddate = DateTime.Now,
                 Adduser ="2", //_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture),
                 AgentId = 2, //_workContext.CurrentUser.Id,
                 Agentname = brokerrecclient.Brokername,
                 Agenttel = brokerrecclient.Phone,
                 BusId = product.Bussnessid,
                 Busname = "YooPoon",
                 Customname = brokerrecclient.Clientname,
                 Ordercode = num,
                 OrderDetail = _orderDetailService.Create(ode),//创建订单详情；
                 Ordertype = EnumOrderType.推荐订单,
                 Remark = "前端经纪人提交",
                 Shipstatus = (int)EnumBRECCType.审核中,
                 Status = (int)EnumOrderStatus.默认,
                 Upddate = DateTime.Now,
                 Upduser ="2" //_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture)
             };

             //创建成交订单
             var num2 = _orderService.CreateOrderNumber();
             OrderDetailEntity ode2 = ode;
             OrderEntity oe2 = oe;
             oe2.OrderDetail = ode2;
             oe2.Ordercode = num2;
             oe2.Ordertype=EnumOrderType.成交订单;
                 
             #endregion

             cmodel = _clientInfoService.GetClientInfosByCondition(sech).First();

             //创建推荐流程
             var model = new BrokerRECClientEntity
             {
                 Broker = _brokerService.GetBrokerById(2),
                 ClientInfo = cmodel,
                 Adduser =2, //_workContext.CurrentUser.Id,
                 Addtime = DateTime.Now,
                 Upuser =2, //_workContext.CurrentUser.Id,
                 Uptime = DateTime.Now,
                 Projectid = brokerrecclient.Projectid,
                 Status = EnumBRECCType.等待上访,
                 RecOrder = _orderService.Create(oe).Id,        //添加推荐订单；
                 DealOrder = _orderService.Create(oe2).Id,       //添加成交订单
             };

             BrokerRecClientService.Create(model);

             return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
         }
    }
}
