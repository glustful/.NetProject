using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ProductComment;
using Community.Service.ProductComment;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ProductCommentController : ApiController
	{
		private readonly IProductCommentService _productCommentService;

		public ProductCommentController(IProductCommentService productCommentService)
		{
			_productCommentService = productCommentService;
		}

		public ProductCommentModel Get(int id)
		{
			var entity =_productCommentService.GetProductCommentById(id);
			var model = new ProductCommentModel
			{
				Id = entity.Id,	
//                Product = entity.Product,	
                AddUser = entity.AddUser,		
                AddTime = entity.AddTime,		
                Content = entity.Content,		
                Stars = entity.Stars,			
            };
			return model;
		}

		public List<ProductCommentModel> Get(ProductCommentSearchCondition condition)
		{
			var model = _productCommentService.GetProductCommentsByCondition(condition).Select(c=>new ProductCommentModel
			{
				Id = c.Id,
//				Product = c.Product,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				Content = c.Content,
				Stars = c.Stars,
			}).ToList();
			return model;
		}

		public bool Post(ProductCommentModel model)
		{
			var entity = new ProductCommentEntity
			{
//				Product = model.Product,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				Content = model.Content,
				Stars = model.Stars,
			};
			if(_productCommentService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ProductCommentModel model)
		{
			var entity = _productCommentService.GetProductCommentById(model.Id);
			if(entity == null)
				return false;
//			entity.Product = model.Product;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.Content = model.Content;
			entity.Stars = model.Stars;
			if(_productCommentService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _productCommentService.GetProductCommentById(id);
			if(entity == null)
				return false;
			if(_productCommentService.Delete(entity))
				return true;
			return false;
		}
	}
}