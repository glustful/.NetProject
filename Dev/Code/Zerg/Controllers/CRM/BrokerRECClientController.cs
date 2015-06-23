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
                ////检测是否存在正在上访的推荐
                //if (_brokerRecClientService.GetBrokerRECClientsByCondition(sech).ToList().Any(p => p.Status == EnumBRECCType.等待上访))
                //{
                //    return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));
                //}
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在被推荐！"));
            }

            //创建订单逻辑白在审核通过后再传教 --Craig.Y.Duan
            //             #region 创建订单 杨定鹏 2015年6月3日17:21:39
            //
            //             //查询商品详情
            //             var product = _productService.GetProductById(brokerrecclient.Projectid);
            //
            //             #region 创建推荐订单 杨定鹏 2015年6月9日17:04:05
            //             //创建订单详情
            //             OrderDetailEntity ode = new OrderDetailEntity();
            //             ode.Adddate = DateTime.Now;
            //             ode.Adduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             ode.Commission = product.Commission;
            //             ode.RecCommission = product.RecCommission;
            //             ode.Dealcommission = product.Dealcommission;
            //             ode.Price = product.Price;
            //             ode.Product = product;
            //             ode.Productname = product.Productname;
            //             //ode.Remark = product.
            //             //ode.Snapshoturl = orderDetailModel.Snapshoturl,
            //             ode.Upddate = DateTime.Now;
            //             ode.Upduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //
            //             //创建订单
            //             OrderEntity oe = new OrderEntity();
            //             oe.Adddate = DateTime.Now;
            //             oe.Adduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             oe.AgentId = brokerrecclient.Adduser;
            //             oe.Agentname = _brokerService.GetBrokerByUserId(brokerrecclient.Adduser).Brokername;
            //             oe.Agenttel = brokerrecclient.Phone;
            //             oe.BusId = product.Bussnessid;
            //             oe.Busname = product.BussnessName;
            //             oe.Customname = brokerrecclient.Clientname;
            //             oe.Ordercode = _orderService.CreateOrderNumber(1);
            //             oe.OrderDetail = _orderDetailService.Create(ode);//创建订单详情；
            //             oe.Ordertype = EnumOrderType.推荐订单;
            //             oe.Remark = "前端经纪人提交";
            //             oe.Shipstatus = (int)EnumBRECCType.审核中;
            //             oe.Status = (int)EnumOrderStatus.默认;
            //             oe.Upddate = DateTime.Now;
            //             oe.Upduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             #endregion
            //
            //             #region 创建成交订单 杨定鹏 2015年6月9日17:04:05
            //
            //             //创建订单详情
            //             OrderDetailEntity ode2 = new OrderDetailEntity();
            //             ode2.Adddate = DateTime.Now;
            //             ode2.Adduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             ode2.Commission = product.Commission;
            //             ode2.RecCommission = product.RecCommission;
            //             ode2.Dealcommission = product.Dealcommission;
            //             ode2.Price = product.Price;
            //             ode2.Product = product;
            //             ode2.Productname = product.Productname;
            //             //ode.Remark = product.
            //             //ode.Snapshoturl = orderDetailModel.Snapshoturl,
            //             ode2.Upddate = DateTime.Now;
            //             ode2.Upduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //
            //             //创建订单
            //             OrderEntity oe2 = new OrderEntity();
            //             oe2.Adddate = DateTime.Now;
            //             oe2.Adduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             oe2.AgentId = brokerrecclient.Adduser;
            //             oe2.Agentname = _brokerService.GetBrokerByUserId(brokerrecclient.Adduser).Brokername;
            //             oe2.Agenttel = brokerrecclient.Phone;
            //             oe2.BusId = product.Bussnessid;
            //             oe2.Busname = product.BussnessName;
            //             oe2.Customname = brokerrecclient.Clientname;
            //             oe2.Ordercode = _orderService.CreateOrderNumber(3);
            //             oe2.OrderDetail = _orderDetailService.Create(ode2);//创建订单详情；
            //             oe2.Ordertype = EnumOrderType.成交订单;
            //             oe2.Remark = "前端经纪人提交";
            //             oe2.Shipstatus = (int)EnumBRECCType.审核中;
            //             oe2.Status = (int)EnumOrderStatus.默认;
            //             oe2.Upddate = DateTime.Now;
            //             oe2.Upduser = brokerrecclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //             #endregion
            //
            //             #endregion

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
            model.Brokerlevel = broker.Level.Name;
            model.Broker = broker;
            model.Adduser = brokerrecclient.Adduser;
            model.Addtime = DateTime.Now;
            model.Upuser = brokerrecclient.Adduser;
            model.Uptime = DateTime.Now;
            model.Projectid = brokerrecclient.Projectid;
            model.Projectname = brokerrecclient.Projectname;
            model.Status = EnumBRECCType.审核中;
            model.DelFlag = EnumDelFlag.默认;

            //             model.RecOrder = _orderService.Create(oe).Id;      //添加推荐订单；
            //             model.DealOrder =_orderService.Create(oe2).Id;       //添加成交订单

            _brokerRecClientService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }
    }
}
