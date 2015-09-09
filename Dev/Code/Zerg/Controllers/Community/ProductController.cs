using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Product;
using Community.Service.Product;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ProductController : ApiController
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		public ProductModel Get(int id)
		{
			var entity =_productService.GetProductById(id);
			var model = new ProductModel
			{
				Id = entity.Id,	
//                Category = entity.Category,	
//                Bussnessid = entity.Bussnessid,	
//                Bussnessname = entity.Bussnessname,	
                Price = entity.Price,		
                Name = entity.Name,			
                Status = entity.Status,		
                MainImg = entity.MainImg,		
                IsRecommend = entity.IsRecommend,
                Sort = entity.Sort,				
                Stock = entity.Stock,		
                Adduser = entity.AddUser,	
                Addtime = entity.AddTime,	
                UpdUser = entity.UpdUser,			
                UpdTime = entity.UpdTime,			
                Subtitte = entity.Subtitte,				
                Contactphone = entity.Contactphone,		
                Type = entity.Type,			
//                Detail = entity.Detail,		
                Comments = entity.Comments,		
//                Parameters = entity.Parameters,
            };
			return model;
		}

		public List<ProductModel> Get(ProductSearchCondition condition)
		{
			var model = _productService.GetProductsByCondition(condition).Select(c=>new ProductModel
			{
				Id = c.Id,
//				Category = c.Category,
//				Bussnessid = c.Bussnessid,
//				Bussnessname = c.Bussnessname,
				Price = c.Price,
				Name = c.Name,
				Status = c.Status,
				MainImg = c.MainImg,
				IsRecommend = c.IsRecommend,
				Sort = c.Sort,
				Stock = c.Stock,
				Adduser = c.AddUser,
				Addtime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
				Subtitte = c.Subtitte,
				Contactphone = c.Contactphone,
				Type = c.Type,
//				Detail = c.Detail,
				Comments = c.Comments,
//				Parameters = c.Parameters,
			}).ToList();
			return model;
		}

		public bool Post(ProductModel model)
		{
			var entity = new ProductEntity
			{
//				Category = model.Category,
//				Bussnessid = model.Bussnessid,
//				Bussnessname = model.Bussnessname,
				Price = model.Price,
				Name = model.Name,
				Status = model.Status,
				MainImg = model.MainImg,
				IsRecommend = model.IsRecommend,
				Sort = model.Sort,
				Stock = model.Stock,
				AddUser = model.Adduser,
				AddTime = model.Addtime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
				Subtitte = model.Subtitte,
				Contactphone = model.Contactphone,
				Type = model.Type,
//				Detail = model.Detail,
				Comments = model.Comments,
//				Parameters = model.Parameters,
			};
			if(_productService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ProductModel model)
		{
			var entity = _productService.GetProductById(model.Id);
			if(entity == null)
				return false;
//			entity.Category = model.Category;
//			entity.Bussnessid = model.Bussnessid;
//			entity.Bussnessname = model.Bussnessname;
			entity.Price = model.Price;
			entity.Name = model.Name;
			entity.Status = model.Status;
			entity.MainImg = model.MainImg;
			entity.IsRecommend = model.IsRecommend;
			entity.Sort = model.Sort;
			entity.Stock = model.Stock;
			entity.AddUser = model.Adduser;
			entity.AddTime = model.Addtime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			entity.Subtitte = model.Subtitte;
			entity.Contactphone = model.Contactphone;
			entity.Type = model.Type;
//			entity.Detail = model.Detail;
			entity.Comments = model.Comments;
//			entity.Parameters = model.Parameters;
			if(_productService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _productService.GetProductById(id);
			if(entity == null)
				return false;
			if(_productService.Delete(entity))
				return true;
			return false;
		}
	}
}