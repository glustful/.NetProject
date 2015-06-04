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
             if (type != EnumBRECCType.等待上访)
             {


                 cmodel = _clientInfoService.GetClientInfosByCondition(sech).First();

                 var model = new BrokerRECClientEntity
                 {
                     Broker = _brokerService.GetBrokerById(_workContext.CurrentUser.Id),
                     ClientInfo = cmodel,
                     Adduser = _workContext.CurrentUser.Id,
                     Addtime = DateTime.Now,
                     Upuser = _workContext.CurrentUser.Id,
                     Uptime = DateTime.Now,
                     Projectid = brokerrecclient.Projectid,
                     Status = EnumBRECCType.等待上访,
                 };
                 BrokerRecClientService.Create(model);

                 #region 创建订单 杨定鹏 2015年6月3日17:21:39
                 //实例化订单操作
                 var api = new OrderController(_productService, _orderDetailService, _orderService);

                 var jObject=new JObject();

                 api.AddRecommonOrder(jObject);

                 #endregion


                 return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
             }
             return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));

         }

        [System.Web.Http.HttpGet]
         public HttpResponseMessage test()
         {
             return PageHelper.toJson(_orderService.CreateOrderNumber());
         }
    }
}
