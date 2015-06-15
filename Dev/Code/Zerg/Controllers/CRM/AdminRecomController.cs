using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using CRM.Service.Level;
using Trading.Entity.Model;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using YooPoon.Common.Encryption;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// admin的推荐至平台流程处理
    /// </summary>
    public class AdminRecomController : ApiController
    {
        private readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBrokerService _brokerService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILevelService _levelService;
        private readonly IRoleService _roleService;

        public AdminRecomController(IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService,
            IUserService userService,
            IWorkContext workContext,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ILevelService levelService,
            IRoleService roleService
            )
        {
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
            _userService = userService;
            _workContext = workContext;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _levelService = levelService;
            _roleService = roleService;
        }

        #region 经济人列表 杨定鹏 2015年5月4日14:29:24

        /// <summary>
        /// 经纪人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage BrokerList(EnumBRECCType status, string brokername,int page, int pageSize)
        {
           
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = EnumBrokerRECClientSearchOrderBy.OrderById,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Brokername=brokername
                    
            };
            var list = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername,
                a.Brokerlevel,
                a.ClientInfo.Phone,
                a.Projectname, 
                a.Addtime,

                a.Clientname,
                SecretaryName=a.SecretaryId.Brokername,
                a.SecretaryPhone,
                Waiter=a.WriterId.Brokername,
                a.WriterPhone,
                a.Uptime

            }).ToList().Select(b=>new
            {
                b.Id,
                b.Brokername,
                b.Brokerlevel,
                b.Phone,
                b.Projectname,
                Addtime =b.Addtime.ToString("yyy-MM-dd"),

                b.Clientname,
                SecretaryName = b.Brokername,
                b.SecretaryPhone,
                Waiter = b.Brokername,
                b.WriterPhone,
                Uptime=b.Uptime.ToString("yyy-MM-dd")
            });

            var totalCont = _brokerRecClientService.GetBrokerRECClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="brokerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddBroker([FromBody]BrokerModel brokerModel)
        {
            if (string.IsNullOrEmpty(brokerModel.UserName)) return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名不能为空"));
            if (string.IsNullOrEmpty(brokerModel.Password)) return PageHelper.toJson(PageHelper.ReturnValue(false, "密码不能为空"));
            if (string.IsNullOrEmpty(brokerModel.Phone)) return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号不能为空"));

            #region UC用户创建 杨定鹏 2015年5月28日14:52:48

            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Phone = brokerModel.Phone
            };

            //判断user表和Broker表中是否存在用户名
            int user2 = _brokerService.GetBrokerCount(condition);

            if (user2 != 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));

            //检测规则表中是否存在权限，不存在则添加
            var role = "broker";
            switch (brokerModel.UserType)
            {
                case EnumUserType.经纪人:
                    role = "broker";
                    break;
                case EnumUserType.商家:
                    role = "merchant";
                    break;
                case EnumUserType.场秘:
                    role = "secretary";
                    break;
                case EnumUserType.带客人员:
                    role = "waiter";
                    break;
                case EnumUserType.普通用户:
                    role = "user";
                    break;
                case EnumUserType.管理员:
                    role = "admin";
                    break;
                case EnumUserType.财务:
                    role = "accountant";
                    break;
            }

            var brokerRole = _roleService.GetRoleByName(role);

            //User权限缺少时自动添加
            if (brokerRole == null)
            {
                brokerRole = new Role
                {
                    RoleName = role,
                    RolePermissions = null,
                    Status = RoleStatus.Normal,
                    Description = "后台添加新权限类别：" + role
                };
            }

            var newUser = new UserBase
            {
                UserName = brokerModel.UserName,
                Password = brokerModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = brokerModel.UserName.ToLower(),
                //注册用户添加权限
                UserRoles = new List<UserRole>(){new UserRole()
                {
                    Role = brokerRole
                }},
                Status = 0
            };

            PasswordHelper.SetPasswordHashed(newUser, brokerModel.Password);

            #endregion

            #region Broker用户创建 杨定鹏 2015年5月28日14:53:32

            var model = new BrokerEntity();
            model.UserId = _userService.InsertUser(newUser).Id;
            model.Brokername = brokerModel.UserName;
            model.Phone = brokerModel.Phone;
            model.Totalpoints = 0;
            model.Amount = 0;
            model.Usertype = brokerModel.UserType;
            model.Regtime = DateTime.Now;
            model.State = 1;
            model.Adduser = 0;
            model.Addtime = DateTime.Now;
            model.Upuser = 0;
            model.Uptime = DateTime.Now;

            //判断初始等级是否存在,否则创建
            var level = _levelService.GetLevelsByCondition(new LevelSearchCondition { Name = "默认等级" }).FirstOrDefault();
            if (level == null)
            {
                var levelModel = new LevelEntity
                {
                    Name = "默认等级",
                    Describe = "系统默认初始创建",
                    Url = "",
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };
                _levelService.Create(levelModel);
            }

            model.Level = level;

            _brokerService.Create(model);

            #endregion

            return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
        }

        #endregion

        #region 待审核业务处理 杨定鹏 2015年5月5日16:28:30
        /// <summary>
        /// 审核状态变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAuditDetail(int id)
        {
            var model = _brokerRecClientService.GetBrokerRECClientById(id);
            var newModel = new BrokerRECClientModel
            {
                Id = model.Id,
                Broker = model.Broker.Id,
                NickName = model.Broker.Nickname,
                Brokername = model.Brokername,
                Brokerlevel = model.Brokerlevel,
                Sex = model.Broker.Sexy,
                RegTime = model.Broker.Regtime.ToString(CultureInfo.InvariantCulture),

                Clientname = model.Clientname,
                HouseType = model.ClientInfo.Housetype,
                Houses = model.ClientInfo.Houses,
                Note = model.ClientInfo.Note,
                Phone = model.Phone
            };

            return PageHelper.toJson(newModel);
        }

        /// <summary>
        /// 推荐流程变更操作,审核流程根据传入的Status字段进行相应变更
        /// 审核不通过，所有流程相关订单转入作废状态
        /// 等待上访，后台管理员确认审核通过，转入该阶段，订单关联驻场秘书和带客人员
        /// 洽谈中，驻场秘书确认客人带到，推荐订单转入结转状态，应生成账单（此处未实现 ）
        /// 客人未到，驻场秘书确认客人不来，所有订单转入作废状态
        /// 洽谈成功，成交订单转入结转状态，应生成账单（此处未实现 2015-6-15 10:50:08）
        /// 洽谈失败，成交订单转入作废状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PassAudit([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel.Id==0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "Id不能为空"));
            }

            var model = _brokerRecClientService.GetBrokerRECClientById(brokerRecClientModel.Id);
            model.Status = brokerRecClientModel.Status;
            model.Uptime = DateTime.Now;

            #region 推荐订单变更 杨定鹏 2015年6月4日17:38:08

            var recOrder =_orderService.GetOrderById(model.RecOrder);
            var dealOrder = _orderService.GetOrderById(model.DealOrder);

            //变更订单状态
            recOrder.Shipstatus = (int)brokerRecClientModel.Status;
            dealOrder.Shipstatus = (int)brokerRecClientModel.Status;

            //成交订单状态变更
            dealOrder.Upduser =_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            dealOrder.Upddate = DateTime.Now;
            
            //分支处理
            switch (brokerRecClientModel.Status)
            {
                case EnumBRECCType.审核不通过:
                    //订单作废
                    recOrder.Status = (int)EnumOrderStatus.审核失败;
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    recOrder.Upduser =_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.等待上访:
                    //审核通过
                    //添加带客和驻场秘书
                    //model.WriterId=brokerRecClientModel.

                    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈中:
                    //审核通过推荐订单
                    recOrder.Status = (int) EnumOrderStatus.审核通过;
                    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.客人未到:
                    //订单作废
                    recOrder.Status = (int)EnumOrderStatus.审核失败;
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    recOrder.Upduser =_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈成功:
                    //审核通过成交订单
                    recOrder.Status = (int) EnumOrderStatus.审核通过;
                    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈失败:
                    //成交订单作废
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    break;
            }

            #endregion

            _orderService.Update(recOrder);
            _orderService.Update(dealOrder);
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true,"确认成功"));
        }

        #region 选择带客人 杨定鹏 2015年5月5日19:45:14
        /// <summary>
        /// 带客人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WaiterList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                UserType = EnumUserType.带客人员
            };
            var list = _brokerService.GetBrokersByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername, 
                a.Phone
            }).ToList();
            return PageHelper.toJson(list);
        }

        #endregion

        #region 场秘管理 杨定鹏 2015年5月5日19:45:40
        /// <summary>
        /// 场秘列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SecretaryList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                UserType = EnumUserType.场秘
            };
            var list = _brokerService.GetBrokersByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername,
                a.Phone
            }).ToList();
            return PageHelper.toJson(list);
        }

        #endregion

        /// <summary>
        /// 确认成功/失败
        /// </summary>
        /// <param name="brokerRecClientModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Access([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var model = new BrokerRECClientEntity
            {
                Id=brokerRecClientModel.Id,
                Status = brokerRecClientModel.Status
            };
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }

        #endregion
    }
}
