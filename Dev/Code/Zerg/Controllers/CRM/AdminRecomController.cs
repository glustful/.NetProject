using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using Webdiyer.WebControls.Mvc;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// admin的推荐至平台流程处理
    /// </summary>
    public class AdminRecomController : ApiController
    {
        private readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBrokerService _brokerService;

        public AdminRecomController(IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService
            )
        {
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
        }

        #region 经济人列表 杨定鹏 2015年5月4日14:29:24

        /// <summary>
        /// 经纪人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage BrokerList([FromBody] BrokerRECClientSearchCondition brokerRecClientSearchCondition)
        {
            if (brokerRecClientSearchCondition == null)
                throw new ArgumentNullException("brokerRecClientSearchCondition");
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = EnumBrokerRECClientSearchOrderBy.OrderById,
            };
            return PageHelper.toJson(_brokerRecClientService.GetBrokerRECClientsByCondition(condition).ToPagedList(Convert.ToInt32(brokerRecClientSearchCondition.PageCount) + 1, 10).ToList());
        }
        #endregion

        #region 待审核业务处理 杨定鹏 2015年5月5日16:28:30
        /// <summary>
        /// 查看审核详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAuditDetail(int id)
        {
            return PageHelper.toJson(_brokerRecClientService.GetBrokerRECClientById(id));
        }

        /// <summary>
        /// 确认审核
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage PassAudit([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var model = new BrokerRECClientEntity
            {
                Id = brokerRecClientModel.Id,
                Status = brokerRecClientModel.Status
            };
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true,"确认成功"));
        }

        #region 选择带客人 杨定鹏 2015年5月5日19:45:14
        /// <summary>
        /// 带客人列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage WaiterList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(condition).ToList());
        }

        #endregion

        #region 场秘管理 杨定鹏 2015年5月5日19:45:40
        /// <summary>
        /// 场秘列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage SecretaryList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(condition).ToList());
        }

        #endregion

        /// <summary>
        /// 确认成功/失败
        /// </summary>
        /// <param name="brokerRecClientModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Access([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var model = new BrokerRECClientEntity
            {
                Id=brokerRecClientModel.Id,
                Status = brokerRecClientModel.Status
            };
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }

        #endregion
    }
}
