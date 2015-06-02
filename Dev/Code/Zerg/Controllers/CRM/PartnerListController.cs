using System.ComponentModel;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PartnerList;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
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

        [HttpGet]
        public HttpResponseMessage SearchPartnerList(EnumPartnerType status, string name = null, int page = 1, int pageSize = 10)
        {
            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername = name,
                Status = status,
                Page = Convert.ToInt32(page),
                PageCount =pageSize
            };
            var partnerList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                p.Id,
                p.PartnersName,
                p.PartnersId,
                BrokerName = p.Brokername,
                Phone = p.Phone,
                Regtime = p.Regtime,
                Agentlevel = p.Agentlevel,
                Headphoto=p.Headphoto,
                status=EnumPartnerType.同意
            });
            var partnerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = partnerList, Condition = brokerSearchCondition, totalCount = partnerListCount });
             
        }

        /// <summary>
        /// 查询经纪人下的合伙人List
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>
        [Description("查询经纪人下的合伙人List")]
        [HttpGet]
        public HttpResponseMessage PartnerListDetailed(int userId)
        {
            var partnerlistsearchcon = new PartnerListSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(userId)
            };
            var partnerList = _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Where(p=>p.Broker.Id==userId).Select(p => new
                {
                 Name=p.Brokername,
                 AddTime =p.Addtime,
                 regtime=p.Regtime, 
                 Phone=p.Phone,
                Headphoto= p.Broker .Headphoto ,
                Id=p.Id,
                PartnerId=p.PartnerId
              
                }).ToList();
            return PageHelper.toJson(new { list = partnerList });
        }

        /// <summary>
        /// 新增合伙人
        /// </summary>
        /// <param name="partnerList"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddPartnerList([FromBody] PartnerListEntity partnerList)
        {
            var sech = new BrokerSearchCondition
            {
                Phones = new[] { partnerList.Phone}
            };
            var list = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();
            if (list != null)
            {
                if (list.PartnersId != 0)
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
                            Status = EnumPartnerType.默认
                        };

                        try
                        {
                            if (_partnerlistService.Create(entity) != null)
                            {
                                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！等待对方同意"));
                            }
                        }
                        catch
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                        }
                    }
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户已经是别人的合伙人了！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在"));


        }




        #endregion


    }
}
