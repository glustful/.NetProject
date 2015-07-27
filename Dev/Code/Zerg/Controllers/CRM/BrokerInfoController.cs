using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BrokeAccount;
using CRM.Service.Broker;
using CRM.Service.ClientInfo;
using CRM.Service.Event;
using CRM.Service.EventOrder;
using CRM.Service.InvitedCode;
using CRM.Service.Level;
using CRM.Service.MessageDetail;
using CRM.Service.PartnerList;
using CRM.Service.RecommendAgent;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
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
        private readonly IBrokeAccountService _brokerAccountService;
        private readonly IEventOrderService _eventOrderService;
        private readonly IInviteCodeService _inviteCodeService;
        private readonly ILevelService _levelService;

        /// <summary>
        /// 经纪人管理初始化
        /// </summary>
        /// <param name="clientInfoService">clientInfoService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="partnerlistService">partnerlistService</param>
        /// <param name="recommendagentService">recommendagentService</param>
        /// <param name="roleService">roleService</param>
        /// <param name="MessageService">MessageService</param>
        /// <param name="userService">userService</param>
        public BrokerInfoController(IClientInfoService clientInfoService,
            IWorkContext workContext,
            IBrokerService brokerService,
            IPartnerListService partnerlistService,
            IRecommendAgentService recommendagentService,
            IRoleService roleService,
            IMessageDetailService MessageService,
            IUserService userService,
            IBrokeAccountService brokerAccountService,
            IEventOrderService eventOrderService,
            IInviteCodeService inviteCodeService,
            ILevelService levelService
            )
        {
            _clientInfoService = clientInfoService;
            _workContext = workContext;
            _brokerService = brokerService;
            _partnerlistService = partnerlistService;
            _recommendagentService = recommendagentService;
            _roleService = roleService;
            _MessageService = MessageService;
            _userService = userService;
            _brokerAccountService = brokerAccountService;
            _eventOrderService = eventOrderService;
            _inviteCodeService = inviteCodeService;
            _levelService = levelService;
        }


        #region 经纪人管理



        /// <summary>
        /// 传入Id,检索经纪人,返回经纪人列表
        /// </summary>
        /// <param name="id">经纪人Id</param>
        /// <returns>经纪人列表</returns>
        [HttpGet]
        [Description("检索返回经纪人列表")]
        public HttpResponseMessage GetBroker(string id)
        {
            if (string.IsNullOrEmpty(id) || !PageHelper.ValidateNumber(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }
            var brokerlist = _brokerService.GetBrokerById(Convert.ToInt32(id));
            var dd = new BrokerModel
            {
                Address = brokerlist.Address,
                Adduser = brokerlist.Adduser,
                Brokername = brokerlist.Brokername,
                Phone = brokerlist.Phone,
                Realname = brokerlist.Realname,
                Nickname = brokerlist.Nickname,
                Sexy = brokerlist.Sexy,
                Sfz = brokerlist.Sfz,
                Email = brokerlist.Email,
                Headphoto = brokerlist.Headphoto,
                rgtime = brokerlist.Regtime.ToShortDateString()
            };
            return PageHelper.toJson(new { List = dd });
        }
        public HttpResponseMessage GetBrokerByAgent(string id)
        {
            if (string.IsNullOrEmpty(id) || !PageHelper.ValidateNumber(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }
            var brokerlist = _brokerService.GetBrokerById(Convert.ToInt32(id));
            var model = new BrokerModel
            {
                Headphoto = brokerlist.Headphoto,
                Nickname = brokerlist.Nickname,
                Brokername = brokerlist.Brokername,
                Realname = brokerlist.Realname,
                Sexy = brokerlist.Sexy,
                Totalpoints = brokerlist.Totalpoints,
                Amount = brokerlist.Amount,
                Phone = brokerlist.Phone,
                Qq = brokerlist.Qq,
                Email = brokerlist.Email,
                Sfz = brokerlist.Sfz,
                Regtime1 = brokerlist.Regtime.ToShortDateString(),
                Usertype1 = brokerlist.Usertype.ToString(),
                State = brokerlist.State,
                PartnersName = brokerlist.WeiXinNumber,
                Address = brokerlist.Address

            };
            return PageHelper.toJson(new { List = model });
        }

        /// <summary>
        /// 传入经纪人Id,检索经纪人信息,返回经纪人信息
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>经纪人信息</returns>

        //by yangyue  2015/7/21 改动邀请码验证------//
        [HttpGet]
        [Description("获取经纪人信息")]
        public HttpResponseMessage GetBrokerByUserId(string userId)
        {
            int IsInvite = 1; //是否使用邀请码（0使用 1未使用)
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }

            var model = _brokerService.GetBrokerByUserId(Convert.ToInt32(userId));
            if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在！"));

            #region 判断经纪人是否使用过邀请码
            InviteCodeSearchCondition icodeseCon = new InviteCodeSearchCondition
            {
                NumUser = model.Id
            };

            if (_inviteCodeService.GetInviteCodeByCount(icodeseCon) > 0)
            {
                IsInvite = 0; //判断有无使用过邀请码
            }

            #endregion

            var brokerInfo = new BrokerModel
            {
                Id = model.Id,
                Brokername = model.Brokername,
                Realname = model.Realname,
                Nickname = model.Nickname,
                Sexy = model.Sexy,
                Sfz = model.Sfz,
                Email = model.Email,
                Phone = model.Phone,
                Headphoto = model.Headphoto,
                WeiXinNumber = model.WeiXinNumber,
                IsInvite = IsInvite
            };
            return PageHelper.toJson(brokerInfo);
        }



        /// <summary>
        /// 传入会员参数,检索会员信息,返回会员列表
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="name">用户名称</param>
        /// <param name="phone">电话</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns></returns>
        [HttpGet]
        [Description("传入会员参数,获取会员列表")]
        public HttpResponseMessage SearchBrokers(EnumUserType userType, string phone = null, string name = null, int page = 1, int pageSize = 10, int state = 2)
        {
            //var phones = new int[1];

            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername = name,
                Phone = phone,
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Page = Convert.ToInt32(page),
                PageCount = 10,
                UserType = userType,
                State = state

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
                p.Headphoto,
                p.State,
                p.Usertype,
                btnVisibleDel = true,
                btnVisibleCan = true,
                btnname = "注销",
                btncolor = "",
                backcolor = ""
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
                b.Headphoto,
                b.State,
                b.Usertype,
                btnVisibleDel = true,
                btnVisibleCan = true
            });
            var brokerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = brokersList, Condition = brokerSearchCondition, totalCount = brokerListCount });
        }

        /// <summary>
        /// 传入经纪人参数,增加经纪人,返回增加结果状态信息
        /// </summary>
        /// <param name="broker">经纪人参数</param>
        /// <returns>新增经纪人结果状态信息</returns>
        [HttpPost]
        [Description("新增经纪人")]
        public HttpResponseMessage AddBroker([FromBody] BrokerEntity broker)
        {

            if (!string.IsNullOrEmpty(broker.Brokername))
            {
                var brokerEntity = new BrokerEntity
                {
                    Brokername = broker.Brokername,
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
        /// 传入经纪人参数,编辑修改经纪人信息,返回修改保存成功状态信息,成功返回"数据更新成功",失败返回"数据更新失败"
        /// </summary>
        /// <param name="broker">经纪人信息</param>
        /// <returns></returns>
        [HttpPost]
        [Description("修改经纪人信息")]
        public HttpResponseMessage UpdateBroker([FromBody] BrokerModel broker)
        {
            if (broker != null && !string.IsNullOrEmpty(broker.Id.ToString()) && PageHelper.ValidateNumber(broker.Id.ToString()))
            {
                var brokerModel = _brokerService.GetBrokerById(broker.Id);
                brokerModel.Headphoto = broker.Headphoto;
                brokerModel.Nickname = broker.Nickname;
                brokerModel.Phone = broker.Phone;
                brokerModel.Sfz = broker.Sfz;
                brokerModel.Email = broker.Email;
                brokerModel.Realname = broker.Realname;
                brokerModel.Sexy = broker.Sexy;
                brokerModel.WeiXinNumber = broker.WeiXinNumber;//by  yangyue  2015/7/16

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
                        //return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }


                }
                #endregion


                #region 邀请码逻辑 by yangyue  2015/7/16
                InviteCodeSearchCondition icodeseCon = new InviteCodeSearchCondition
                {
                    NumUser = brokerModel.Id,
                    State = 1
                };
                //判断有无使用过邀请码
                if (_inviteCodeService.GetInviteCodeByCount(icodeseCon) <= 0)//没使用过邀请码
                {
                    //邀请码不为空
                    if (!string.IsNullOrEmpty(broker.code))
                    {
                        var levelCon = new LevelSearchCondition
                        {
                            Name = "白银"
                        };
                        var level = _levelService.GetLevelsByCondition(levelCon).FirstOrDefault();

                         #region  白银逻辑

                        BrokerSearchCondition bsearchModel = new BrokerSearchCondition
                        {
                            Levels = level

                        };
                        //1判断白银等级人数是否《=3000 
                        if (_brokerService.GetBrokerCount(bsearchModel) <= 3000)
                        {
                            var invite = new InviteCodeSearchCondition
                            {

                                Number = broker.code,
                                State = 0
                            };
                            var con = _inviteCodeService.GetInviteCodeByCondition(invite).FirstOrDefault();//查询邀请码是否存在并且未使用

                            if (con != null) //存在 未使用
                            {
                                #region 添加到活动订单 经纪人账户表 修改经纪人等级 生成3个邀请码 并发送到手机
                                //添加活动订单信息
                                EventOrderEntity emodel = new EventOrderEntity();
                                emodel.AcDetail = "完整经济人资料奖励30元";
                                emodel.Addtime = DateTime.Now;
                                emodel.MoneyCount = 30;
                                emodel.Broker = brokerModel;
                                _eventOrderService.Create(emodel);

                                //添加到经纪人账户表
                                BrokeAccountEntity brokeraccountmodel = new BrokeAccountEntity();
                                brokeraccountmodel.MoneyDesc = "完整经济人资料奖励30元";
                                brokeraccountmodel.Balancenum = 30;
                                brokeraccountmodel.Adduser = brokerModel.Id;
                                brokeraccountmodel.Addtime = DateTime.Now;
                                brokeraccountmodel.Upuser = brokerModel.Id;
                                brokeraccountmodel.Uptime = DateTime.Now;
                                brokeraccountmodel.Broker = brokerModel;
                                brokeraccountmodel.Type = 2;
                                brokeraccountmodel.State = 0;
                                _brokerAccountService.Create(brokeraccountmodel);


                                //修改邀请码表信息
                                con.NumUser = brokerModel.Id;
                                con.UseTime = DateTime.Now;
                                con.State = 1;
                                _inviteCodeService.Update(con);

                                //更新等级
                                brokerModel.Level = level;                   
                                _brokerService.Update(brokerModel);
                                

                                //并且生成3个邀请码发送到手机端口 并插入库中
                                string randmNums = string.Empty;
                                for (int i = 0; i < 3; i++)
                                {
                                    string rans = GenerateRandomNumber(6);
                                    randmNums += rans + ",";

                                    InviteCodeEntity inviteCode = new InviteCodeEntity();
                                    inviteCode.CreatTime = DateTime.Now;
                                    inviteCode.Number = rans;
                                    inviteCode.UseTime = DateTime.Now;
                                    inviteCode.State = 0;
                                    inviteCode.Broker = brokerModel;
                                    _inviteCodeService.Create(inviteCode);                                  
                                }
                                SMSHelper.Sending(brokerModel.Phone, "恭喜您完善个人信息，奖励您三个邀请码：" + randmNums + "赶快邀请小伙伴们，惊喜等你哟！" + "【优客惠】");
                                #endregion
                            }
                            else //不存在 或已被使用
                            {
                                #region 邀请码不存在 或已被使用 就转为青铜逻辑

                                #region 青铜逻辑
                                var levelConn = new LevelSearchCondition
                                {
                                    Name = "青铜"
                                };
                                var qlevel = _levelService.GetLevelsByCondition(levelConn).FirstOrDefault();

                                BrokerSearchCondition qbsearchModel = new BrokerSearchCondition
                                {
                                    Levels = qlevel

                                };
                              
                                // 1判断青铜是否《=1000                             
                                if (_brokerService.GetBrokerCount(qbsearchModel) <= 1000)
                                {
                                    var qinvite = new InviteCodeSearchCondition
                                    {

                                        Number = broker.code,
                                        State = 0
                                    };
                                    var qcon = _inviteCodeService.GetInviteCodeByCondition(qinvite).FirstOrDefault();//查询邀请码是否存在并且未使用
                                    if (qcon != null)
                                    {
                                        EventOrderEntity emodel = new EventOrderEntity();
                                        emodel.AcDetail = "完整经济人资料无邀请码奖励20元";
                                        emodel.Addtime = DateTime.Now;
                                        emodel.MoneyCount = 20;
                                        emodel.Broker = brokerModel;
                                        _eventOrderService.Create(emodel);

                                        //添加到经纪人账户表
                                        BrokeAccountEntity brokeraccountmodel = new BrokeAccountEntity();
                                        brokeraccountmodel.MoneyDesc = "完整经济人资料奖励20元";
                                        brokeraccountmodel.Balancenum = 20;
                                        brokeraccountmodel.Adduser = brokerModel.Id;
                                        brokeraccountmodel.Addtime = DateTime.Now;
                                        brokeraccountmodel.Upuser = brokerModel.Id;
                                        brokeraccountmodel.Uptime = DateTime.Now;
                                        brokeraccountmodel.Broker = brokerModel;
                                        brokeraccountmodel.Type = 2;
                                        brokeraccountmodel.State = 0;
                                        _brokerAccountService.Create(brokeraccountmodel);


                                        brokerModel.Level = qlevel;
                                        _brokerService.Update(brokerModel);
                                        //给20元钱 等级设为青铜
                                    }
                                }
                                else
                                {
                                    //等级设为青铜
                                    brokerModel.Level = qlevel;
                                    _brokerService.Update(brokerModel);
                                   
                                }
                                #endregion

                                #endregion
                            }
                        }
                        else
                        {
                            //白银人数超过3000 等级设为白银
                            brokerModel.Level = level;
                            _brokerService.Update(brokerModel);
                        }

                        #endregion
                    }
                    else//邀请码没有填写 给20元钱 等级设为青铜
                    {
                        #region 没有填写邀请码 给20元钱 等级设为青铜
                        var levelConn = new LevelSearchCondition
                        {
                            Name = "青铜"
                        };
                        var qlevel = _levelService.GetLevelsByCondition(levelConn).FirstOrDefault(); //等级为青铜
                        //判断青铜是否《=1000 
                        BrokerSearchCondition qbsearchModel = new BrokerSearchCondition
                        {
                            Levels = qlevel

                        };

                        if (_brokerService.GetBrokerCount(qbsearchModel) <= 1000) //判断青铜是否《=1000 
                        {
                            //青铜等级人数《=1000 给20元钱 等级设为青铜

                            //添加到活动订单表
                            EventOrderEntity emodel = new EventOrderEntity();
                            emodel.AcDetail = "完整经济人资料无邀请码奖励20元";
                            emodel.Addtime = DateTime.Now;
                            emodel.MoneyCount = 20;
                            emodel.Broker = brokerModel;
                            _eventOrderService.Create(emodel);

                            //添加到经纪人账户表
                            BrokeAccountEntity brokeraccountmodel = new BrokeAccountEntity();
                            brokeraccountmodel.MoneyDesc = "完整经济人资料奖励20元";
                            brokeraccountmodel.Balancenum = 20;
                            brokeraccountmodel.Adduser = brokerModel.Id;
                            brokeraccountmodel.Addtime = DateTime.Now;
                            brokeraccountmodel.Upuser = brokerModel.Id;
                            brokeraccountmodel.Uptime = DateTime.Now;
                            brokeraccountmodel.Broker = brokerModel;
                            brokeraccountmodel.Type = 2;
                            brokeraccountmodel.State = 0;
                            _brokerAccountService.Create(brokeraccountmodel);


                            brokerModel.Level = qlevel;
                            _brokerService.Update(brokerModel);


                        }
                        else
                        {
                            //青铜人数已经超过1000人 则等级直接设为青铜
                            brokerModel.Level = qlevel;
                            _brokerService.Update(brokerModel);
                        }
                        #endregion
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


        #region  生成邀请码  by yangyue

        private static char[] constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',  
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            int[] array = new int[3];

            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }

            return newRandom.ToString();


        }
        #endregion



        /// <summary>
        /// 传入经纪人ID,删除经纪人,返回删除结果状态信息,成功提示＂数据删除成功＂，失败提示＂数据删除失败＂
        /// </summary>
        /// <param name="id">经纪人ID</param>
        /// <returns>经纪人删除结果状态信息</returns>
        [HttpPost]
        [Description("删除经纪人")]
        public HttpResponseMessage DeleteBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                var broker = _brokerService.GetBrokerById(Convert.ToInt32(id));
                broker.State = 0;
                if (_brokerService.Update(broker) != null)
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
        /// 传入经纪人Id,注销经纪人(不是物理删除,只是逻辑上的删除,以后可以恢复),返回注销结果状态提示,成功提示"数据删除成功",失败提示"数据删除失败"
        /// </summary>
        /// <param name="id">经纪人ID</param>
        /// <returns>经纪人数据删除结果状态信息</returns>
        [HttpPost]
        public HttpResponseMessage CancelBroker(string id, string btnname = "")
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                var broker = _brokerService.GetBrokerById(Convert.ToInt32(id));
                if (btnname == "注销")
                    broker.State = -1;
                else if (btnname == "恢复")
                    broker.State = 1;
                if (_brokerService.Update(broker) != null)
                {
                    if (btnname == "注销")
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功注销！"));
                    else if (btnname == "恢复")
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功恢复！"));
                }
                else
                {
                    if (btnname == "注销")
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据注销失败！"));
                    else if (btnname == "恢复")
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据恢复失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 检索经纪人列表,返回经纪人列表前10条
        /// </summary>
        /// <returns>经纪人列表</returns>

        [HttpGet]
        [Description("检索经纪人列表,返回经纪人列表前10条")]
        public HttpResponseMessage OrderByBrokerList()
        {


            var brokersList = _brokerService.OrderbyBrokersList().Select(p => new
            {
                p.Id,
                p.Brokername,
                p.Agentlevel,
                p.Amount

            }).ToList();

            return PageHelper.toJson(new { List = brokersList });
        }



        /// <summary>
        /// 检索经纪人列表,返回经纪人列表前3条
        /// </summary>
        /// <returns>经纪人列表</returns>
        [Description("获取前3的经纪人列表")]

        [HttpGet]
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
        /// <returns>经纪人详细信息</returns>
        [HttpGet]
        [Description("获取经纪人详细信息")]
        public HttpResponseMessage GetBrokerDetails()
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
                    //更新到经纪人表中 可用金额
                    broker.Amount = Convert.ToDecimal(GetBrokerAmount());
                    _brokerService.Update(broker);


                    var partnerCount = 0;//合伙人个数
                    var refereeCount = 0;//推荐人个数
                    var customerCount = 0;//客户个数
                    var levelStr = "";//等级
                    var orderStr = "0";//排名
                    var allMoneys = "0";//总佣金

                    var partnerlistsearchcon = new PartnerListSearchCondition
                    {
                        Brokers = broker,
                        Status = EnumPartnerType.同意
                    };
                    partnerCount = _partnerlistService.GetPartnerListCount(partnerlistsearchcon);

                    var recomagmentsearchcon = new RecommendAgentSearchCondition
                    {
                        BrokerId = broker.Id
                    };
                    refereeCount = _recommendagentService.GetRecommendAgentCount(recomagmentsearchcon);

                    var condition = new ClientInfoSearchCondition
                    {
                        Addusers = broker.Id
                    };
                    customerCount = _clientInfoService.GetClientInfoCount(condition);

                    levelStr = broker.Level.Name;

                    allMoneys = broker.Amount.ToString();

                    orderStr = GetOrdersByuserId(broker.Id.ToString());

                    return PageHelper.toJson(new { partnerCount = partnerCount, refereeCount = refereeCount, customerCount = customerCount, levelStr = levelStr, orderStr = orderStr, allMoneys = allMoneys, photo = broker.Headphoto, Name = broker.Brokername });
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }


        /// <summary>
        /// 计算经纪人的剩余账户金额 （账户金额表 和提现表相减）
        /// </summary>
        /// <returns>经纪人剩余账户</returns>
        /// 
        [Description("查询经纪人账户余额")]
        public string GetBrokerAmount()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    BrokeAccountSearchCondition broconditon = new BrokeAccountSearchCondition
                    {
                        Brokers = broker,
                        State = 0
                    };
                    //BrokerWithdrawDetailSearchCondition browithdetailcon = new BrokerWithdrawDetailSearchCondition
                    //{
                    //    Brokers = broker,
                    //    Type = "1"

                    //};
                    //decimal AddMoneys = _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Count() > 0 ? _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Sum(o => o.Balancenum) : 0;//新增的金额总和
                    //decimal TxMoneys = _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(browithdetailcon).Count() > 0 ? _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(browithdetailcon).Sum(o => o.Withdrawnum) : 0;//提现的总金额
                    //return (AddMoneys - TxMoneys).ToString();

                    return _brokerAccountService.GetBrokeAccountsByCondition(broconditon).Count() > 0 ? _brokerAccountService.GetBrokeAccountsByCondition(broconditon).Sum(o => o.Balancenum).ToString() : "0";//金额总和
                }
            }
            return "";

        }




        
        /// <summary>
        /// 传入经纪人ID,检索经纪人,返回经纪人排名顺序
        /// </summary>
        /// <param name="userid">经纪人ID</param>
        /// <returns>经纪人排名顺序</returns>
        /// 
        [Description("获取经纪人排名顺序")]
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

            if (listOrder.Count <= 0)//无数据
            {
                return "1";
            }
            else
            {
                var resultOrder = listOrder.FirstOrDefault(o => o.userId == userid);
                if (resultOrder != null)
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
        /// 通过 邀请码获取发送者信息 (UserController注册也同一判断)
        /// </summary>
        /// <param name="invitationCode">邀请码</param>
        /// <returns>发送者信息</returns>
        [HttpPost]
        [Description("邀请码发送者信息")]
        public HttpResponseMessage GetBrokerByInvitationCode([FromBody] string invitationCode)
        {
            if (!string.IsNullOrEmpty(invitationCode))
            {
                MessageDetailSearchCondition messageSearchcondition = new MessageDetailSearchCondition
                {
                    InvitationCode = invitationCode,
                    Title = "推荐经纪人"
                };
                var messageDetail = _MessageService.GetMessageDetailsByCondition(messageSearchcondition).FirstOrDefault();
                if (messageDetail != null)
                {
                    return PageHelper.toJson(new { invitationCode = invitationCode });
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));
        }





    }


    /// <summary>
    /// 排名类
    /// </summary>
    [Description("排名类")]
    public class ResultOrder
    {
        public int Id { get; set; } //排名Id

        public string userId { get; set; }//用户ID
        public string Name { get; set; }//用户姓名
        public decimal Moneys { get; set; }//金额
    }
}
