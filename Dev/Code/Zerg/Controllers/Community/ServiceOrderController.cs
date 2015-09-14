using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Order;
using Community.Entity.Model.Product;
using Community.Entity.Model.ServiceOrder;
using Community.Entity.Model.ServiceOrderDetail;
using Community.Service.Product;
using Community.Service.ServiceOrder;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ServiceOrderController : ApiController
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;

        public ServiceOrderController(IServiceOrderService serviceOrderService, IProductService productService, IWorkContext workContext)
        {
            _serviceOrderService = serviceOrderService;
            _productService = productService;
            _workContext = workContext;
        }

        public ServiceOrderModel Get(int id)
        {
            var entity = _serviceOrderService.GetServiceOrderById(id);
            if (entity == null)
                return null;
            var model = new ServiceOrderModel
            {
                Id = entity.Id,
                OrderNo = entity.OrderNo,
                Addtime = entity.AddTime,
                AddUser = entity.AddUser,
                Flee = entity.Flee,
                Address = entity.Address,
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
                UpdTime = entity.UpdTime
            };
            return model;
        }

        public HttpResponseMessage Get([FromUri]ServiceOrderSearchCondition condition)
        {
            var model = _serviceOrderService.GetServiceOrdersByCondition(condition).Select(c => new ServiceOrderModel
            {
                Id = c.Id,
                OrderNo = c.OrderNo,
                Addtime = c.AddTime,
                AddUser = c.AddUser,
                Flee = c.Flee,
                Address = c.Address,
                Servicetime = c.Servicetime,
                Remark = c.Remark,
                Status = c.Status,
                UpdUser = c.UpdUser,
                UpdTime = c.UpdTime
            }).ToList();
            var totalPages = _serviceOrderService.GetServiceOrderCount(condition);
            return PageHelper.toJson(new { List = model, Condition = condition, TotalPages = totalPages });
        }

        public bool Post([FromBody]ServiceOrderModel model)
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
                return false;
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
                Address = model.Address,
                Servicetime = model.Servicetime,
                Remark = model.Remark,
                Details = products,
                Status = EnumOrderStatus.Created,
                UpdUser = _workContext.CurrentUser.Id,
                UpdTime = DateTime.Now
            };

            //保存
            if (_serviceOrderService.Create(entity).Id > 0)
            {
                return true;
            }
            return false;
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