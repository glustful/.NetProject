using System.Collections.Generic;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Bank;

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



        public List<BrokerEntity> GetUser(string name)
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
