using System.Globalization;
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
using CRM.Service.ClientInfo;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 经纪人带客
    /// </summary>
    public class BrokerLeadClientController : ApiController
    {
        private IBrokerLeadClientService _brokerleadclientService;
        private readonly IBrokerService _brokerService;//经纪人
        private readonly IClientInfoService _clientInfoService;
        private readonly IWorkContext _workContext;

        public BrokerLeadClientController(
            IBrokerLeadClientService brokerleadclientService, 
            IBrokerService brokerService,
            IClientInfoService clientInfoService,
            IWorkContext workContext
            )
        {
            _brokerleadclientService = brokerleadclientService;
            _brokerService = brokerService;
            _clientInfoService = clientInfoService;
            _workContext = workContext;
        }

        /// <summary>
        /// 添加一个带客记录
        /// </summary>
        /// <param name="brokerrecclient"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody] BrokerRECClientModel brokerleadclient)
        {
            int Cid = 0;
            EnumBRECCType type;
            //查询客户信息
            var sech = new ClientInfoSearchCondition
            {
                Clientname = brokerleadclient.Clientname,
                Phone = brokerleadclient.Phone.ToString(CultureInfo.InvariantCulture)

            };
            var Cmodel = _clientInfoService.GetClientInfosByCondition(sech).FirstOrDefault();

            if (Cmodel == null)
            {
                //客户信息
                var client = new ClientInfoEntity
                {
                    Clientname = brokerleadclient.Clientname,
                    Phone = brokerleadclient.Phone.ToString(CultureInfo.InvariantCulture),
                    Housetype = brokerleadclient.HouseType,
                    Houses = brokerleadclient.Houses,
                    Note = brokerleadclient.Note,
                    Adduser = brokerleadclient.Broker,
                    Addtime = DateTime.Now,
                    Upuser = brokerleadclient.Broker,
                    Uptime = DateTime.Now
                };
                _clientInfoService.Create(client);

                Cid = _clientInfoService.GetClientInfosByCondition(sech).First().Id;
                type = _brokerleadclientService.GetBrokerLeadClientById(Cid).Status;
            }
            else
            {
                type = _brokerleadclientService.GetBrokerLeadClientById(Cmodel.Id).Status;
            }

            //检测
            if (type != EnumBRECCType.等待上访)
            {
                

                var cmodel = _clientInfoService.GetClientInfosByCondition(sech).First();

                var model = new BrokerLeadClientEntity
                {
                    Broker = _brokerService.GetBrokerById(brokerleadclient.Broker),
                    ClientInfo = cmodel,
                    Adduser = _workContext.CurrentUser.Id,
                    Addtime = DateTime.Now,
                    Upuser = _workContext.CurrentUser.Id,
                    Uptime = DateTime.Now,
                    ProjectId = brokerleadclient.Projectid,
                    Status = EnumBRECCType.等待上访,
                };
                _brokerleadclientService.Create(model);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));

        }

    }
}
