using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.RecommendAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;
using System.ComponentModel;
namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    /// <summary>
    /// 推荐经纪人  李洪亮 2015-05-06
    /// </summary>
    [Description("推荐经纪人管理")]
    public class RecommendAgentController : ApiController
    {

        private readonly IRecommendAgentService _recommendagentService;
        private IBrokerService _brokerService;
        /// <summary>
        /// 推荐经纪人管理初始化
        /// </summary>
        /// <param name="recommendagentService">recommendagentService</param>
        /// <param name="brokerService">brokerService</param>
        public RecommendAgentController(IRecommendAgentService recommendagentService, IBrokerService brokerService)
        {
            _recommendagentService = recommendagentService;
            _brokerService = brokerService;
        }

        #region 推荐经济人

        /// <summary>
        /// 查询推荐经纪人表中的所有数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>推荐经纪人表中的所有数据</returns>
        [Description("查询推荐经纪人表中的所有数据")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentList(string name = null, int page = 1, int pageSize = 10)
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {

                Brokername = name,
                Page = Convert.ToInt32(page),
                PageCount = pageSize,

            };
            var recommendAgentList = _recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).Select(p => new
            {
                BBrokername = p.Brokername,
                PresenteebId = p.PresenteebId,
                Brokername = p.Broker.Brokername,


            }).ToList();
            var partnerListCount = _recommendagentService.GetRecommendAgentCount(recomagmentsearchcon);
            return PageHelper.toJson(new { List = recommendAgentList, Condition = recomagmentsearchcon, totalCount = partnerListCount });

        }

        /// <summary>
        /// 查询推荐经纪人表中的所有数据
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns>推荐经纪人表中的所有数据</returns>
        [Description("传入经纪人id，查询推荐经纪人表中的所有数据")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentListById(int id = 0)
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                PresenteebId = id

            };
            var recommendAgentList = _recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).Select(p => new
            {
                BBrokername = p.Brokername,
                phone = p.Broker.Phone,
                regtime = p.Broker.Regtime,
                agentlevel = p.Broker.Agentlevel,
                Brokername = p.Broker.Brokername,
                Nickname = p.Broker.Nickname,
                Headphono = p.Broker.Headphoto,
                Amount = p.Broker.Amount,
                Sfz = p.Broker.Sfz,


            }).ToList();
            var partnerListCount = _recommendagentService.GetRecommendAgentCount(recomagmentsearchcon);
            return PageHelper.toJson(new { List = recommendAgentList, Condition = recomagmentsearchcon, totalCount = partnerListCount });

        }





        /// <summary>
        /// 传入经纪人id，查询该经纪人荐过的其他经纪人
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns>推荐过的其他经纪人列表</returns>
        [Description("传入经纪人id，查询该经纪人荐过的其他经纪人")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentListByUserId(string userId)
        {


            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                BrokerId = Convert.ToInt32(userId)
            };


            return PageHelper.toJson(_recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).Select(p => new
            {
                p.Phone,
                p.Qq,
                p.Brokername,
                p.Agentlevel,
                p.Id
            }).ToList());
        }

        /// <summary>
        /// 传入新增推荐经纪人参数，新增推荐经纪人，返回新增结果状态信息。
        /// </summary>
        /// <param name="recommendAgent">推荐经纪人参数</param>
        /// <returns>新增推荐经纪人结果状态信息</returns>
         [Description("新增推荐经纪人")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddRecommendAgent([FromBody] RecommendAgentEntity recommendAgent)
        {
            if (recommendAgent != null)
            {
                var entity = new RecommendAgentEntity
                {
                    PresenteebId = recommendAgent.PresenteebId,
                    Qq = "",
                    Agentlevel = "",
                    Brokername = "",
                    Phone = "",
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
