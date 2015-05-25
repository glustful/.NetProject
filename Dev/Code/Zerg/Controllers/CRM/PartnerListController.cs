using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PartnerList;
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
    /// <summary>
    /// 合伙人  李洪亮  2015-05-05
    /// </summary>
    public class PartnerListController : ApiController
    {
        private IPartnerListService _partnerlistService;
        private IBrokerService _brokerService;
        public PartnerListController(IPartnerListService partnerlistService, IBrokerService brokerService)
        {
            _partnerlistService = partnerlistService;
            _brokerService = brokerService;
        }

        #region 合伙人详情


        /// <summary>
        /// 查询经纪人及他所属的合伙人
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchPartnerList(string name = null, int page = 1, int pageSize = 10)
        {
            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername = name,
                Page = Convert.ToInt32(page),
                PageCount =pageSize
            };
            var partnerList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                Id = p.Id,
                PartnersName = p.PartnersName,
                PartnersId = p.PartnersId,
                BrokerName = p.Brokername
            }).ToList();
            var partnerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = partnerList, Condition = brokerSearchCondition, totalCount = partnerListCount });
             
        }

        /// <summary>
        /// 查询经纪人下的合伙人List
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage PartnerListDetailed(string userId)
        {
            var partnerlistsearchcon = new PartnerListSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId))
            };
            var partnerList = _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Select(p => new
                {
                 Name=p.Brokername,
                 AddTime =p.Addtime

                }).ToList();
            return PageHelper.toJson(partnerList);
        }

        /// <summary>
        /// 新增合伙人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddPartnerList([FromBody] PartnerListEntity partnerList)
        {
            if (partnerList != null)
            {
                var entity = new PartnerListEntity
                {
                    Agentlevel = "",
                    Brokername = "",
                    PartnerId = 0,
                    Phone = 0,
                    Regtime = DateTime.Now,
                    Broker = null,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };

                try
                {
                    if (_partnerlistService.Create(entity) != null)
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
