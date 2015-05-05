using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.Broker;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// CRM 个人信息管理/admin管理
    /// </summary>
    public class AdminInfoController : ApiController
    {
        private readonly IBrokerService _brokerService;

        public AdminInfoController(IBrokerService brokerService
            )
        {
            _brokerService = brokerService;
        }
    }
}
