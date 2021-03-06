using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ServiceOrderDetail;
using Community.Service.ServiceOrderDetail;
using Zerg.Models.Community;
using Zerg.Common;
using Community.Entity.Model.ProductParameter;
using System.Web.Http.Cors;
using System.Net.Http;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class ServiceOrderDetailController : ApiController
	{
		private readonly IServiceOrderDetailService _serviceOrderDetailService;

		public ServiceOrderDetailController(IServiceOrderDetailService serviceOrderDetailService)
		{
			_serviceOrderDetailService = serviceOrderDetailService;
		}

//		public ServiceOrderDetailModel Get(int id)
//		{
//			var entity =_serviceOrderDetailService.GetServiceOrderDetailById(id);
//			var model = new ServiceOrderDetailModel
//			{
//				Id = entity.Id,				
////                ServiceOrder = entity.ServiceOrder,	
////                Product = entity.Product,	
//                Count = entity.Count,		
//                Price = entity.Price,	
//            };
//			return model;
//		}

        public HttpResponseMessage Get([FromUri]ServiceOrderDetailSearchCondition condition)
		{
			var model = _serviceOrderDetailService.GetServiceOrderDetailsByCondition(condition).Select(c=>new 
			{
				Id = c.Id,
//				ServiceOrder = c.ServiceOrder,
                //Product = c.Product,
                ProductName = c.Product.Name,
                MainImg =c.Product.MainImg,
				Count = c.Count,
				Price = c.Price,
                OrderNo = c.ServiceOrder.OrderNo,
                proparameter = c.Product.Parameters.Select(o => o.Parameter.Name).FirstOrDefault(),
                propValue = c.Product.Parameters.Select(o => o.ParameterValue.Value).FirstOrDefault(),
               
			}).ToList();
            var count = _serviceOrderDetailService.GetServiceOrderDetailCount(condition);
            return PageHelper.toJson(new { List = model,TotalCount=count });
		}

//		public bool Post(ServiceOrderDetailModel model)
//		{
//			var entity = new ServiceOrderDetailEntity
//			{
////				ServiceOrder = model.ServiceOrder,
////				Product = model.Product,
//				Count = model.Count,
//				Price = model.Price,
//			};
//			if(_serviceOrderDetailService.Create(entity).Id > 0)
//			{
//				return true;
//			}
//			return false;
//		}

//		public bool Put(ServiceOrderDetailModel model)
//		{
//			var entity = _serviceOrderDetailService.GetServiceOrderDetailById(model.Id);
//			if(entity == null)
//				return false;
////			entity.ServiceOrder = model.ServiceOrder;
////			entity.Product = model.Product;
//			entity.Count = model.Count;
//			entity.Price = model.Price;
//			if(_serviceOrderDetailService.Update(entity) != null)
//				return true;
//			return false;
//		}
//
//		public bool Delete(int id)
//		{
//			var entity = _serviceOrderDetailService.GetServiceOrderDetailById(id);
//			if(entity == null)
//				return false;
//			if(_serviceOrderDetailService.Delete(entity))
//				return true;
//			return false;
//		}
	}
}