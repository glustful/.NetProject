using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
     [EnableCors("*", "*", "*", SupportsCredentials = true)]

     //经纪人推荐客户
    public class BrokerRECClientController : ApiController
    {
         public readonly IBrokerRECClientService _BrokerRECClientService;
         private readonly IBrokerService _brokerService;//经纪人
         public BrokerRECClientController(IBrokerRECClientService BrokerRECClientService, IBrokerService brokerService)
         {
             _BrokerRECClientService =BrokerRECClientService;
             _brokerService = brokerService;
         }




         /// <summary>
         /// 查询某经纪人推荐的客户
         /// </summary>
         /// <param name="userid"></param>
         /// <returns></returns>
         [HttpGet]
         public HttpResponseMessage SearchBrokerRECClient(string userid)
         {
             var p = new BrokerRECClientSearchCondition
             {
                 Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userid))
             };
             var list = _BrokerRECClientService.GetBrokerRECClientsByCondition(p).ToList();
             return PageHelper.toJson(list);

         }


         /// <summary>
         /// 推荐一个
         /// </summary>
         /// <param name="brokerrecclient"></param>
         /// <returns></returns>
         [HttpPost]
         public HttpResponseMessage Add([FromBody]  BrokerRECClientEntity  brokerrecclient)
         {
             var entity=new BrokerRECClientEntity
             {
                 Broker = brokerrecclient.Broker
             };

               try
                {
                    if (_BrokerRECClientService.Create(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
              return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));

         }


    }
}
