using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.BrokerRECClient;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 所有客户信息模块的带客推荐，所有成交
    /// </summary>
    public class ClientScheduleController : ApiController
    {
        private readonly IBrokerRECClientService _brokerRecClientService;

        public ClientScheduleController(IBrokerRECClientService brokerRecClientService)
        {
            _brokerRecClientService = brokerRecClientService;
        }

        #region 推荐列表明细
        /// <summary>
        /// 根据传入的进度类型返回带客列表
        /// </summary>
        /// <param name="brokerRecClientModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RecommendList([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = EnumBrokerRECClientSearchOrderBy.OrderById,
                Status = brokerRecClientModel.Status
            };
            return PageHelper.toJson(_brokerRecClientService.GetBrokerRECClientsByCondition(condition).ToList());
        }

        /// <summary>
        /// 根据传入的ID返回用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBrokerInfo(int id)
        {
            try
            {
                var model = _brokerRecClientService.GetBrokerRECClientById(id);
                if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "该条记录不存在"));
                var model2 = new BrokerRECClientModel
                {
                    Id = model.Id,
                    Broker = model.Broker.Id
                };

                return PageHelper.toJson(model2);
            }
            catch (Exception)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据库连接失败"));
                }
        }

        #endregion
    }
}
