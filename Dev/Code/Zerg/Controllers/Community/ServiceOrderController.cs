using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ServiceOrder;
using Community.Service.ServiceOrder;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
	public class ServiceOrderController : ApiController
	{
		private readonly IServiceOrderService _serviceOrderService;

		public ServiceOrderController(IServiceOrderService serviceOrderService)
		{
			_serviceOrderService = serviceOrderService;
		}

		public ServiceOrderModel Get(int id)
		{
			var entity =_serviceOrderService.GetServiceOrderById(id);
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
            };
			return model;
		}

		public List<ServiceOrderModel> Get(ServiceOrderSearchCondition condition)
		{
			var model = _serviceOrderService.GetServiceOrdersByCondition(condition).Select(c=>new ServiceOrderModel
			{
				Id = c.Id,
				OrderNo = c.OrderNo,
				Addtime = c.AddTime,
				AddUser = c.AddUser,
				Flee = c.Flee,
				Address = c.Address,
				Servicetime = c.Servicetime,
				Remark = c.Remark,
			}).ToList();
			return model;
		}

		public bool Post(ServiceOrderModel model)
		{
			var entity = new ServiceOrderEntity
			{
				OrderNo = model.OrderNo,
				AddTime = model.Addtime,
				AddUser = model.AddUser,
				Flee = model.Flee,
				Address = model.Address,
				Servicetime = model.Servicetime,
				Remark = model.Remark,
			};
			if(_serviceOrderService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ServiceOrderModel model)
		{
			var entity = _serviceOrderService.GetServiceOrderById(model.Id);
			if(entity == null)
				return false;
			entity.OrderNo = model.OrderNo;
			entity.AddTime = model.Addtime;
			entity.AddUser = model.AddUser;
			entity.Flee = model.Flee;
			entity.Address = model.Address;
			entity.Servicetime = model.Servicetime;
			entity.Remark = model.Remark;
			if(_serviceOrderService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _serviceOrderService.GetServiceOrderById(id);
			if(entity == null)
				return false;
			if(_serviceOrderService.Delete(entity))
				return true;
			return false;
		}
	}
}