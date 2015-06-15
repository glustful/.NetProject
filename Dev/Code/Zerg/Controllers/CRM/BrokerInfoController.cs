using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using CRM.Entity.Model;
using CRM.Service.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using YooPoon.Core.Data;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;
using YooPoon.WebFramework.User.Entity;
using YooPoon.Core.Site;
using CRM.Service.PartnerList;
using CRM.Service.RecommendAgent;
using CRM.Service.ClientInfo;
using CRM.Service.MessageDetail;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous ]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 经纪人管理  李洪亮  2015-05-04
    /// </summary>
    [Description("经纪人管理")]
    public class BrokerInfoController : ApiController
    {
        private readonly IBrokerService _brokerService;
        private readonly IWorkContext _workContext;
        private IPartnerListService _partnerlistService;//合伙人
        private readonly IRecommendAgentService _recommendagentService; //推荐经纪人
        private IClientInfoService _clientInfoService;//客户
        private readonly IRoleService _roleService;
        private readonly IMessageDetailService _MessageService;
        private readonly IUserService _userService;

        public BrokerInfoController(IClientInfoService clientInfoService,
            IWorkContext workContext, 
            IBrokerService brokerService,
            IPartnerListService partnerlistService, 
            IRecommendAgentService recommendagentService,
            IRoleService roleService,
            IMessageDetailService MessageService,
            IUserService userService
            )
        {
            _clientInfoService = clientInfoService;
            _workContext =workContext;
            _brokerService = brokerService;
            _partnerlistService = partnerlistService;
            _recommendagentService = recommendagentService;
            _roleService = roleService;
            _MessageService = MessageService; 
            _userService = userService;
        }


        #region 经纪人管理 
       
        
        
        /// <summary>
        /// 获取经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBroker(string id)
        {
            if (string.IsNullOrEmpty(id) || !PageHelper.ValidateNumber(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }
            var brokerlist = _brokerService.GetBrokerById(Convert.ToInt32(id));
            return PageHelper.toJson(new { List = brokerlist });
        }

        /// <summary>
        /// 通过User查找经纪人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBrokerByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }

            var model = _brokerService.GetBrokerByUserId(Convert.ToInt32(userId));

            if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在！"));

            var brokerInfo = new BrokerModel
            {
                Id = model.Id,
            
                Realname = model.Realname,
                Nickname = model.Nickname,
                Sexy = model.Sexy,
                Sfz = model.Sfz,
                Email = model.Email,
                Phone=model.Phone,
                Headphoto=model.Headphoto
            };

            return PageHelper.toJson(brokerInfo);
        }


        /// <summary>
        /// 会员列表查询操作
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
       [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchBrokers(EnumUserType userType,int? phone, string name = null,  int page = 1, int pageSize = 10)
        {
            //var phones = new int[1];

            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername=name,
                //Phones=phones,
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Page=Convert.ToInt32(page),
                PageCount=10,
                UserType = userType
            };
           
            var brokersList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                p.Id,
                p.Nickname,
                p.Brokername,
                p.Realname,
                p.Phone,
                p.Sfz,
                p.Amount,
                p.Agentlevel,
                p.Regtime,
                p.Headphoto
            }).ToList().Select(b => new
            {
                b.Id,
                b.Nickname,
                b.Brokername,
                b.Realname,
                b.Phone,
                b.Sfz,
                b.Amount,
                b.Agentlevel,
                Regtime = b.Regtime.ToString("yyyy-MM-dd"),
                b.Headphoto
            });
            var brokerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = brokersList, Condition = brokerSearchCondition, totalCount = brokerListCount });
        }

        /// <summary>
        /// 新增经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBroker([FromBody] BrokerEntity broker)
        {

            if (!string.IsNullOrEmpty(broker.Brokername) )
            {
                var brokerEntity = new BrokerEntity
                {
                    Brokername=broker.Brokername,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_brokerService.Create(brokerEntity) != null)
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
        /// 修改经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateBroker([FromBody] BrokerEntity broker)
        {
            if (broker != null && !string.IsNullOrEmpty(broker.Id.ToString()) && PageHelper.ValidateNumber(broker.Id.ToString()) )
            {
                var brokerModel = _brokerService.GetBrokerById(broker.Id);
                brokerModel.Headphoto = broker.Headphoto;
                brokerModel.Nickname = broker.Nickname;
                brokerModel.Phone = broker.Phone;
                brokerModel.Sfz = broker.Sfz;
                brokerModel.Email = broker.Email;
                brokerModel.Realname = broker.Realname;
                brokerModel.Sexy = broker.Sexy;

                #region 转职经纪人 杨定鹏 2015年6月11日17:29:58
                //填写身份证，邮箱，和真实姓名后就能转职经纪人
                if (!string.IsNullOrEmpty(broker.Email) && !string.IsNullOrEmpty(broker.Sfz) &&
                    !string.IsNullOrEmpty(broker.Realname))
                {
                    //权限变更

                    var brokerRole = _roleService.GetRoleByName("broker");

                    //User权限缺少时自动添加
                    if (brokerRole == null)
                    {
                        brokerRole = new Role
                        {
                            RoleName = "broker",
                            RolePermissions = null,
                            Status = RoleStatus.Normal,
                            Description = "user用户转职为broker"
                        };
                    }

                    var user = _userService.FindUser(brokerModel.UserId);
                    user.UserRoles.First().Role = brokerRole;
                    
                    //更新用户权限
                    if (_userService.ModifyUser(user))
                    {
                        //更新broker表记录
                        brokerModel.Usertype = EnumUserType.经纪人;
                        _brokerService.Update(brokerModel);
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                }

                #endregion


                try
                {
                    if (_brokerService.Update(brokerModel) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }


            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));

        }


        /// <summary>
        /// 删除经纪人(标为删除状态 不可恢复)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_brokerService.Delete(_brokerService.GetBrokerById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 注销经纪人(以后能够恢复)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CancelBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_brokerService.Delete(_brokerService.GetBrokerById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 经纪人排行 返回前10条
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage  OrderByBrokerList()
        {
          

            var brokersList = _brokerService.OrderbyBrokersList().Select(p => new
            {
                p.Id,
                p.Brokername,
                p.Agentlevel,
                p.Amount

            }).ToList();
            
            return PageHelper.toJson(new { List = brokersList});
        }


        /// <summary>
        /// 经纪人排行 返回前3条
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage OrderByBrokerTopThree()
        {


            var brokersList = _brokerService.OrderbyBrokersList().Select(p => new
            {
                p.Id,
                p.Brokername,
                p.Agentlevel,
                p.Amount

            }).Take(3).ToList();

            return PageHelper.toJson(new { List = brokersList });
        }



        /// <summary>
        /// 获取经纪人详细信息（合伙人个数  推荐人个数  客户个数  等级 排名 总佣金）
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
       public HttpResponseMessage  GetBrokerDetails()
        {
             var user = (UserBase)_workContext.CurrentUser;
             if (user != null)
             {
                 var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                 if (broker == null)
                 {
                     return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                 }
                 else
                 {
                     var partnerCount = 0;//合伙人个数
                     var refereeCount = 0;//推荐人个数
                     var customerCount = 0;//客户个数
                     var levelStr = "";//等级
                     var orderStr = "0";//排名
                     var allMoneys = "0";//总佣金

                     var partnerlistsearchcon = new PartnerListSearchCondition
                     {
                          Brokers=broker,
                          Status = EnumPartnerType.同意
                     };
                     partnerCount = _partnerlistService.GetPartnerListCount(partnerlistsearchcon);

                     var recomagmentsearchcon = new RecommendAgentSearchCondition
                     {
                         BrokerId =broker.Id
                     };
                     refereeCount = _recommendagentService.GetRecommendAgentCount(recomagmentsearchcon);

                     var condition = new ClientInfoSearchCondition
                     {
                      Addusers=broker.Id
                     };
                     customerCount = _clientInfoService.GetClientInfoCount(condition);

                     levelStr = broker.Agentlevel;

                     allMoneys = broker.Amount.ToString();

                     orderStr = GetOrdersByuserId(broker.Id.ToString());

                     return PageHelper.toJson(new { partnerCount = partnerCount, refereeCount = refereeCount, customerCount = customerCount, levelStr = levelStr, orderStr = orderStr, allMoneys = allMoneys ,photo=broker.Headphoto,Name=broker.Brokername});
                 }
             }
             return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }

        /// <summary>
        /// 获取经纪人的排名顺序
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetOrdersByuserId(string userid)
        {
            #region 排序实现
            List<ResultOrder> listOrder = new List<ResultOrder>();
            var brokerorderList = _brokerService.OrderbyAllBrokersList().ToList();
            var brokerorderlistArray = brokerorderList.ToArray();
            int count = 1;
            for (int i = 0; i < brokerorderlistArray.Length; i++)
            {
                 //确定是否到数组边界
                if (i + 1 < brokerorderlistArray.Length)
                {
                    //如果与list中下一位的Num数相等则 排名Count数不变
                    if (brokerorderlistArray[i].Amount == brokerorderlistArray[i + 1].Amount)
                    {
                        var item = new ResultOrder { Id = count, userId = brokerorderlistArray[i].Id.ToString(), Name = brokerorderlistArray[i].Brokername, Moneys = brokerorderlistArray[i].Amount };
                        listOrder.Add(item);
                    }
                    else
                    {
                        //如果与list中下一位的Num数不相等则 排名Count加1
                        var item = new ResultOrder { Id = count, userId = brokerorderlistArray[i].Id.ToString(), Name = brokerorderlistArray[i].Brokername, Moneys = brokerorderlistArray[i].Amount };
                        listOrder.Add(item);
                        count++;
                    }
                }
                //如果是最后一位了就直接添加
                else
                {
                    var item = new ResultOrder { Id = count, userId = brokerorderlistArray[i].Id.ToString(), Name = brokerorderlistArray[i].Brokername, Moneys = brokerorderlistArray[i].Amount };
                    listOrder.Add(item);
                }
            }
            #endregion

           if (listOrder.Count<=0)//无数据
           {
               return "1";
           }
            else
            {
                var resultOrder = listOrder.FirstOrDefault(o => o.userId == userid);
               if(resultOrder!=null)
               {
                   return resultOrder.Id.ToString();
               }
               else
               {
                   //没找到  
                   return (listOrder[listOrder.Count - 1].Id + 1).ToString();
               }
            }
           return "0";
        }

        #endregion


        /// <summary>
        /// 通过 邀请码获取发送者信息
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage  GetBrokerByInvitationCode([FromBody] string invitationCode)
        {
            if(!string.IsNullOrEmpty(invitationCode))
            {
                MessageDetailSearchCondition messageSearchcondition=new MessageDetailSearchCondition{
                      InvitationCode=invitationCode,
                       Title="推荐经纪人"
                };
                var messageDetail = _MessageService.GetMessageDetailsByCondition(messageSearchcondition).FirstOrDefault();
                if(messageDetail!=null)
                {
                    return PageHelper.toJson(new { invitationuserid=messageDetail.InvitationId });  
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));  
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));  
        }
    }


    /// <summary>
    /// 排名类
    /// </summary>
    public class ResultOrder
    {
        public int Id { get; set; } //排名Id

        public string userId { get; set; }//用户ID
        public string Name { get; set; }//用户姓名
        public decimal  Moneys { get; set; }//金额
    }
}
