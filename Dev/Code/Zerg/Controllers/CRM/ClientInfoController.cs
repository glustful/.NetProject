using System.Globalization;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using CRM.Service.BrokerLeadClient;
using CRM.Service.ClientInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;
using Zerg.Models.CRM;
using System.Data;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 客户信息  李洪亮 2015-05-06
    /// </summary>
    [Description("客户信息管理类")]
    public class ClientInfoController : ApiController
    {
        private IClientInfoService _clientInfoService;
        private IBrokerService _brokerService;
        private IBrokerRECClientService _brokerRecClientService;
        private IBrokerLeadClientService _brokerLeadClientService;
        private readonly IWorkContext _workContext;
        public ClientInfoController(
            IClientInfoService clientInfoService,
            IBrokerService brokerService,
            IBrokerRECClientService brokerRecClientService,
            IBrokerLeadClientService brokerLeadClientService,
            IWorkContext workContext
            )
        {
            _clientInfoService = clientInfoService;
            _brokerService = brokerService;
            _brokerRecClientService = brokerRecClientService;
            _brokerLeadClientService = brokerLeadClientService;
            _workContext = workContext;
        }

        #region 客户信息

        /// <summary>
        /// 传入客户姓名，检索经纪人列表，返回推荐经纪人表中
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="clientName">客户姓名</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>推荐经纪人列表</returns>
        /// <param name="orderByAll">排序参数{序号（OrderById），客户姓名（OrderByClientname），联系电话（OrderByPhone），
        /// 经纪人名（OrderByBrokername），操作时间（OrderByUptime）}</param>
        /// <param name="isDes">是否降序</param>
        [Description("通过客户姓名查询到经纪人列表")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetClientInfoList(EnumBRECCType status, string clientName, int page, int pageSize, EnumBrokerRECClientSearchOrderBy orderByAll = EnumBrokerRECClientSearchOrderBy.OrderById, bool isDes = true)
        {
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy =orderByAll ,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Clientname = clientName,
                IsDescending =isDes 
                //Brokername = clientName
            };

            var list = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Clientname,
                a.ClientInfo.Phone,
                a.Brokername,
                a.Uptime

            }).ToList();

            var totalCont = _brokerRecClientService.GetBrokerRECClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }

        /// <summary>
        /// 传入客户ID，检索客户信息，返回客户详细信息
        /// </summary>
        /// <param name="id">客户id</param>
        /// <returns>客户详细信息</returns>

        [Description("传入客户ID，检索返回客户详细信息")]
        [HttpGet]
        public HttpResponseMessage ClientInfo(int id)
        {
            var condition = new BrokerRECClientSearchCondition
            {
                Id = id
            };


            var model = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).ToList();
            try
            {
                var clientModel = model.Select(p => new
                {

                    Clientname = p.ClientInfo.Clientname,
                    Phone = p.ClientInfo.Phone,
                    Housetype = p.ClientInfo.Housetype,
                    Houses = p.ClientInfo.Houses,
                    Note = p.ClientInfo.Note,
                    Uptime = p.Uptime.ToString(CultureInfo.InvariantCulture)
                });
                var brokerModel = model.Select(p => new
                {

                    Brokername = p.Brokername,
                    Brokerlevel = p.Brokerlevel,
                    Phone = p.Broker.Phone,
                    Qq = p.Broker.Qq,
                    RegTime = p.Broker.Regtime.ToString(CultureInfo.InvariantCulture),
                    Projectname = p.Projectname
                });
                return PageHelper.toJson(new { clientModel, brokerModel });
            }
            catch { }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }




        /// <summary>
        /// 传入客户参数，添加客户，返回添加客户信息。
        /// </summary>
        /// <param name="clientinfo">客户信息参数</param>
        /// <returns>结果状态信息</returns>

        [Description("新增一个客户")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddClientInfo([FromBody] ClientInfoEntity clientinfo)
        {
            if (clientinfo != null)
            {
                var entity = new ClientInfoEntity
                {
                    Clientname = clientinfo.Clientname,
                    Houses = clientinfo.Houses,
                    Housetype = clientinfo.Housetype,
                    Note = clientinfo.Note,
                    Phone = clientinfo.Phone,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                    Adduser = clientinfo.Adduser,
                    Upuser = clientinfo.Upuser
                };

                try
                {
                    if (_clientInfoService.Create(entity) != null)
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



        /// <summary>
        /// 无传入参数，通过经纪人ID查询他的客户列表，返回经纪人客户列表
        /// </summary>
        /// <returns>经纪人客户列表</returns>
        [Description("通过经纪人ID查询他的客户列表，返回经纪人客户列表")]
        public HttpResponseMessage GetClientInfoListByUserId(int page)
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                var condition = new ClientInfoSearchCondition
                {
                    Addusers = broker.UserId,
                    Page =page ,
                    PageCount =10
                };
                var list = _clientInfoService.GetClientInfosByCondition(condition).Select(p => new
                    {
                        p.Clientname,
                        p.Phone,
                        p.Id,
                        p.Houses,
                        p.Housetype

                    }).ToList();

                int totalCount = _clientInfoService.GetClientInfoCount(condition);

                return PageHelper.toJson(new { list = list,totalCount });
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));

        }





        ///////////新增方法，查询当前经纪人带客信息，以及带客进度反馈信息///////////
        /// <summary>
        /// 传入客户ID，检索客户信息，返回客户详细信息
        /// </summary>
        /// <param name="id">客户id</param>
        /// <returns>客户详细信息</returns>

        [Description("传入经纪人ID，检索返回带客详细信息")]
        [HttpGet]
        public HttpResponseMessage GetStatusByUserId(int page)
        {
          //============================================chenda start===========================================
            var user = (UserBase)_workContext.CurrentUser;
            var broker = _brokerService.GetBrokerByUserId(user.Id);
            
            if (broker != null) 
            {
                var condition = new BrokerLeadClientSearchCondition
                {
                    OrderBy = EnumBrokerLeadClientSearchOrderBy.OrderByTime,
                    Page = page,
                    PageCount = 10,
                    Brokers = broker
                };
                var conditon2 = new BrokerRECClientSearchCondition
                {
                    OrderBy =EnumBrokerRECClientSearchOrderBy.OrderByTime,
                    Page = page,
                    PageCount = 10,
                    Brokers = broker
                };
                var model = _brokerLeadClientService.GetBrokerLeadClientsByCondition(condition).ToList();
                if (model == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "当前经纪人没有带过客户"));
                }
                var model2 = _brokerRecClientService.GetBrokerRECClientsByCondition(conditon2).ToList();
                if (model2 == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "当前经纪人没有推荐过客户"));
                }

                List<ReturnCustomModel> listModel = new List<ReturnCustomModel>();

                // 带客
               var  listdk= model.Select(p => new
                {
                    StrType = "带客",
                    Clientname = p.ClientInfo.Clientname,
                    Housetype = p.ClientInfo.Housetype,
                    Houses = p.ClientInfo.Houses,
                    Phone = p.Broker.Phone,
                    Status = p.Status,
                    Id = p.Id,
                }).ToList().ToList();

                foreach(var p in listdk)
                {
                    listModel.Add(new ReturnCustomModel{ Clientname=p.Clientname, Houses=p.Houses, Housetype=p.Housetype, Id=p.Id.ToString(), Phone=p.Phone, Status=p.Status.ToString(), StrType=p.StrType});
                }

                //推荐
                var  listtj= model2.Select(c => new
                {
                        StrType = "推荐",
                        Clientname = c.ClientInfo.Clientname,
                        Housetype = c.ClientInfo.Housetype,
                        Houses = c.ClientInfo.Houses,
                        Phone = c.Broker.Phone,
                        Status = c.Status,
                        Id = c.Id,
                        Uptime = c.Uptime.ToString(CultureInfo.InvariantCulture)
                }).ToList();

                foreach (var p in listtj)
                {
                    listModel.Add(new ReturnCustomModel { Clientname = p.Clientname, Houses = p.Houses, Housetype = p.Housetype, Id = p.Id.ToString(), Phone = p.Phone, Status = p.Status.ToString(), StrType = p.StrType});
                }

                int totalCount = _brokerLeadClientService.GetBrokerLeadClientCount(condition) + _brokerRecClientService.GetBrokerRECClientCount(conditon2);
                return PageHelper.toJson(new { List = listModel, totalCount = totalCount });       
            
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
            //================================================================chenda end============================================================================
        }
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 判断当前用户是否是经济人，返回结果状态值，是经纪人返回"1",否则返回"0"
        /// </summary>
        /// <returns>是否是经纪人结果状态信息</returns>

        [Description("判断当前用户是否是经济人")]
        [HttpGet]
        public HttpResponseMessage Getbroker()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    if (broker.Usertype ==EnumUserType .普通用户)
                    {
                        return PageHelper.toJson(new { count = 0 });//返回0，为普通用户
                    }
                    else
                    {
                        return PageHelper.toJson(new { count = 1 });//返回1，为经纪人、财务、admin、带客人员、驻秘、商家
                    }
                    
                }
                else
                {
                    return PageHelper.toJson(new { count = 0 });//返回0，不是经纪人
                }
            }
            return PageHelper.toJson(new { count = 2 }); //返回2，未登录
        }
        /// <summary>
        /// 获取当前经纪人，通过经纪人ID查询他的客户个数，返回客户数
        /// </summary>
        /// <returns>经纪人客户数量</returns>
        [Description("获取当前经纪人，通过经纪人ID查询他的客户个数，返回客户数")]
        public HttpResponseMessage GetClientInfoNumByUserId()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    var condition = new ClientInfoSearchCondition
                    {
                        Addusers = broker.Id
                    };
                    var count = _clientInfoService.GetClientInfoCount(condition);

                    return PageHelper.toJson(new { count });
                }
                else
                {
                    var count = 0;

                    return PageHelper.toJson(new { count });
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));

        }
        #endregion


    }

    /// <summary>
    /// 返回带客列表信息
    /// </summary>
    public class  ReturnCustomModel
    {

        // 客户相关信息
        public virtual string Clientname { get; set; }
        // 经纪人相关信息
        public virtual string Housetype { get; set; }
        public virtual string Houses { get; set; }
        //带客进度
        public virtual string Status { get; set; }
        //客户电话
        public virtual string Phone { get; set; }
        //Id
        public virtual string Id { get; set; }

        public string StrType { get; set; }
    }


}
