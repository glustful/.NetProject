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
using Trading.Service.Product;
using YooPoon.Common.Encryption;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
     [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// admin的推荐至平台流程处理
    /// </summary>
    [Description("Andmin的推荐平台流程处理")]
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
        private readonly IProductService _productService;
        /// <summary>
        /// andmin推荐平台流程处理
        /// </summary>
        /// <param name="brokerRecClientService">brokerRecClientService</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="userService">userService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="orderService">orderService</param>
        /// <param name="orderDetailService">orderDetailService</param>
        /// <param name="levelService">levelService</param>
        /// <param name="roleService">roleService</param>
        /// <param name="productService">productService</param>
        public AdminRecomController(IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService,
            IUserService userService,
            IWorkContext workContext,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ILevelService levelService,
            IRoleService roleService,
            IProductService productService
            )
        {
            _productService = productService;
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
            _userService = userService;
            _workContext = workContext;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _levelService = levelService;
            _roleService = roleService;
        }
        #region 经纪人列表
        /// <summary>
        /// 传入经纪人推荐类型,经纪人名称,页面设置等,查询经济人列表,返回经纪人列表 
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="orderByAll">排序参数{序号（OrderById），经纪人名（OrderByBrokername），经纪人等级（OrderByBrokerlevel），
        ///客户姓名（OrderByClientname），客户电话（OrderByPhone），项目名称（OrderByProjectname），
        ///推荐时间（OrderByAddtime）负责场秘（OrderBySecretaryName），带客人员（OrderByWaiter），项目名称（OrderByProjectname），审核时间（OrderByUptime）}</param>
        /// <param name="isDes">是否降序</param>
        /// <param name="brokername">经纪人名称或者客户名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人列表</returns>
        [HttpGet]
        [Description("查询经纪人列表")]
        public HttpResponseMessage BrokerList(EnumBRECCType status, string brokername, int page, int pageSize, EnumBrokerRECClientSearchOrderBy orderByAll = EnumBrokerRECClientSearchOrderBy .OrderByTime, bool isDes = true)
        {

            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = orderByAll,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Clientname = brokername,
                IsDescending =isDes 

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
                SecretaryName = a.SecretaryId.Brokername,
                a.SecretaryPhone,
                Waiter = a.WriterId.Brokername,
                a.WriterPhone,
                a.Uptime

            }).ToList().Select(b => new
            {
                b.Id,
                b.Brokername,
                b.Brokerlevel,
                b.Phone,
                b.Projectname,
                Addtime = b.Addtime.ToString("yyy-MM-dd"),

                b.Clientname,
                SecretaryName = b.Brokername,
                b.SecretaryPhone,
                Waiter = b.Brokername,
                b.WriterPhone,
                Uptime = b.Uptime.ToString("yyy-MM-dd")
            });

            var totalCont = _brokerRecClientService.GetBrokerRECClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }
        #endregion
        #region 添加一个推荐用户
        /// <summary>
        /// 传入经济人参数,添加一个经济人,返回添加结果状态信息.成功提示"注册成功"
        /// </summary>
        /// <param name="brokerModel">经济人参数</param>
        /// <returns>注册结果状态信息</returns>
        [HttpPost]
        [Description("经济人注册")]
        public HttpResponseMessage AddBroker([FromBody]BrokerModel brokerModel)
        {
            if (string.IsNullOrEmpty(brokerModel.UserName)) return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名不能为空"));
            if (string.IsNullOrEmpty(brokerModel.Password)) return PageHelper.toJson(PageHelper.ReturnValue(false, "密码不能为空"));
            if (string.IsNullOrEmpty(brokerModel.Phone)) return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号不能为空"));
            // 创建推荐用户
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
            return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
        }
        #endregion
        #region 待审核业务处理 杨定鹏 2015年5月5日16:28:30
        /// <summary>
        /// 审核状态变更
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>审核后的经纪人</returns>
        [HttpGet]
        [Description("审核状态变更")]
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
                RegTime = model.Broker.Regtime.ToString("yyy-MM-dd"),

                Clientname = model.Clientname,
                HouseType = model.ClientInfo.Housetype,
                Houses = model.ClientInfo.Houses,
                Note = model.ClientInfo.Note,
                BrokerPhone = model.Phone,
                Phone = model.ClientInfo.Phone,
                Projectname = model.Projectname
               
            };

            return PageHelper.toJson(newModel);
        }
        #endregion
        #region 推荐流程变更操作
        /// <summary>
        /// 推荐流程变更操作,审核流程根据传入的Status字段进行相应变更
        /// 审核不通过，所有流程相关订单转入作废状态
        /// 等待上访，后台管理员确认审核通过，转入该阶段，订单关联驻场秘书和带客人员
        /// 洽谈中，驻场秘书确认客人带到，推荐订单转入结转状态，应生成账单（此处未实现 ）
        /// 客人未到，驻场秘书确认客人不来，所有订单转入作废状态
        /// 洽谈成功，成交订单转入结转状态，应生成账单（此处未实现 2015-6-15 10:50:08）
        /// 洽谈失败，成交订单转入作废状态
        /// </summary>
        /// <param name="brokerRecClientModel">经济人推荐参数</param>
        /// <returns>确认结果状态信息</returns>
        [HttpPost]
        [Description("通过推荐")]
        public HttpResponseMessage PassAudit([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel.Id == 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "Id不能为空"));
            }

            var model = _brokerRecClientService.GetBrokerRECClientById(brokerRecClientModel.Id);
            model.Status = brokerRecClientModel.Status;

            //if (brokerRecClientModel.Status == EnumBRECCType.等待上访)
            //{
            //    //查询商品详情
            //    var product = _productService.GetProductById(model.Projectid);

            //    //创建订单详情
            //    OrderDetailEntity ode = new OrderDetailEntity();
            //    ode.Adddate = DateTime.Now;
            //    ode.Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
            //    ode.Commission = product.Commission;
            //    ode.RecCommission = product.RecCommission;
            //    ode.Dealcommission = product.Dealcommission;
            //    ode.Price = product.Price;
            //    ode.Product = product;
            //    ode.Productname = product.Productname;
            //    //ode.Remark = product.
            //    //ode.Snapshoturl = orderDetailModel.Snapshoturl,
            //    ode.Upddate = DateTime.Now;
            //    ode.Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture);

            //    //创建订单
            //    OrderEntity oe = new OrderEntity();
            //    oe.Adddate = DateTime.Now;
            //    oe.Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
            //    oe.AgentId = model.Broker.Id;//model.Adduser;此处存的应该是经纪人ID
            //    oe.Agentname = _brokerService.GetBrokerByUserId(model.Adduser).Brokername;
            //    oe.Agenttel = model.Phone;
            //    oe.BusId = product.Bussnessid;
            //    oe.Busname = product.BussnessName;
            //    oe.Customname = model.Clientname;
            //    oe.Ordercode = _orderService.CreateOrderNumber(1);
            //    oe.OrderDetail = ode;//订单详情；
            //    oe.Ordertype = EnumOrderType.推荐订单;
            //    oe.Remark = "前端经纪人提交";
            //    oe.Shipstatus = (int)EnumBRECCType.审核中;
            //    oe.Status = (int)EnumOrderStatus.默认;
            //    oe.Upddate = DateTime.Now;
            //    oe.Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
               
            //    _orderService.Create(oe);
            //    model.RecOrder = oe.Id;
            //     _brokerRecClientService.Update(model);
            //}
            //else if (brokerRecClientModel.Status == EnumBRECCType.审核不通过) { return PageHelper.toJson(PageHelper.ReturnValue(false, "审核不通过")); }
                  
            //else if (brokerRecClientModel.Status == EnumBRECCType.审核中)
            //{}
            //else if (brokerRecClientModel.Status == EnumBRECCType.洽谈中) 
            //{
            //    var recOrder = _orderService.GetOrderById(model.RecOrder);
            //    int a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
            //    recOrder.Shipstatus = a;
            //    recOrder.Status = (int)EnumOrderStatus.审核通过;
            //    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    recOrder.Upddate = DateTime.Now;
            //    _orderService.Update(recOrder);
            //}
            //else if (brokerRecClientModel.Status == EnumBRECCType.洽谈成功) 
            //{
            //    var recOrder = _orderService.GetOrderById(model.RecOrder);
            //    int a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
            //    recOrder.Shipstatus = a;
            //    recOrder.Status = (int)EnumOrderStatus.审核通过;
            //    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    recOrder.Upddate = DateTime.Now;
            //    _orderService.Update(recOrder);
            //}
            //else if (brokerRecClientModel.Status == EnumBRECCType.洽谈失败) 
            //{
            //    var recOrder = _orderService.GetOrderById(model.RecOrder);
            //    int a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
            //    recOrder.Shipstatus = a;
            //    model.DelFlag = EnumDelFlag.删除;
            //    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    recOrder.Upddate = DateTime.Now;
            //    _orderService.Update(recOrder);
            //}
            //else if (brokerRecClientModel.Status == EnumBRECCType.客人未到) 
            //{
            //    var recOrder = _orderService.GetOrderById(model.RecOrder);
            //    int a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
            //    recOrder.Shipstatus = a;
            //    recOrder.Status = (int)EnumOrderStatus.审核失败;
            //    recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    recOrder.Upddate = DateTime.Now;
            //    _orderService.Update(recOrder);
            //}
            switch (brokerRecClientModel.Status) 
            {
                case EnumBRECCType.审核中:
                  
                    break;
                case EnumBRECCType.审核不通过:
                    _brokerRecClientService.Delete(model);
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "操作成功"));
                case EnumBRECCType.等待上访:
                    //查询商品详情
                    var product = _productService.GetProductById(model.Projectid);
                    //创建订单详情
                    OrderDetailEntity ode = new OrderDetailEntity();
                    ode.Adddate = DateTime.Now;
                    ode.Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
                    ode.Commission = product.Commission;
                    ode.RecCommission = product.RecCommission;
                    ode.Dealcommission = product.Dealcommission;
                    ode.Price = product.Price;
                    ode.Product = product;
                    ode.Productname = product.Productname;
                    //ode.Remark = product.
                    //ode.Snapshoturl = orderDetailModel.Snapshoturl,
                    ode.Upddate = DateTime.Now;
                    ode.Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
                    //创建订单
                    OrderEntity oe = new OrderEntity();
                    oe.Adddate = DateTime.Now;
                    oe.Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
                    oe.AgentId = model.Broker.Id;//model.Adduser;此处存的应该是经纪人ID
                    oe.Agentname = _brokerService.GetBrokerByUserId(model.Adduser).Brokername;
                    oe.Agenttel = model.Phone;
                    oe.BusId = product.Bussnessid;
                    oe.Busname = product.BussnessName;
                    oe.Customname = model.Clientname;
                    oe.Ordercode = _orderService.CreateOrderNumber(1);
                    oe.OrderDetail = ode;//订单详情；
                    oe.Ordertype = EnumOrderType.推荐订单;
                    oe.Remark = "前端经纪人提交";
                    oe.Shipstatus = (int)EnumBRECCType.审核中;
                    oe.Status = (int)EnumOrderStatus.默认;
                    oe.Upddate = DateTime.Now;
                    oe.Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture);
                    _orderService.Create(oe);
                    model.RecOrder = oe.Id;
                    _brokerRecClientService.Update(model);
                    break;
                case EnumBRECCType.洽谈中:
                        var recOrder = _orderService.GetOrderById(model.RecOrder);
                        int a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
                        recOrder.Shipstatus = a;
                        recOrder.Status = (int)EnumOrderStatus.审核通过;
                        recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        recOrder.Upddate = DateTime.Now;
                        _orderService.Update(recOrder);
                    break;
                case EnumBRECCType.洽谈成功:
                        recOrder = _orderService.GetOrderById(model.RecOrder);
                        a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
                        recOrder.Shipstatus = a;
                        recOrder.Status = (int)EnumOrderStatus.审核通过;
                        recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        recOrder.Upddate = DateTime.Now;
                        _orderService.Update(recOrder);
                    break;
                case EnumBRECCType.洽谈失败:
                        recOrder = _orderService.GetOrderById(model.RecOrder);
                        a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
                        recOrder.Shipstatus = a;
                        model.DelFlag = EnumDelFlag.删除;
                        recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        recOrder.Upddate = DateTime.Now;
                        _orderService.Update(recOrder);
                    break;
                case EnumBRECCType.客人未到:
                        recOrder = _orderService.GetOrderById(model.RecOrder);
                        a = (int)Enum.Parse(typeof(EnumBRECCType), brokerRecClientModel.Status.ToString());
                        recOrder.Shipstatus = a;
                        recOrder.Status = (int)EnumOrderStatus.审核失败;
                        recOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        recOrder.Upddate = DateTime.Now;
                        _orderService.Update(recOrder);
                    break;
            }
            model.Uptime = DateTime.Now;
            model.Upuser = _workContext.CurrentUser.Id;
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "确认成功"));
        }
        #endregion 
        #region 选择带客人 杨定鹏 2015年5月5日19:45:14
        /// <summary>
        /// 查询待客列表,返回待客列表
        /// </summary>
        /// <returns>待客列表</returns>
        [HttpGet]
        [Description("查询待客列表,返回待客列表")]
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
        /// 查询场秘列表,返回场秘列表
        /// </summary>
        /// <returns>场秘列表</returns>
        [HttpGet]
        [Description("查询场秘列表")]
        public HttpResponseMessage SecretaryList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                UserType = EnumUserType.场秘,
                State=1
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
        #region 经纪人确认提交
        /// <summary>
        /// 传入经济人推荐参数,确认成功提交
        /// </summary>
        /// <param name="brokerRecClientModel">经济人参数</param>
        /// <returns>提交结果状态信息</returns>

        [Description("经纪人推荐确认提交")]
        public HttpResponseMessage Access([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var model = new BrokerRECClientEntity
            {
                Id = brokerRecClientModel.Id,
                Status = brokerRecClientModel.Status
            };
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));        
        }
        #endregion
       
    }
}     