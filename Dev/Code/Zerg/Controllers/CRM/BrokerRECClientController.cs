using System.Globalization;
using System.Web.Mvc;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Service.ClientInfo;
using Newtonsoft.Json.Linq;
using Trading.Entity.Model;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using Trading.Service.Product;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Controllers.Trading.Trading.Order;
using Zerg.Models.CRM;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [System.Web.Http.AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]

    //经纪人推荐客户
    [Description("经纪人推荐客户类")]
    public class BrokerRECClientController : ApiController
    {
        public readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBrokerService _brokerService;//经纪人
        private readonly IClientInfoService _clientInfoService;
        private readonly IWorkContext _workContext;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        /// <summary>
        /// 经纪人推荐客户初始化
        /// </summary>
        /// <param name="brokerRecClientService">brokerRecClientService</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="clientInfoService">clientInfoService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="productService">productService</param>
        /// <param name="orderDetailService">orderDetailService</param>
        /// <param name="orderService">orderService</param>
        public BrokerRECClientController(
            IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService,
            IClientInfoService clientInfoService,
            IWorkContext workContext,
            IProductService productService,
            IOrderDetailService orderDetailService,
            IOrderService orderService

            )
        {
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
            _clientInfoService = clientInfoService;
            _workContext = workContext;
            _productService = productService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
        }




        /// <summary>
        /// 传入经纪人ID,检索经纪人推进客户,返回经纪人推荐客户列表
        /// </summary>
        /// <param name="userid">经纪人ID</param>
        /// <returns>经纪人推荐客户列表</returns>
        [System.Web.Http.HttpGet]
        [Description("获取经纪人推荐客户列表")]
        public HttpResponseMessage SearchBrokerRecClient(string userid)
        {
            var sech = new BrokerRECClientSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userid))
            };
            var list = _brokerRecClientService.GetBrokerRECClientsByCondition(sech).Select(p => new
            {
                p.Brokerlevel,
                p.Brokername,
                p.ClientInfo.Phone,
                ProjectName = p.Projectid == 0 ? "" : _productService.GetProductById(p.Projectid).Productname,
            }).ToList();
            return PageHelper.toJson(new { List = list });

        }


        /// <summary>
        /// 创建一个推荐流程：
        /// 1、检查客户是否存在于数据库，如存在则检测是否有流程位于正在上访状态，如有，则跳出。
        /// 2、创建推荐订单和带客订单，初始订单状态置于审核中，随审核流程变更而变更。
        /// 3、创建推荐流程，关联经纪人，客户，订单，推荐。
        /// </summary>
        /// <param name="brokerrecclient">经纪人推荐参数</param>
        /// <returns>添加结果状态信息</returns>
        [Description("经纪人推荐客户")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Add([FromBody]BrokerRECClientModel brokerrecclient)
        {
            if (brokerrecclient.Adduser == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "经济人ID不能为空！"));
            if (brokerrecclient.Broker == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "经纪人ID不能为空"));
            if (string.IsNullOrEmpty(brokerrecclient.Clientname)) return PageHelper.toJson(PageHelper.ReturnValue(false, "客户名不能为空"));
            if (string.IsNullOrEmpty(brokerrecclient.Phone)) return PageHelper.toJson(PageHelper.ReturnValue(false, "客户电话不能为空！"));

            //查询客户信息
            var sech = new BrokerRECClientSearchCondition
            {
                Clientname = brokerrecclient.Clientname,
                Phone = brokerrecclient.Phone,
                Projectids = new[] { brokerrecclient.Projectid },
                DelFlag = EnumDelFlag.默认
            };

            var cmodel = _brokerRecClientService.GetBrokerRECClientsByCondition(sech);

            //检测客户是否存在于数据库
            if (!cmodel.Any())
            {
                //客户信息
                var client = new ClientInfoEntity
                {
                    Clientname = brokerrecclient.Clientname,
                    Phone = brokerrecclient.Phone.ToString(CultureInfo.InvariantCulture),
                    Housetype = brokerrecclient.HouseType,
                    Houses = brokerrecclient.Houses,
                    Note = brokerrecclient.Note,
                    Adduser = brokerrecclient.Broker,
                    Addtime = DateTime.Now,
                    Upuser = brokerrecclient.Broker,
                    Uptime = DateTime.Now
                };

                _clientInfoService.Create(client);
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在被推荐！"));
            }

            //查询客户信息
            var sech2 = new ClientInfoSearchCondition
            {
                Clientname = brokerrecclient.Clientname,
                Phone = brokerrecclient.Phone.ToString(CultureInfo.InvariantCulture),
            };
            var cmodel2 = _clientInfoService.GetClientInfosByCondition(sech2).FirstOrDefault();
            //查询经纪人信息
            var broker = _brokerService.GetBrokerByUserId(brokerrecclient.Adduser);

            //创建推荐流程
            var model = new BrokerRECClientEntity();
            model.Broker = _brokerService.GetBrokerById(brokerrecclient.Adduser);
            model.ClientInfo = cmodel2;
            model.Clientname = brokerrecclient.Clientname;
            //model.Qq = Convert.ToInt32(brokerrecclient.Qq);
            model.Phone = brokerrecclient.Phone;       //客户电话
            model.Brokername = broker.Brokername;
            //model.Brokerlevel = broker.Level.Name;
            model.Broker = broker;
            model.Adduser = brokerrecclient.Adduser;
            model.Addtime = DateTime.Now;
            model.Upuser = brokerrecclient.Adduser;
            model.Uptime = DateTime.Now;
            model.Projectid = brokerrecclient.Projectid;
            model.Projectname = brokerrecclient.Projectname;
            model.Status = EnumBRECCType.审核中;
            model.DelFlag = EnumDelFlag.默认;
            model.RecOrder = (int)EnumOrderType.推荐订单;

            _brokerRecClientService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }
    }
}
