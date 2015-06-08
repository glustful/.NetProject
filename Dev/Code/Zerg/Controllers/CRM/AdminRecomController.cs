using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using Trading.Entity.Model;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
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

        public AdminRecomController(IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService,
            IUserService userService,
            IWorkContext workContext,
            IOrderService orderService,
            IOrderDetailService orderDetailService
            )
        {
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
            _userService = userService;
            _workContext = workContext;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
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
            #region UC用户创建 杨定鹏 2015年5月28日14:52:48
            var user = _userService.GetUserByName(brokerModel.UserName);

            var condition = new BrokerSearchCondition
            {   
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Phone = brokerModel.Phone
            };

            //判断user表和Broker表中是否存在用户名
            var user2 = _brokerService.GetBrokerCount(condition);
            if (user != null || user2!=0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
            }
            var newUser = new UserBase
            {
                UserName = brokerModel.UserName,
                Password = brokerModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = brokerModel.UserName.ToLower(),
                Status = 0
            };
            PasswordHelper.SetPasswordHashed(newUser, brokerModel.Password);
            if (_userService.InsertUser(newUser).Id <= 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "注册用户失败，请重试"));
            }
            
            #endregion

            #region Broker用户创建 杨定鹏 2015年5月28日14:53:32

            var model = new BrokerEntity();
            model.UserId = _userService.GetUserByName(brokerModel.UserName).Id;
            model.Brokername = brokerModel.Brokername;
            model.Phone = brokerModel.Phone;
            model.Totalpoints = 0;
            model.Amount = 0;
            model.Usertype = brokerModel.UserType;
            model.Regtime = DateTime.Now;
            model.State = 1;
            model.Adduser = _workContext.CurrentUser.Id;
            model.Addtime = DateTime.Now;
            model.Upuser = _workContext.CurrentUser.Id;
            model.Uptime = DateTime.Now;

            //缺少等级

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
        /// 确认审核
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
            dealOrder.Upduser = "2"; //_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            dealOrder.Upddate = DateTime.Now;
            
            //分支处理
            switch (brokerRecClientModel.Status)
            {
                case EnumBRECCType.审核不通过:
                    //订单作废
                    recOrder.Status = (int)EnumOrderStatus.审核失败;
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    recOrder.Upduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈中:
                    //审核通过推荐订单
                    recOrder.Status = (int) EnumOrderStatus.审核通过;
                    recOrder.Upduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.客人未到:
                    //订单作废
                    recOrder.Status = (int)EnumOrderStatus.审核失败;
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    recOrder.Upduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈成功:
                    //审核通过成交订单
                    recOrder.Status = (int) EnumOrderStatus.审核通过;
                    recOrder.Upduser = "2";//_workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    recOrder.Upddate = DateTime.Now;
                    break;

                case EnumBRECCType.洽谈失败:
                    //成交订单作废
                    dealOrder.Status = (int)EnumOrderStatus.审核失败;
                    break;
            }

            #endregion

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
