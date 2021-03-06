﻿using System.Globalization;
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
        #region 带客列表
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
        #endregion
        #region 查询当前经纪人带客记录
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
        #endregion
        #region 经纪人列表
       /// <summary>
       /// 经纪人列表
       /// </summary>
       /// <param name="status">带客推荐状态</param>
       /// <param name="brokername">经纪人名称</param>
       /// <param name="orderByAll">排序 参数{序号（OrderById），经纪人名（OrderByBrokername），客户名（OrderByClientName），
       ///客户电话（OrderByPhone），项目名称（OrderByProjectname），预约时间（OrderByAppointmenttime）}</param>
       /// <param name="isDes">是否降序</param>
       /// <param name="page"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLeadClientInfoByBrokerName(EnumBLeadType status, string brokername, EnumBrokerLeadClientSearchOrderBy orderByAll = EnumBrokerLeadClientSearchOrderBy .OrderByTime, bool isDes = true, int page = 1, int pageSize = 10)
        {

            var condition = new BrokerLeadClientSearchCondition
            {
                OrderBy = orderByAll,
                Page = page,
                PageCount = pageSize,
                Status = status,
                ClientName = brokername,
                isDescending =isDes 

            };

            var list = _brokerleadclientService.GetBrokerLeadClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Appointmenttime,
                a.Brokername,
                a.ClientInfo.Phone,
                a.Projectname,
                a.Addtime,
                a.ClientInfo.Clientname,

                SecretaryName = a.SecretaryId.Brokername,
                a.SecretaryPhone,
                Waiter = a.WriterId.Brokername,
                a.WriterPhone,
                a.Uptime

            }).ToList().Select(b => new
            {
                b.Id,
                b.Brokername,
                b.Clientname,

                b.Phone,
                b.Projectname,
                Appointmenttime = b.Appointmenttime.ToString("yyy-MM-dd"),
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
        #endregion
        #region 经纪人带客详情
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
                RegTime = model.Broker.Regtime.ToString("yyy-MM-dd"),
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
        #endregion
        #region  添加一个带客记录
        /// <summary>
        /// 添加一个带客记录
        /// </summary>
        /// <param name="brokerrecclient"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody] BrokerLeadClientModel brokerleadclient)
        {
            if (brokerleadclient.Adduser == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "经济人ID不能为空！"));
            if (brokerleadclient.Broker == 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "经纪人ID不能为空！"));
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
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该客户正在被带客！"));
            }

            //查询客户信息
            var sech2 = new ClientInfoSearchCondition
            {
                Clientname = brokerleadclient.Clientname, 
                Phone = brokerleadclient.Phone.ToString(CultureInfo.InvariantCulture),
            };
            var cmodel2 = _clientInfoService.GetClientInfosByCondition(sech2).FirstOrDefault();
            if (cmodel2 == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "带客出错！"));
            }
            //查询经纪人信息
            var broker = _brokerService.GetBrokerByUserId(brokerleadclient.Adduser);
            //创建代客流程
            var model = new BrokerLeadClientEntity();
            model.Broker = _brokerService.GetBrokerById(brokerleadclient.Adduser);
            model.ClientInfo = cmodel2;
            model.ClientName = brokerleadclient.Clientname;
            model.Appointmenttime = Convert.ToDateTime(brokerleadclient.Appointmenttime);
            //model.Qq = Convert.ToInt32(brokerrecclient.Qq);
            model.Phone = brokerleadclient.Phone;       //客户电话
            model.Brokername = broker.Brokername;
            //model.BrokerLevel = broker.Level.Name;
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
            model.Details = brokerleadclient.Note;

            _brokerleadclientService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }
        #endregion
        #region 更新带客流程状态
        /// <summary>
        /// 传入经纪人带客管理参数,更新带客信息,返回更新结果转态信息成功提示"操作成功",失败提示"操作失败".
        /// </summary>
        /// <param name="model">经纪人带客管理参数</param>
        /// <returns>更新结果状态信息</returns>
        [HttpPost]
        [Description("更新经纪人带客管理信息")]
        public HttpResponseMessage UpdateLeadClient([FromBody]BrokerLeadClientModel model)
        {
            if (model.Id == 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "Id不能为空"));
            }
            var entity = _brokerleadclientService.GetBrokerLeadClientById(model.Id);
            entity.Status = model.Status;
            model.Uptime = DateTime.Now;

            //if (model.Status == EnumBLeadType.等待上访) 
            //{
            //    entity.SecretaryId = _brokerService.GetBrokerByUserId(model.SecretaryId);
            //    //查询商品详情
            //    var product = _productService.GetProductById(model.Projectid);

            //    //创建订单详情
            //    OrderDetailEntity ode = new OrderDetailEntity
            //    {
            //        Adddate = DateTime.Now,
            //        Adduser = model.Adduser.ToString(CultureInfo.InvariantCulture),
            //        Commission = product.Commission,
            //        RecCommission = product.RecCommission,
            //        Dealcommission = product.Dealcommission,
            //        Price = product.Price,
            //        Product = product,
            //        Productname = product.Productname,
            //        Upddate = DateTime.Now,
            //        Upduser = model.Adduser.ToString(CultureInfo.InvariantCulture)
            //    };

            //    //创建订单
            //    OrderEntity oe = new OrderEntity
            //    {
            //        Adddate = DateTime.Now,
            //        Adduser = entity.Adduser.ToString(CultureInfo.InvariantCulture),
            //        AgentId = entity.Broker.Id,//model.Broker,//model.Adduser,
            //        Agenttel = model.Phone,
            //        Agentname = _brokerService.GetBrokerByUserId(entity.Adduser).Brokername,
            //        BusId = product.Bussnessid,
            //        Busname = product.BussnessName,
            //        Customname = entity.ClientInfo.Clientname,
            //        Ordercode = _orderService.CreateOrderNumber(2),
            //        OrderDetail = ode,
            //        Ordertype = EnumOrderType.带客订单,
            //        Remark = "前端经纪人提交",
            //        Shipstatus = (int)EnumBLeadType.等待上访,
            //        Status = (int)EnumOrderStatus.审核通过,
            //        Upddate = DateTime.Now,
            //        Upduser = entity.Adduser.ToString(),//model.Adduser.ToString(CultureInfo.InvariantCulture)
            //    };
            //    _orderService.Create(oe);
            //    entity.ComOrder = oe.Id;
            //    _brokerleadclientService.Update(entity);
            //}
            //else if (model.Status == EnumBLeadType.预约不通过) { return PageHelper.toJson(PageHelper.ReturnValue(false, "预约不通过")); }
            //else if (model.Status == EnumBLeadType.预约中) {}
            //else if (model.Status == EnumBLeadType.洽谈中) 
            //{
            //    var comOrder = _orderService.GetOrderById(entity.ComOrder);
            //    //变更订单状态
            //    int a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
            //    comOrder.Shipstatus = a;
            //    //comOrder.Shipstatus = (int)EnumBRECCType.洽谈中;
            //    comOrder.Status = (int)EnumOrderStatus.审核通过;
            //    comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    comOrder.Upddate = DateTime.Now;
            //    _orderService.Update(comOrder);
            //}
            //else if (model.Status == EnumBLeadType.洽谈成功) 
            //{
            //    var comOrder = _orderService.GetOrderById(entity.ComOrder);
            //    //变更订单状态
            //    int a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
            //    comOrder.Shipstatus = a;
            //    comOrder.Status = (int)EnumOrderStatus.审核通过;
            //    comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    comOrder.Upddate = DateTime.Now;
            //    _orderService.Update(comOrder);

            //}
            //else if (model.Status == EnumBLeadType.洽谈失败) 
            //{
            //    var comOrder = _orderService.GetOrderById(entity.ComOrder);
            //    //变更订单状态
            //    int a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
            //    comOrder.Shipstatus = a;
            //    //comOrder.Shipstatus = (int)EnumBRECCType.洽谈失败;
            //    comOrder.Status = (int)EnumOrderStatus.审核失败;
            //    comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    comOrder.Upddate = DateTime.Now;
            //    model.DelFlag = (int)EnumDelFlag.删除;
            //    _orderService.Update(comOrder);
            //}
            //else if (model.Status == EnumBLeadType.客人未到) 
            //{
            //    //订单作废
            //    var comOrder = _orderService.GetOrderById(entity.ComOrder);
            //    //变更订单状态
            //    int a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
            //    comOrder.Shipstatus = a;
            //    //comOrder.Shipstatus = (int)EnumBRECCType.客人未到;
            //    comOrder.Status = (int)EnumOrderStatus.审核失败;
            //    comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
            //    comOrder.Upddate = DateTime.Now;
            //    _orderService.Update(comOrder);
            //}
            switch (model.Status) 
            {
              
                case EnumBLeadType.预约中:
                    break;
                case EnumBLeadType.预约不通过:
                    _brokerleadclientService.Delete(entity);
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "操作成功"));
                case EnumBLeadType.等待上访:
                        entity.SecretaryId = _brokerService.GetBrokerByUserId(model.SecretaryId);
                        //查询商品详情
                        var product = _productService.GetProductById(model.Projectid);

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

                        //创建订单
                        OrderEntity oe = new OrderEntity
                        {
                            Adddate = DateTime.Now,
                            Adduser = entity.Adduser.ToString(CultureInfo.InvariantCulture),
                            AgentId = entity.Broker.Id,//model.Broker,//model.Adduser,
                            Agenttel = model.Phone,
                            Agentname = _brokerService.GetBrokerByUserId(entity.Adduser).Brokername,
                            BusId = product.Bussnessid,
                            Busname = product.BussnessName,
                            Customname = entity.ClientInfo.Clientname,
                            Ordercode = _orderService.CreateOrderNumber(2),
                            OrderDetail = ode,
                            Ordertype = EnumOrderType.带客订单,
                            Remark = "前端经纪人提交",
                            Shipstatus = (int)EnumBLeadType.等待上访,
                            Status = (int)EnumOrderStatus.审核通过,
                            Upddate = DateTime.Now,
                            Upduser = entity.Adduser.ToString(),//model.Adduser.ToString(CultureInfo.InvariantCulture)
                        };
                        _orderService.Create(oe);
                        entity.ComOrder = oe.Id;
                        _brokerleadclientService.Update(entity);
                    break;
                case EnumBLeadType.洽谈中:
                        var comOrder = _orderService.GetOrderById(entity.ComOrder);
                        //变更订单状态
                        int a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
                        comOrder.Shipstatus = a;
                        //comOrder.Shipstatus = (int)EnumBRECCType.洽谈中;
                        comOrder.Status = (int)EnumOrderStatus.审核通过;
                        comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        comOrder.Upddate = DateTime.Now;
                        _orderService.Update(comOrder);
                    break;
                case EnumBLeadType.洽谈成功:
                        comOrder = _orderService.GetOrderById(entity.ComOrder);
                        //变更订单状态
                        a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
                        comOrder.Shipstatus = a;
                        comOrder.Status = (int)EnumOrderStatus.审核通过;
                        comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        comOrder.Upddate = DateTime.Now;
                        _orderService.Update(comOrder);
                    break;
                case EnumBLeadType.洽谈失败:
                        comOrder = _orderService.GetOrderById(entity.ComOrder);
                        //变更订单状态
                        a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
                        comOrder.Shipstatus = a;
                        //comOrder.Shipstatus = (int)EnumBRECCType.洽谈失败;
                        comOrder.Status = (int)EnumOrderStatus.审核失败;
                        comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                        comOrder.Upddate = DateTime.Now;
                        model.DelFlag = (int)EnumDelFlag.删除;
                        _orderService.Update(comOrder);
                    break;
                case EnumBLeadType.客人未到:
                    //订单作废
                    comOrder = _orderService.GetOrderById(entity.ComOrder);
                    //变更订单状态
                    a = (int)Enum.Parse(typeof(EnumBLeadType), model.Status.ToString());
                    comOrder.Shipstatus = a;
                    //comOrder.Shipstatus = (int)EnumBRECCType.客人未到;
                    comOrder.Status = (int)EnumOrderStatus.审核失败;
                    comOrder.Upduser = _workContext.CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
                    comOrder.Upddate = DateTime.Now;
                    model.DelFlag = (int)EnumDelFlag.删除;
                    _orderService.Update(comOrder);
                    break;
            }
            entity.Uptime = DateTime.Now;
            entity.Upuser = _workContext.CurrentUser.Id;
            _brokerleadclientService.Update(entity);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "操作成功"));
        }
        #endregion
    }

}
