using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.OrderDetail;
using Community.Service.OrderDetail;
using Zerg.Models.Community;
using System.Web.Http.Cors;
using System.Net.Http;
using Zerg.Common;
using Community.Entity.Model.ProductParameter;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class OrderDetailController : ApiController
	{
		private readonly IOrderDetailService _orderDetailService;

		public OrderDetailController(IOrderDetailService orderDetailService)
		{
			_orderDetailService = orderDetailService;
		}

//		public OrderDetailModel Get(int id)
//		{
//			var entity =_orderDetailService.GetOrderDetailById(id);
//			var model = new OrderDetailModel
//			{
//				Id = entity.Id,	
////                Product = entity.Product,
//                ProductName = entity.ProductName,	
//                UnitPrice = entity.UnitPrice,	
//                Count = entity.Count,		
//                Snapshoturl = entity.Snapshoturl,	
//                Remark = entity.Remark,	
//                Adduser = entity.Adduser,	
//                Adddate = entity.Adddate,	
//                Upduser = entity.Upduser,	
//                Upddate = entity.Upddate,	
//                Totalprice = entity.Totalprice,	
////                Order = entity.Order,		
//            };
//			return model;
//		}

        public HttpResponseMessage Get([FromUri]OrderDetailSearchCondition condition)
		{
            var model = _orderDetailService.GetOrderDetailsByCondition(condition).Select(c => new
            {
                Id = c.Id,
                //				Product = c.Product,
                ProductName = c.ProductName,
                UnitPrice = c.UnitPrice,
                Count = c.Count,
                Snapshoturl = c.Snapshoturl,
                Remark = c.Remark,
                Adduser = c.Adduser,
                Adddate = c.Adddate,
                Upduser = c.Upduser,
                Upddate = c.Upddate,
                Totalprice = c.Totalprice,
                No = c.Order.No,
                Price = c.Product.Price,
                proparameter = c.Product.Parameters.Select(o => o.Parameter.Name).FirstOrDefault(),
                propValue = c.Product.Parameters.Select(o => o.ParameterValue.Value ).FirstOrDefault (),
                orderId=c.Order .Id 
                //				Order = c.Order,
            }).ToList();
            return PageHelper.toJson(new { List = model });
		}

//		public bool Post(OrderDetailModel model)
//		{
//			var entity = new OrderDetailEntity
//			{
////				Product = model.Product,
//				ProductName = model.ProductName,
//				UnitPrice = model.UnitPrice,
//				Count = model.Count,
//				Snapshoturl = model.Snapshoturl,
//				Remark = model.Remark,
//				Adduser = model.Adduser,
//				Adddate = model.Adddate,
//				Upduser = model.Upduser,
//				Upddate = model.Upddate,
//				Totalprice = model.Totalprice,
////				Order = model.Order,
//			};
//			if(_orderDetailService.Create(entity).Id > 0)
//			{
//				return true;
//			}
//			return false;
//		}

//		public bool Put(OrderDetailModel model)
//		{
//			var entity = _orderDetailService.GetOrderDetailById(model.Id);
//			if(entity == null)
//				return false;
////			entity.Product = model.Product;
//			entity.ProductName = model.ProductName;
//			entity.UnitPrice = model.UnitPrice;
//			entity.Count = model.Count;
//			entity.Snapshoturl = model.Snapshoturl;
//			entity.Remark = model.Remark;
//			entity.Adduser = model.Adduser;
//			entity.Adddate = model.Adddate;
//			entity.Upduser = model.Upduser;
//			entity.Upddate = model.Upddate;
//			entity.Totalprice = model.Totalprice;
////			entity.Order = model.Order;
//			if(_orderDetailService.Update(entity) != null)
//				return true;
//			return false;
//		}
//
//		public bool Delete(int id)
//		{
//			var entity = _orderDetailService.GetOrderDetailById(id);
//			if(entity == null)
//				return false;
//			if(_orderDetailService.Delete(entity))
//				return true;
//			return false;
//		}
	}
}