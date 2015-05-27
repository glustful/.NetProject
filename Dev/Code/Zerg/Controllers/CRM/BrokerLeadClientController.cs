using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerLeadClient;
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
    /// <summary>
    /// 经纪人带客
    /// </summary>
    public class BrokerLeadClientController : ApiController
    {
        private IBrokerLeadClientService _brokerleadclientService;
        private readonly IBrokerService _brokerService;//经纪人

        public BrokerLeadClientController(IBrokerLeadClientService brokerleadclientService, IBrokerService brokerService)
        {
            _brokerleadclientService = brokerleadclientService;
            _brokerService = brokerService;
        }


        /// <summary>
        /// 查询某经纪人的带客记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SearchBrokerLeadClient(string userid)
        {
            var p = new BrokerLeadClientSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userid))
            };
            var list = _brokerleadclientService.GetBrokerLeadClientsByCondition(p).ToList();
            return PageHelper.toJson(list);

        }


        /// <summary>
        /// 添加一个带客记录
        /// </summary>
        /// <param name="brokerrecclient"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody]  BrokerLeadClientEntity brokerleadclient)
        {
            var entity = new BrokerLeadClientEntity
            {
                Broker = brokerleadclient.Broker
            };

            try
            {
                if (_brokerleadclientService.Create(entity) != null)
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
