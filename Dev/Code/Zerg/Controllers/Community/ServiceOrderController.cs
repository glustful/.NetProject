using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Product;
using Community.Entity.Model.ServiceOrder;
using Community.Entity.Model.ServiceOrderDetail;
using Community.Service.Member;
using Community.Service.MemberAddress;
using Community.Service.Product;
using Community.Service.ServiceOrder;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ServiceOrderController : ApiController
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IMemberAddressService _memberAddressService;
        private readonly IMemberService _memberService;

        public ServiceOrderController(IServiceOrderService serviceOrderService, IProductService productService, IWorkContext workContext, IMemberAddressService memberAddressService, IMemberService memberService)
        {
            _serviceOrderService = serviceOrderService;
            _productService = productService;
            _workContext = workContext;
            _memberAddressService = memberAddressService;
            _memberService = memberService;
        }

        public ServiceOrderModel Get(int id)
        {
            var currentUser = (UserBase)_workContext.CurrentUser;
            var entity = _serviceOrderService.GetServiceOrderById(id);
            if (entity == null)
                return null;
            if (!currentUser.UserRoles.ToList()
    .Exists(c => c.Role.RoleName == "superAdmin" || c.Role.RoleName == "admin") )
   // .Exists(c => c.Role.RoleName == "superAdmin" || c.Role.RoleName == "admin") && ProductEntity.AddUser != currentUser.Id)
                return null;
            var model = new ServiceOrderModel
            {
                Id = entity.Id,
                OrderNo = entity.OrderNo,
                Addtime = entity.AddTime,
                AddUser = entity.AddUser,
                Flee = entity.Flee,
                Address = entity.Address.Address,
                Servicetime = entity.Servicetime,
                Remark = entity.Remark,
                Details = entity.Details.Select(c => new ServiceOrderDetailModel()
                {
                    Count = c.Count,
                    Id = c.Id,
                    Price = c.Price,
                    Product = new ProductModel
                    {
                        Id = c.Product.Id,
                        Name = c.Product.Name,
                        MainImg = c.Product.MainImg
                    }
                }).ToList(),
                Status = entity.Status,
                UpdUser = entity.UpdUser,
                UpdTime = entity.UpdTime,
                MemberAddressId = entity.Address.Id
            };
            return model;
        }

        public HttpResponseMessage Get([FromUri]ServiceOrderSearchCondition condition)
        {
            var currentUser = (UserBase)_workContext.CurrentUser;
            if (!currentUser.UserRoles.ToList()
                .Exists(c => c.Role.RoleName == "superAdmin" || c.Role.RoleName == "admin"))
                condition.AddUsers = new[] { currentUser.Id };
            var model = _serviceOrderService.GetServiceOrdersByCondition(condition).Select(c => new ServiceOrderModel
            {
                Id = c.Id,
                OrderNo = c.OrderNo,
                Addtime = c.AddTime,
                AddUser = c.AddUser,
                Flee = c.Flee,
                Address = c.Address.Address,
                Servicetime = c.Servicetime,
                Remark = c.Remark,
                Status = c.Status,
                UpdUser = c.UpdUser,
                UpdTime = c.UpdTime,
                UserName = c.AddMember.UserName,
                Details = c.Details.Select(d => new ServiceOrderDetailModel
                {
                    Count = d.Count,
                    Id = d.Id,
                    MainImg = d.MainImg,
                    Price = d.Price,
                    ProductName = d.ProductName,
                    Product = new ProductModel
                    {
                        Id = d.Product.Id,
                        MainImg = d.Product.MainImg,
                        Name = d.Product.Name,
                        Price = d.Product.Price,
                        OldPrice = d.Product.OldPrice
                    },
                }).ToList()
            }).ToList();
            var totalPages = _serviceOrderService.GetServiceOrderCount(condition);
            return PageHelper.toJson(new { List = model, Condition = condition, TotalPages = totalPages });
        }

        public HttpResponseMessage Post([FromBody]ServiceOrderModel model)
        {
            //获取订单明细对应的商品
            var products = _productService.GetProductsByCondition(new ProductSearchCondition
            {
                Ids = model.Details.Select(c => c.Product.Id).ToArray(),
                Type = EnumProductType.Service
            }).ToList().Select(p => new ServiceOrderDetailEntity
            {
                Count = model.Details.First(d => d.Product.Id == p.Id).Count,
                Product = p,
                Price = p.Price
            }).ToList();
            if (products.Count < 1)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "生成订单失败，无法找到服务商品"));
            //订单编号
            Random rd = new Random();
            var orderNumber = "S" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(100, 999);

            //订单实体
            var entity = new ServiceOrderEntity
            {
                OrderNo = orderNumber,
                AddTime = DateTime.Now,
                AddUser = _workContext.CurrentUser.Id,
                Flee = products.Sum(c => (c.Count * c.Price)),
                Address = _memberAddressService.GetMemberAddressById(model.MemberAddressId),
                Servicetime = model.Servicetime,
                Remark = model.Remark,
                Details = products,
                Status = EnumServiceOrderStatus.Created,
                UpdUser = _workContext.CurrentUser.Id,
                UpdTime = DateTime.Now,
                AddMember = _memberService.GetMemberByUserId(_workContext.CurrentUser.Id)
            };

            //保存
            if (_serviceOrderService.Create(entity).Id > 0)
            {
                //TODO:回掉接口写到Msg里，完成回掉方法
                return PageHelper.toJson(PageHelper.ReturnValue(true, "null", new ServiceOrderModel()
                {
                    Id = entity.Id,
                    OrderNo = entity.OrderNo,
                    Flee = entity.Flee,
                    Addtime = entity.AddTime
                }));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "生成服务订单失败"));
        }

        public bool Put([FromBody]ServiceOrderModel model)
        {
            var entity = _serviceOrderService.GetServiceOrderById(model.Id);
            if (entity == null)
                return false;
            entity.Status = model.Status;
            entity.UpdUser = _workContext.CurrentUser.Id;
            entity.UpdTime = DateTime.Now;
            return _serviceOrderService.Update(entity) != null;
        }

        public bool Delete(int id)
        {
            var entity = _serviceOrderService.GetServiceOrderById(id);
            if (entity == null)
                return false;
            return _serviceOrderService.Delete(entity);
        }
    }
}