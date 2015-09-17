using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Community.Entity.Model.Order;
using Community.Entity.Model.OrderDetail;
using Community.Entity.Model.Product;
using Community.Service.MemberAddress;
using Community.Service.Order;
using Community.Service.Product;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class CommunityOrderController : ApiController
	{
		private readonly IOrderService _orderService;
	    private readonly IProductService _productService;
	    private readonly IWorkContext _workContext;
	    private readonly IMemberAddressService _memberAddressService;

	    public CommunityOrderController(IOrderService orderService, IProductService productService, IWorkContext workContext,IMemberAddressService memberAddressService)
	    {
	        _orderService = orderService;
	        _productService = productService;
	        _workContext = workContext;
	        _memberAddressService = memberAddressService;
	    }

	    public OrderModel Get(int id)
		{
			var entity =_orderService.GetOrderById(id);
			var model = new OrderModel
			{
				Id = entity.Id,	
                No = entity.No,		
                Status = entity.Status,			
                CustomerName = entity.CustomerName,
                Remark = entity.Remark,	
                Adddate = entity.AddDate,	
                Adduser = entity.AddUser,	
                Upduser = entity.UpdUser,
                Upddate = entity.UpdDate,	
                Totalprice = entity.Totalprice,	
                Actualprice = entity.Actualprice,	
                Details = entity.Details.Select(c=>new OrderDetailModel
                {
                    Count = c.Count,
                    Id = c.Id,
                    Product = new ProductModel
                    {
                        Name = c.Product.Name,
                        Id = c.Product.Id
                    },
                    ProductName = c.ProductName,
                    UnitPrice = c.UnitPrice,
                    Remark = c.Remark,
                    Snapshoturl = c.Snapshoturl,
                    Totalprice = c.Totalprice,
                }).ToList(),
		        UserName = entity.AddMember.UserName
            };
			return model;
		}

		public HttpResponseMessage Get(OrderSearchCondition condition)
		{
			var model = _orderService.GetOrdersByCondition(condition).Select(c=>new OrderModel
			{
				Id = c.Id,
				No = c.No,
				Status = c.Status,
				CustomerName = c.CustomerName,
				Remark = c.Remark,
				Adddate = c.AddDate,
				Adduser = c.AddUser,
				Upduser = c.UpdUser,
				Upddate = c.UpdDate,
				Totalprice = c.Totalprice,
				Actualprice = c.Actualprice,
//				Details = c.Details,
                UserName = c.AddMember.UserName
			}).ToList();
		    var totalCount = _orderService.GetOrdersByCondition(condition);
			return PageHelper.toJson(new {List = model,Condition = condition,TotalCount=totalCount});
		}

		public bool Post([FromBody]OrderModel model)
		{
            //获取订单明细对应的商品
            var products = _productService.GetProductsByCondition(new ProductSearchCondition
            {
                Ids = model.Details.Select(c => c.Product.Id).ToArray(),
                Type = EnumProductType.Service
            }).ToList().Select(p => new OrderDetailEntity
            {
                Count = model.Details.First(d => d.Product.Id == p.Id).Count,
                Product = p,
                UnitPrice = p.Price,
                Adddate = DateTime.Now,
                Adduser = _workContext.CurrentUser.Id,
                ProductName = p.Name,
                Remark = "",
                Snapshoturl = "",
                Totalprice = model.Details.First(d => d.Product.Id == p.Id).Count * p.Price
            }).ToList();
            if (products.Count < 1)
                return false;

            //订单编号
            Random rd = new Random();
            var orderNumber = "O" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(100, 999);

			var entity = new OrderEntity
			{
                No = orderNumber,
				Status = EnumOrderStatus.Created,
				CustomerName = model.CustomerName,
				Remark = model.Remark,
                AddDate = DateTime.Now,
                AddUser = _workContext.CurrentUser.Id,
                UpdUser = _workContext.CurrentUser.Id,
                UpdDate = DateTime.Now,
                Totalprice = products.Sum(c => (c.Count * c.UnitPrice)),
                Actualprice = products.Sum(c => (c.Count * c.UnitPrice)),
				Details = products,
                Address = _memberAddressService.GetMemberAddressById(model.MemberAddressId)
			};
			if(_orderService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public HttpResponseMessage Put(OrderModel model)
		{
		    var allowEditRoles = new[]
		    {
                "superAdmin",
		        "admin"
		    };
			var entity = _orderService.GetOrderById(model.Id);
			if(entity == null)
				return PageHelper.toJson(PageHelper.ReturnValue(false,string.Format("无法获取到Id为{0}的订单",model.Id)));
		    if (_workContext.CurrentUser.Id != entity.AddUser &&
		        !((UserBase) _workContext.CurrentUser).UserRoles.Select(c => c.Role.RoleName)
		            .ToList()
		            .Intersect(allowEditRoles)
		            .Any())
                return PageHelper.toJson(PageHelper.ReturnValue(false, "当前用户没有权限修改此订单"));
			entity.Status = model.Status;
			entity.UpdUser = _workContext.CurrentUser.Id;
			entity.UpdDate = DateTime.Now;
		    return PageHelper.toJson(_orderService.Update(entity) != null ? PageHelper.ReturnValue(true, "操作成功") : PageHelper.ReturnValue(false, "操作失败，请查看日志"));
		}

        public HttpResponseMessage Delete(int id)
		{
			var entity = _orderService.GetOrderById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, string.Format("无法获取到Id为{0}的订单", id)));
            return PageHelper.toJson(_orderService.Delete(entity) ? PageHelper.ReturnValue(true, "操作成功") : PageHelper.ReturnValue(false, "操作失败，请查看日志"));
		}
	}
}