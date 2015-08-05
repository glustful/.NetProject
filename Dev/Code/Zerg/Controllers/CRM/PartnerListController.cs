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
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;


namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 合伙人  李洪亮  2015-05-05
    /// </summary>
    [Description("合伙人管理类")]
    public class PartnerListController : ApiController
    {
        private readonly IPartnerListService _partnerlistService;
        private readonly IBrokerService _brokerService;
        private readonly IWorkContext _workContext;
        /// <summary>
        /// 合伙人管理类初始化
        /// </summary>
        /// <param name="partnerlistService">partnerlistService</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="workContext">workContext</param>
        public PartnerListController(IPartnerListService partnerlistService, IBrokerService brokerService, IWorkContext workContext)
        {
            _partnerlistService = partnerlistService;
            _brokerService = brokerService;
            _workContext = workContext;
        }

        #region 合伙人详情

        /// <summary>
        /// 查询经纪人及他所属的合伙人，返回合伙人列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="name">名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <param name="orderByAll">排序参数{经纪人名（OrderByBrokername），他的上家（OrderByPartnersName）}</param>
        /// <param name="isDes">是否降序</param>
        /// <returns>合伙人列表</returns>
        [Description("检索返回经纪人及他所属的合伙人列表")]
        [HttpGet]
        public HttpResponseMessage SearchPartnerList(EnumPartnerType status, EnumBrokerSearchOrderBy orderByAll = EnumBrokerSearchOrderBy.OrderById, bool isDes = true, string name = null, int page = 1, int pageSize = 10)
        {
            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername = name,
                Status = status,
                Page = Convert.ToInt32(page),
                PageCount = pageSize,
                OrderBy =orderByAll ,
                isDescending =isDes 
            };
            var partnerList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                p.Id,
                p.UserId,
                PartnersName = p.WeiXinNumber == null ? "无" : p.WeiXinNumber,
                p.PartnersId,
                BrokerName = p.Brokername,
                Phone = p.Phone,
                Regtime = p.Regtime,
                Agentlevel = p.Agentlevel,
                Headphoto = p.Headphoto,
                status = EnumPartnerType.同意
            });
            var partnerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = partnerList, Condition = brokerSearchCondition, totalCount = partnerListCount });

        }
        /// <summary>
        /// 传入经纪人ID，查询和经纪人关联的合伙人列表
        /// </summary>
        /// <param name="PartnersId">经纪人id</param>
        /// <returns>经纪人的合伙人列表</returns>
        [Description("返回经纪人关联的合伙人列表")]
        [HttpGet]
        public HttpResponseMessage SearchPartnerList1(int PartnersId)
        {
            if (PartnersId > 0)
            {
                var brokerSearchCondition = new BrokerSearchCondition
                {

                    PartnersId = PartnersId
                };
                var partnerList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
                {
                    p.Id,
                    p.UserId,
                    PartnersName = p.WeiXinNumber,
                    p.PartnersId,
                    BrokerName = p.Brokername,
                    Nickname = p.Nickname,
                    Phone = p.Phone,
                    Regtime = p.Regtime,
                    Agentlevel = p.Agentlevel,
                    Headphoto = p.Headphoto,
                    Sfz = p.Sfz,
                    Amount = p.Amount,
                    status = EnumPartnerType.同意
                }).ToList().Select(p => new
                {
                    p.Id,
                    p.UserId,
                    p.PartnersName,
                    p.PartnersId,
                    BrokerName = p.BrokerName,
                    Nickname = p.Nickname,
                    Phone = p.Phone,
                    Regtime = p.Regtime.ToString("yyyy-MM-dd"),
                    Agentlevel = p.Agentlevel,
                    Headphoto = p.Headphoto,
                    Sfz = p.Sfz,
                    Amount = p.Amount,
                    status = EnumPartnerType.同意
                });
                var partnerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
                return PageHelper.toJson(new { List = partnerList, Condition = brokerSearchCondition, totalCount = partnerListCount });
            }
            else { return null; }

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
                Brokers = _brokerService.GetBrokerByUserId(userId),
                Status = EnumPartnerType.同意,
            };

            var partnerList = _partnerlistService.GetPartnerListsByCondition(partnerlistsearchcon).Where(p => p.Broker.UserId == userId).ToList().Select(c => new
                {
                    Name = c.Brokername,
                    AddTime = c.Addtime,
                    regtime = c.Regtime.ToString("yyyy-MM-dd"),
                    Phone = c.Phone,
                    Headphoto = _brokerService.GetBrokerById(Convert.ToInt32(c.PartnerId)).Headphoto,
                    Id = c.Id,
                    PartnerId = c.PartnerId,


                }).ToList().Select(c => new
                {
                    Name = c.Name,
                    AddTime = c.AddTime,
                    regtime = c.regtime,
                    Phone = c.Phone,
                    Headphoto = _brokerService.GetBrokerById(Convert.ToInt32(c.PartnerId)).Headphoto,
                    Id = c.Id,
                    PartnerId = c.PartnerId,
                    Agentlevel = _brokerService.GetBrokerById(c.PartnerId).Agentlevel,
                    Amount = _brokerService.GetBrokerById(c.PartnerId).Amount,
                    Sfz = _brokerService.GetBrokerById(c.PartnerId).Sfz,
                    Nickname = _brokerService.GetBrokerById(c.PartnerId).Nickname,
                });

            return PageHelper.toJson(new { partnerList });

        }

        /// <summary>
        /// 传入合伙人参数，新增合伙人，返回新增结果状态信息
        /// </summary>
        /// <param name="partnerList">partnerList</param>
        /// <returns>合伙人列表</returns>
        [HttpPost]
        [Description("新增加一个合伙人")]
        public HttpResponseMessage AddPartnerList([FromBody] PartnerListEntity partnerList)
        {
            var sech = new BrokerSearchCondition
            {
                Phone = partnerList.Phone
            };
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前用户
                if (broker != null)
                {
                    var getbroker = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();//通过手机号查询经纪人
                    if (getbroker != null)
                    {
                        //判断是否是本身
                        if (broker.Id == getbroker.Id)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，不能添加本身"));
                        }
                        //判断要添加的这个经纪人是否有上家
                        if (getbroker.PartnersId == null || getbroker.PartnersId == 0)//查询他的上家是否存在
                        {
                            //1 添加到partnerList表中  

                            var entity = new PartnerListEntity();
                            entity.PartnerId = getbroker.Id;     //添加的下家
                            entity.Brokername = getbroker.Brokername;
                            entity.Phone = getbroker.Phone;
                            entity.Agentlevel = getbroker.Level.Name;
                            entity.Regtime = getbroker.Regtime;

                            entity.Adduser = broker.Id;
                            entity.Addtime = DateTime.Now;
                            entity.Upuser = broker.Id;
                            entity.Uptime = DateTime.Now;
                            entity.Broker = broker;
                            entity.Status = EnumPartnerType.默认;
                            //判断当前用户的合伙人个数是否》=3

                            PartnerListSearchCondition plsearCon = new PartnerListSearchCondition
                            {
                                Brokers = broker,
                                Status = EnumPartnerType.同意
                            };
                            if (_partnerlistService.GetPartnerListCount(plsearCon) >= 3)
                            {
                                return PageHelper.toJson(PageHelper.ReturnValue(true, "对不起，您的合伙人数已满 不能添加"));
                            }

                            if (_partnerlistService.Create(entity) != null)
                            {

                                return PageHelper.toJson(PageHelper.ReturnValue(true, "邀请成功！等待对方同意"));

                            }
                            else
                            {
                                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新失败！请与客服联系"));
                            }


                        }
                        else
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户已经是别人的合伙人了！"));

                        }


                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "该经纪人不存在"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，请登录"));






            //var list = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();//通过手机号查询经纪人
            //if (list != null)
            //{
            //    if (list.PartnersId != 0)//查询他的上家是否存在
            //    {
            //        if (partnerList != null)
            //        {
            //            var entity = new PartnerListEntity();
            //            entity.PartnerId = list.Id;     //添加的下家
            //            entity.Phone = partnerList.Phone;

            //            if (list.Id == _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id)
            //            {
            //                return PageHelper.toJson(PageHelper.ReturnValue(false, "不能添加自身！"));
            //            }

            //            //上家的属性
            //                 entity.Agentlevel = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Level.Name;
            //                 entity.Brokername = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Brokername;

            //                 entity.Regtime = DateTime.Now;
            //                 entity.Broker = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id);

            //                 entity.Upuser = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id;
            //                 entity.Uptime = DateTime.Now;
            //                 entity.Adduser = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Id;
            //                 entity.Addtime = DateTime.Now;
            //                 entity.Status = EnumPartnerType.默认;

            //            try
            //            {
            //                if (_partnerlistService.Create(entity) != null)
            //                {
            //                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！等待对方同意"));
            //                }
            //            }
            //            catch
            //            {
            //                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
            //            }
            //        }
            //        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            //    }
            //    return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户已经是别人的合伙人了！"));
            //}
            //return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在"));


        }

        /// <summary>
        /// 查询经纪人收到的邀请，返回邀请列表
        /// </summary>
        /// <param name="brokerId">经纪人ID</param>
        /// <returns>经纪人收到的邀请列表</returns>
        [Description("查询经纪人收到的邀请，返回邀请列表")]
        public HttpResponseMessage GetInviteForBroker(int brokerId = 0)
        {
            if (brokerId == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            if (_partnerlistService.GetInviteForBroker(_brokerService.GetBrokerByUserId(brokerId).Id, EnumPartnerType.默认).Any())
            {
                var list = _partnerlistService.GetInviteForBroker(_brokerService.GetBrokerByUserId(brokerId).Id, EnumPartnerType.默认).Select(a => new
                {
                    a.Id,
                    a.PartnerId,
                    HeadPhoto = a.Broker.Headphoto,
                    BrokerName = a.Broker.Brokername,
                    AddTime = a.Addtime
                }).ToList().Select(b => new
                {
                    b.Id,
                    b.PartnerId,
                    b.HeadPhoto,
                    b.BrokerName,
                    AddTime = b.AddTime.ToString("yyy-mm-dd")
                });

                return PageHelper.toJson(new { list });
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "当前没有邀请"));
        }

        /// <summary>
        /// 设置合伙人状态，返回设置结果状态信息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="id">合伙人id</param>
        /// <returns>设置合伙人状态结果状态信息</returns>
        [Description(" 设置合伙人状态")]
        [HttpGet]
        public HttpResponseMessage SetPartner(EnumPartnerType status, int id = 0)
        {
            if (id == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            var model = _partnerlistService.GetPartnerListById(id);
            model.Status = status;
            if (status == EnumPartnerType.拒绝)
            {
                _partnerlistService.Update(model);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "拒绝成功！"));
            }

            else
            {



                var user = (UserBase)_workContext.CurrentUser;
                if (user != null)
                {
                    var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前用户
                    if (broker != null)
                    {
                        //判断要添加的这个经纪人是否有上家
                        if (broker.PartnersId == null || broker.PartnersId == 0)//查询他的上家是否存在
                        {

                            // 更改状态为同意 1 ，  
                            if (_partnerlistService.Update(model) != null)
                            {
                                //2更新 当前要添加的这个经纪人的PartnersId字段为当前用户
                                broker.PartnersId = model.Broker.Id;
                                broker.WeiXinNumber = model.Broker.Brokername;
                                if (_brokerService.Update(broker) != null)
                                {

                                    return PageHelper.toJson(PageHelper.ReturnValue(true, "邀请成功！"));
                                }

                            }
                        }
                        else
                        {
                            model.Status = EnumPartnerType.拒绝;
                            _partnerlistService.Update(model);
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起,您已经是别人的合伙人了！"));
                        }

                    }
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，请先登录"));

            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，数据错误"));
        }


        #endregion


    }
}
