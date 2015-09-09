using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ProductParameter;
using Community.Service.ProductParameter;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ProductParameterController : ApiController
	{
		private readonly IProductParameterService _productParameterService;

		public ProductParameterController(IProductParameterService productParameterService)
		{
			_productParameterService = productParameterService;
		}

		public ProductParameterModel Get(int id)
		{
			var entity =_productParameterService.GetProductParameterById(id);
			var model = new ProductParameterModel
			{
				Id = entity.Id,				
//                ParameterValue = entity.ParameterValue,			
//                Parameter = entity.Parameter,		
//                Product = entity.Product,		
                Sort = entity.Sort,		
                AddUser = entity.AddUser,	
                AddTime = entity.AddTime,	
                UpdUser = entity.UpdUser,	
                UpdTime = entity.UpdTime,		
            };
			return model;
		}

		public List<ProductParameterModel> Get(ProductParameterSearchCondition condition)
		{
			var model = _productParameterService.GetProductParametersByCondition(condition).Select(c=>new ProductParameterModel
			{
				Id = c.Id,
//				ParameterValue = c.ParameterValue,
//				Parameter = c.Parameter,
//				Product = c.Product,
				Sort = c.Sort,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
			return model;
		}

		public bool Post(ProductParameterModel model)
		{
			var entity = new ProductParameterEntity
			{
//				ParameterValue = model.ParameterValue,
//				Parameter = model.Parameter,
//				Product = model.Product,
				Sort = model.Sort,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
			};
			if(_productParameterService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ProductParameterModel model)
		{
			var entity = _productParameterService.GetProductParameterById(model.Id);
			if(entity == null)
				return false;
//			entity.ParameterValue = model.ParameterValue;
//			entity.Parameter = model.Parameter;
//			entity.Product = model.Product;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			if(_productParameterService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _productParameterService.GetProductParameterById(id);
			if(entity == null)
				return false;
			if(_productParameterService.Delete(entity))
				return true;
			return false;
		}
	}
}