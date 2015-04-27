using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CRM.Entity.Model;
using CRM.Service.Bank;
using CRM.Service.Broker;

namespace Zerg.Controllers
{
    /// <summary>
    /// CRM 经纪人
    /// </summary>
    public class CRMBrokerController : ApiController
    {
        private readonly IBankService _bankService;

        public CRMBrokerController(IBankService bankService )
        {
            _bankService = bankService;
        }

        public List<BrokerEntity> GetUser()
        {
            var brokerlist = new List<BrokerEntity>
            {
                new BrokerEntity()
            };

            _bankService.GetBankById(0);

            return brokerlist;
        }

    }
}
