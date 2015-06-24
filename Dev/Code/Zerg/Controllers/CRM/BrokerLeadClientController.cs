using System.Globalization;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerLeadClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Service.ClientInfo;
using Trading.Entity.Model;
using Trading.Service.Order;
using Trading.Service.OrderDetail;
using Trading.Service.Product;
using Trading.Service.ProductDetail;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 经纪人带客
    /// </summary>
    [Description("经纪人带客类")]
    public class BrokerLeadClientController : ApiController
    {
        private readonly IBrokerLeadClientService _brokerleadclientService;
        private readonly IBrokerService _brokerService;//经纪人
        private readonly IClientInfoService _clientInfoService;
        private readonly IWorkContext _workContext;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IOrderDetailService _orderDetailService;

        /// <summary>
        /// 经纪人带客初始化
        /// </summary>
        /// <param name="brokerleadclientService">brokerleadclientService</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="clientInfoService">clientInfoService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="orderService">orderService</param>
        /// <param name="productService">productService</param>
        /// <param name="orderDetailService">orderDetailService</param>
        public BrokerLeadClientController(
            IBrokerLeadClientService brokerleadclientService,
            IBrokerService brokerService,
            IClientInfoService clientInfoService,
            IWorkContext workContext,
            IOrderService orderService,
            IProductService productService,
            IOrderDetailService orderDetailService
            )
        {
            _brokerleadclientService = brokerleadclientService;
            _brokerService = brokerService;
            _clientInfoService = clientInfoService;
            _workContext = workContext;
            _orderService = orderService;
            _productService = productService;
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// 通过地区查询带客列表,返回带客列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人带客列表</returns>
        [HttpGet]
        public HttpResponseMessage GetBLCList(int page = 1, int pageSize = 10)
        {
            var sech = new BrokerLeadClientSearchCondition
            {
                OrderBy = EnumBrokerLeadClientSearchOrderBy.OrderById,
                Status = EnumBLeadType.预约中,
               
            };
            var list = _brokerleadclientService.GetBrokerLeadClientsByCondition(sech).Select(p => new
            {
                p.Id,
                p.Brokername,
                p.ClientName,
                p.ClientInfo.Phone,
                p.ProjectId,
                p.Appointmenttime
            }).ToList()==null? null : _brokerleadclientService.GetBrokerLeadClientsByCondition(sech).Select(p => new
            {
                p.Id,
                p.Brokername,
                p.ClientName,
                p.ClientInfo.Phone,
                p.ProjectId,
                p.Appointmenttime
            }).ToList().Select(a => new
            {
                a.Id,
                a.Brokername,
                a.ClientName,
                a.Phone,
                ProjectName =a.ProjectId==0?"":  _productService.GetProductById(a.ProjectId).Productname,
                Appointmenttime = a.Appointmenttime.ToString("yyy-MM-dd")
            });
            

            var count = _brokerleadclientService.GetBrokerLeadClientCount(sech);
            return PageHelper.toJson(new { List = list, Condition = sech, totalCount = count });
        }
        /// <summary>
        /// 查询某经纪人带客记录
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchBrokerLeadClient(string userid)
        {
            var sech = new BrokerLeadClientSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userid))
            };
            var list = _brokerleadclientService.GetBrokerLeadClientsByCondition(sech).Select(p => new
            {
                p.Id,
                p.Brokername,
                p.ClientName,
                p.ClientInfo.Phone,
                p.ProjectId,
                p.Appointmenttime
            }).ToList().Select(a => new 
            {
                a.Id,
                a.Brokername,
                a.ClientName,
                a.Phone,
                ProjectName = a.ProjectId == 0 ? "" : _productService.GetProductById(a.ProjectId).Productname,
                Appointmenttime = a.Appointmenttime.ToString("yyy-MM-dd")
            });
            return PageHelper.toJson(new { List = list});

        }
        /// <summary>
        /// 经纪人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLeadCientInfoByBrokerName(EnumBLeadType status, string brokername, int page, int pageSize)
        {

            var condition = new BrokerLeadClientSearchCondition
            {
                OrderBy = EnumBrokerLeadClientSearchOrderBy.OrderById,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Brokername = brokername

            };
            var list = _brokerleadclientService.GetBrokerLeadClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername,
                a.ClientInfo.Phone,
                a.Projectname,
                a.Addtime,

                
                SecretaryName = a.SecretaryId.Brokername,
                a.SecretaryPhone,
                Waiter = a.WriterId.Brokername,
                a.WriterPhone,
                a.Uptime

            }).ToList().Select(b => new
            {
                b.Id,
                b.Brokername,
               
                b.Phone,
                b.Projectname,
                Addtime = b.Addtime.ToString("yyy-MM-dd"),

                
                SecretaryName = b.Brokername,
                b.SecretaryPhone,
                Waiter = b.Brokername,
                b.WriterPhone,
                Uptime = b.Uptime.ToString("yyy-MM-dd")
            });

            var totalCont = _brokerleadclientService.GetBrokerLeadClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }

        /// <summary>
        /// 传入经纪人带客管理ID
        /// </summary>
        /// <param name="id">经纪人带客管理ID</param>
        /// <returns>经纪人带客详情</returns>
        [HttpGet]
        [Description("获取经纪人带客详情")]
        public HttpResponseMessage GetBlDetail(int id)
        {
            var model = _brokerleadclientService.GetBrokerLeadClientById(id);
            var detail = new BrokerLeadClientModel
            {
                Id = model.Id,
                Broker = model.Broker.Id,
                Brokername = model.Brokername,
                Brokerlevel = model.BrokerLevel,
                NickName = model.Broker.Nickname,
                BrokerPhone = model.Broker.Phone,
                Sex = model.Broker.Sexy,
                RegTime = model.Broker.Regtime.ToString("yyy-mm-dd"),
                Clientname = model.ClientName,
                HouseType = model.ProjectId == 0 ? "" : _productService.GetProductById(model.ProjectId).Productname,
                //HouseType = _productService.GetProductById(model.ProjectId).Productname,
                Phone = model.ClientInfo.Phone,
                Note = model.Details,
                Houses = model.ProjectId == 0 ? "" : _productService.GetProductById(model.ProjectId).SubTitle,
                Projectname = model.Projectname,
                Projectid = model.ProjectId,
                
            };

            return PageHelper.toJson(detail);
        }

        /// <summary>
        /// 添加一个带客记录
        /// </summary>
        /// <param name="brokerrecclient"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody] BrokerLeadClientModel brokerleadclient)
        {
            if (brokerleadclient.Adduser == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "经济人ID不能为空！"));
            if (string.IsNullOrEmpty(brokerleadclient.Clientname)) return PageHelper.toJson(PageHelper.ReturnValue(false, "客户名不能为空"));
            if (string.IsNullOrEmpty(brokerleadclient.Phone)) return PageHelper.toJson(PageHelper.ReturnValue(false, "客户电话不能为空！"));

            //查询客户信息
            var sech = new BrokerLeadClientSearchCondition
            {
                ClientName = brokerleadclient.Clientname,
                Phone = brokerleadclient.Phone,
                Projectids = new[] { brokerleadclient.Projectid },
                DelFlag = EnumDelFlag.默认
            };

            var cmodel = _brokerleadclientService.GetBrokerLeadClientsByCondition(sech).FirstOrDefault();

            //检测客户是否存在于数据库
            if (cmodel == null)
            {
                //客户信息
                var client = new ClientInfoEntity
                {
                    Clientname = brokerleadclient.Clientname,
                    Phone = brokerleadclient.Phone.ToString(CultureInfo.InvariantCulture),
                    Housetype = brokerleadclient.HouseType,
                    Houses = brokerleadclient.Houses,
                    Note = brokerleadclient.Note,
                    Adduser = brokerleadclient.Broker,
                    Addtime = DateTime.Now,
                    Upuser = brokerleadclient.Broker,
                    Uptime = DateTime.Now
                };

                _clientInfoService.Create(client);
            }
            else
            {
                ////检测是否存在正在上访的代客
                //if (_brokerRecClientService.GetBrokerRECClientsByCondition(sech).ToList().Any(p => p.Status == EnumBRECCType.等待上访))
                //{
                //    return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在上访！"));
                //}
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在被代客！"));
            }

            //            #region 创建订单 杨定鹏 2015年6月3日17:21:39
            //
            //查询商品详情
            //            var product = _productService.GetProductById(brokerleadclient.Projectid);
            //
            //            #region 创建带客订单 杨定鹏 2015年6月9日17:04:05
            //            //创建订单详情
            //            OrderDetailEntity ode = new OrderDetailEntity();
            //            ode.Adddate = DateTime.Now;
            //            ode.Adduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            ode.Commission = product.Commission;
            //            ode.RecCommission = product.RecCommission;
            //            ode.Dealcommission = product.Dealcommission;
            //            ode.Price = product.Price;
            //            ode.Product = product;
            //            ode.Productname = product.Productname;
            //            //ode.Remark = product.
            //            //ode.Snapshoturl = orderDetailModel.Snapshoturl,
            //            ode.Upddate = DateTime.Now;
            //            ode.Upduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //
            //            //创建订单
            //            OrderEntity oe = new OrderEntity();
            //            oe.Adddate = DateTime.Now;
            //            oe.Adduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            oe.AgentId = brokerleadclient.Adduser;
            //            oe.Agentname = _brokerService.GetBrokerByUserId(brokerleadclient.Adduser).Brokername;
            //            oe.Agenttel = brokerleadclient.Phone;
            //            oe.BusId = product.Bussnessid;
            //            oe.Busname = product.BussnessName;
            //            oe.Customname = brokerleadclient.Clientname;
            //            oe.Ordercode = _orderService.CreateOrderNumber(2);  //创建带客订单号
            //            oe.OrderDetail = _orderDetailService.Create(ode);//创建订单详情；
            //            oe.Ordertype = EnumOrderType.带客订单;
            //            oe.Remark = "前端经纪人提交";
            //            oe.Shipstatus = (int)EnumBRECCType.审核中;
            //            oe.Status = (int)EnumOrderStatus.默认;
            //            oe.Upddate = DateTime.Now;
            //            oe.Upduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            #endregion
            //
            //            #region 创建成交订单 杨定鹏 2015年6月9日17:04:05
            //
            //            //创建订单详情
            //            OrderDetailEntity ode2 = new OrderDetailEntity();
            //            ode2.Adddate = DateTime.Now;
            //            ode2.Adduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            ode2.Commission = product.Commission;
            //            ode2.RecCommission = product.RecCommission;
            //            ode2.Dealcommission = product.Dealcommission;
            //            ode2.Price = product.Price;
            //            ode2.Product = product;
            //            ode2.Productname = product.Productname;
            //            //ode.Remark = product.
            //            //ode.Snapshoturl = orderDetailModel.Snapshoturl,
            //            ode2.Upddate = DateTime.Now;
            //            ode2.Upduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //
            //            //创建订单
            //            OrderEntity oe2 = new OrderEntity();
            //            oe2.Adddate = DateTime.Now;
            //            oe2.Adduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            oe2.AgentId = brokerleadclient.Adduser;
            //            oe2.Agentname = _brokerService.GetBrokerByUserId(brokerleadclient.Adduser).Brokername;
            //            oe2.Agenttel = brokerleadclient.Phone;
            //            oe2.BusId = product.Bussnessid;
            //            oe2.Busname = product.BussnessName;
            //            oe2.Customname = brokerleadclient.Clientname;
            //            oe2.Ordercode = _orderService.CreateOrderNumber(3); //创建成交订单
            //            oe2.OrderDetail = _orderDetailService.Create(ode2);//创建订单详情；
            //            oe2.Ordertype = EnumOrderType.成交订单;
            //            oe2.Remark = "前端经纪人提交";
            //            oe2.Shipstatus = (int)EnumBRECCType.审核中;
            //            oe2.Status = (int)EnumOrderStatus.默认;
            //            oe2.Upddate = DateTime.Now;
            //            oe2.Upduser = brokerleadclient.Adduser.ToString(CultureInfo.InvariantCulture);
            //            #endregion
            //
            //            #endregion

            //查询客户信息
            var sech2 = new ClientInfoSearchCondition
            {
                Clientname = brokerleadclient.Clientname,
                Phone = brokerleadclient.Phone.ToString(CultureInfo.InvariantCulture),
            };
            var cmodel2 = _clientInfoService.GetClientInfosByCondition(sech2).FirstOrDefault();

            //查询经纪人信息
            var broker = _brokerService.GetBrokerByUserId(brokerleadclient.Adduser);

            //创建代客流程
            var model = new BrokerLeadClientEntity();
            model.Broker = _brokerService.GetBrokerById(brokerleadclient.Adduser);
            model.ClientInfo = cmodel2;
            model.ClientName = brokerleadclient.Clientname;
            model.Appointmenttime = DateTime.Now;
            //model.Qq = Convert.ToInt32(brokerrecclient.Qq);
            model.Phone = brokerleadclient.Phone;       //客户电话
            model.Brokername = broker.Brokername;
            model.BrokerLevel = broker.Level.Name;
            model.Broker = broker;
            model.Adduser = brokerleadclient.Adduser;
            model.Addtime = DateTime.Now;
            model.Upuser = brokerleadclient.Adduser;
            model.Uptime = DateTime.Now;
            //model.ProjectId = brokerleadclient.Id;
            model.ProjectId = brokerleadclient.Projectid;
            model.Projectname = brokerleadclient.Projectname;
            model.Status = EnumBLeadType.预约中;
            model.DelFlag = EnumDelFlag.默认;
            model.ComOrder = (int)EnumOrderType.带客订单;

            //            model.ComOrder = _orderService.Create(oe).Id;      //添加代客订单；
            //            model.DealOrder = _orderService.Create(oe2).Id;       //添加成交订单

            _brokerleadclientService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }
        /// <summary>
        /// 传入经纪人带客管理参数,更新带客信息,返回更新结果转态信息成功提示"操作成功",失败提示"操作失败".
        /// </summary>
        /// <param name="model">经纪人带客管理参数</param>
        /// <returns>更新结果状态信息</returns>
        [HttpPost]
        [Description("更新经纪人带客管理信息")]
        public HttpResponseMessage UpdateLeadClient([FromBody]BrokerLeadClientModel model)
        {
            var entity = _brokerleadclientService.GetBrokerLeadClientById(model.Id);
            entity.Status = model.Status;
            if (model.Status == EnumBLeadType.等待上访)
            {
                entity.SecretaryId = _brokerService.GetBrokerByUserId(model.SecretaryId);
                //查询商品详情
                var product = _productService.GetProductById(model.Projectid);

                #region 创建带客订单 杨定鹏 2015年6月9日17:04:05
                //创建订单详情
                OrderDetailEntity ode = new OrderDetailEntity
                {
                    Adddate = DateTime.Now,
                    Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture),
                    Commission = product.Commission,
                    RecCommission = product.RecCommission,
                    Dealcommission = product.Dealcommission,
                    Price = product.Price,
                    Product = product,
                    Productname = product.Productname,
                    Upddate = DateTime.Now,
                    Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture)
                };
                //ode.Remark = product.
                //ode.Snapshoturl = orderDetailModel.Snapshoturl,

                //创建订单
                    OrderEntity oe = new OrderEntity
                    {
                        Adddate = DateTime.Now,
                        Adduser = model.Broker.ToString(),//model.Adduser.ToString(CultureInfo.InvariantCulture),
                        AgentId = model.Broker,//model.Adduser,
                        //HouseType = model.ProjectId == 0 ? "" : _productService.GetProductById(model.ProjectId).Productname,
                       // Agentname = model.Adduser == 0 ? "" : _brokerService.GetBrokerByUserId(model.Adduser).Brokername,
                        Agentname = model.Brokername,//_brokerService.GetBrokerByUserId(model.Adduser).Brokername,
                        Agenttel = model.Phone,
                        BusId = product.Bussnessid,
                        Busname = product.BussnessName,
                        Customname = model.Clientname,
                        Ordercode = _orderService.CreateOrderNumber(2),
                        OrderDetail = ode,
                        Ordertype = EnumOrderType.带客订单,
                        Remark = "前端经纪人提交",
                        Shipstatus = (int)EnumBRECCType.等待上访,
                        Status = (int)EnumOrderStatus.审核通过,
                        Upddate = DateTime.Now,
                        Upduser = model.Broker.ToString(),//model.Adduser.ToString(CultureInfo.InvariantCulture)
                    };
                    //创建带客订单号
                    //订单详情；
                    _orderService.Create(oe);

                #endregion

                }
            entity.Uptime = DateTime.Now;
            entity.Upuser = _workContext.CurrentUser.Id;
            _brokerleadclientService.Update(entity);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "操作成功"));
        }

    }
}
