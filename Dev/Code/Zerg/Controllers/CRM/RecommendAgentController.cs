using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.RecommendAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{

    /// <summary>
    /// 推荐经纪人  李洪亮 2015-05-06
    /// </summary>
    public class RecommendAgentController : ApiController
    {

        private readonly IRecommendAgentService _recommendagentService;
        private IBrokerService _brokerService;
        public RecommendAgentController(IRecommendAgentService recommendagentService, IBrokerService brokerService)
        {
            _recommendagentService = recommendagentService;
            _brokerService=brokerService;
        }

        #region 推荐经济人

        /// <summary>
        /// 查询推荐经纪人表中的所有数据
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentList()
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                 PageCount=10
            };
            return PageHelper.toJson(_recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).ToList());
        }





        /// <summary>
        /// 查询经纪人下他推荐过的经纪人
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentByUserId(string userId)
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId))
            };
            return PageHelper.toJson(_recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).ToList());
        }

        /// <summary>
        /// 新增 推荐经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddRecommendAgent([FromBody] RecommendAgentEntity recommendAgent)
        {
            if (recommendAgent != null)
            {
                var entity = new RecommendAgentEntity
                {
                    PresenteebId=recommendAgent.PresenteebId,
                     Qq=0,
                    Agentlevel = "",
                    Brokername = "",
                    Phone = 0,
                    Regtime = DateTime.Now,
                    Broker = null,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };

                try
                {
                    if (_recommendagentService.Create(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }




        #endregion


    }
}
