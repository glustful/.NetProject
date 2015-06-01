using System.Globalization;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Service.ClientInfo;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
     [EnableCors("*", "*", "*", SupportsCredentials = true)]

     //经纪人推荐客户
    public class BrokerRECClientController : ApiController
    {
         public readonly IBrokerRECClientService BrokerRecClientService;
         private readonly IBrokerService _brokerService;//经纪人
         private readonly IClientInfoService _clientInfoService;
         private readonly IWorkContext _workContext;

         public BrokerRECClientController(
             IBrokerRECClientService brokerRecClientService, 
             IBrokerService brokerService,
             IClientInfoService clientInfoService,
             IWorkContext workContext
             )
         {
             BrokerRecClientService =brokerRecClientService;
             _brokerService = brokerService;
             _clientInfoService = clientInfoService;
             _workContext = workContext;
         }




         /// <summary>
         /// 查询某经纪人推荐的客户
         /// </summary>
         /// <param name="userid"></param>
         /// <returns></returns>
         [HttpGet]
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
         [HttpPost]
         public HttpResponseMessage Add([FromBody]  BrokerRECClientModel  brokerrecclient)
         {
             //查询客户信息
             var sech = new ClientInfoSearchCondition
             {
                 Clientname = brokerrecclient.Clientname,
                 Phone = brokerrecclient.Phone.ToString(CultureInfo.InvariantCulture)

             };
             var cid = _clientInfoService.GetClientInfosByCondition(sech).First().Id;
             var type = BrokerRecClientService.GetBrokerRECClientById(cid).Status;

             //检测
             if (type!=EnumBRECCType.等待上访)
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

                 var cmodel = _clientInfoService.GetClientInfosByCondition(sech).First();

                 var model = new BrokerRECClientEntity
                 {
                     Broker = _brokerService.GetBrokerById(brokerrecclient.Broker),
                     ClientInfo = cmodel,
                     Phone = brokerrecclient.Phone,
                     Qq = brokerrecclient.Qq,
                     Adduser = _workContext.CurrentUser.Id,
                     Addtime = DateTime.Now,
                     Upuser = _workContext.CurrentUser.Id,
                     Uptime = DateTime.Now,
                     Projectid = brokerrecclient.Projectid,
                     Status = EnumBRECCType.审核中,
                 };
                 BrokerRecClientService.Create(model);
                 return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
             }
             return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));
         }
    }
}
