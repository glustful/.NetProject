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

namespace Zerg.Controllers.CRM
{
   [EnableCors("*", "*", "*", SupportsCredentials = true)]
   [AllowAnonymous]
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
        public HttpResponseMessage GetRecommendAgentList(string name = null, int page = 1, int pageSize = 10)
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                
                Brokername = name,
                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };
            var recommendAgentList = _recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).Select(p => new {               
              BBrokername= p.Brokername,
              Brokername=p.Broker.Brokername,
              
            
            }).ToList();
            var partnerListCount = _recommendagentService.GetRecommendAgentCount(recomagmentsearchcon);
            return PageHelper.toJson(new { List = recommendAgentList, Condition = recomagmentsearchcon, totalCount = partnerListCount });
          
        }


      


        /// <summary>
        /// 查询经纪人下他推荐过的经纪人
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRecommendAgentListByUserId(string userId)
        {
            var recomagmentsearchcon = new RecommendAgentSearchCondition
            {
                BrokerId =Convert.ToInt32( userId)
            };


            return PageHelper.toJson(_recommendagentService.GetRecommendAgentsByCondition(recomagmentsearchcon).Select(p=>  new
            {
                p.Phone,
                p.Qq,
                p.Brokername,
                p.Agentlevel,
                p.Id
            }).ToList());
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

       [HttpPost]
        public HttpResponseMessage SendRecommendAgentSms([FromBody] string phone)
        {
           if (!string.IsNullOrEmpty(phone))
           {
              
           }
           var p = SMSHelper.Sending(phone, "你好，感谢你使用创富宝软件！发送时间是：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
           return PageHelper.toJson(p);
        }
    }
}
