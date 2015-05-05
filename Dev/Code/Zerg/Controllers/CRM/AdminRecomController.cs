using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Entity.Model;
using CRM.Service.Broker;
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
        private readonly IBrokerService _brokerService;

        public AdminRecomController(IBrokerService brokerService
            )
        {
            _brokerService = brokerService;
        }

        #region 经济人列表 杨定鹏 2015年5月4日14:29:24
        /// <summary>
        /// 经纪人列表
        /// </summary>
        /// <param name="brokerSearchModel"></param>
        /// <returns></returns>
        public HttpResponseMessage BrokerList([FromBody]BrokerSearchModel brokerSearchModel)
        {
            var condition = new BrokerSearchCondition()
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(condition).ToPagedList(Convert.ToInt32(brokerSearchModel.Pageindex) + 1, 10).ToList());
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
            return null;
        }

        #endregion
    }
}
