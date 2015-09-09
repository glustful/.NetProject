using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Order;
using Community.Service.Order;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class OrderController : ApiController
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
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
//                Details = entity.Details,		
            };
			return model;
		}

		public List<OrderModel> Get(OrderSearchCondition condition)
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
			}).ToList();
			return model;
		}

		public bool Post(OrderModel model)
		{
			var entity = new OrderEntity
			{
				No = model.No,
				Status = model.Status,
				CustomerName = model.CustomerName,
				Remark = model.Remark,
				AddDate = model.Adddate,
				AddUser = model.Adduser,
				UpdUser = model.Upduser,
				UpdDate = model.Upddate,
				Totalprice = model.Totalprice,
				Actualprice = model.Actualprice,
//				Details = model.Details,
			};
			if(_orderService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(OrderModel model)
		{
			var entity = _orderService.GetOrderById(model.Id);
			if(entity == null)
				return false;
			entity.No = model.No;
			entity.Status = model.Status;
			entity.CustomerName = model.CustomerName;
			entity.Remark = model.Remark;
			entity.AddDate = model.Adddate;
			entity.AddUser = model.Adduser;
			entity.UpdUser = model.Upduser;
			entity.UpdDate = model.Upddate;
			entity.Totalprice = model.Totalprice;
			entity.Actualprice = model.Actualprice;
//			entity.Details = model.Details;
			if(_orderService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _orderService.GetOrderById(id);
			if(entity == null)
				return false;
			if(_orderService.Delete(entity))
				return true;
			return false;
		}
	}
}